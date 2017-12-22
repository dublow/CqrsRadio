using System;
using CqrsRadio.Common.StatsD;
using CqrsRadio.Domain.Aggregates;
using CqrsRadio.Domain.Repositories;
using CqrsRadio.Domain.Services;
using CqrsRadio.Domain.ValueTypes;
using CqrsRadio.Handlers;
using CqrsRadio.Infrastructure.Bus;
using CqrsRadio.Infrastructure.EventStores;
using CqrsRadio.Web.Models;
using Nancy;
using Nancy.ModelBinding;
using Environment = CqrsRadio.Domain.Configuration.Environment;

namespace CqrsRadio.Web.Modules
{
    public class HomeModule : NancyModule
    {
        private readonly IDeezerApi _deezerApi;
        private readonly ISongRepository _songRepository;
        private readonly IPlaylistRepository _playlistRepository;
        private readonly Environment _environment;
        private readonly IMetric _metric;

        public HomeModule(IDeezerApi deezerApi, ISongRepository songRepository, 
            IPlaylistRepository playlistRepository, Environment environment,
            IMetric metric)
        {
            _deezerApi = deezerApi;
            _songRepository = songRepository;
            _playlistRepository = playlistRepository;
            _environment = environment;
            _metric = metric;
            var eventStream = new MemoryEventStream();
            var eventPublisher = new EventBus(eventStream);

            eventPublisher.Subscribe(new PlaylistHandler(playlistRepository));

            Get["/"] = _ =>
            {
                using (var timer = _metric.Timer("home"))
                {
                    var model = new
                    {
                        appid = _environment.AppId,
                        channel = _environment.Channel
                    };

                    return View["index", model];
                }   
            };

            Get["/channel"] = _ =>
            {
                using (var timer = _metric.Timer("channel"))
                {
                    var cacheExpire = 60 * 60 * 24 * 365;

                    return View["channel"]
                        .WithHeader("Pragma", "public")
                        .WithHeader("Cache-Control", $"maxage={cacheExpire}")
                        .WithHeader("Expires", DateTime.Now.AddMinutes(cacheExpire).ToString("F"));
                }
            };

            Post["/login"] = _ =>
            {
                using (var timer = _metric.Timer("createplaylist"))
                {
                    var model = this.Bind<LoginViewModel>();

                    var user = User.Create(eventStream, eventPublisher, _deezerApi, _songRepository,
                        _playlistRepository, model.Email, model.Nickname, model.UserId, model.AccessToken,
                        _environment.Size);

                    user.AddPlaylist(model.PlaylistName);
                    Console.WriteLine(user.Playlist.PlaylistId.Value);
                    return Response.AsJson(new {playlistCreated = !user.Playlist.PlaylistId.IsEmpty});
                }
            };

            Get["/canCreatePlaylist/{userId}"] = parameters =>
            {
                using (var timer = _metric.Timer("hasplaylist"))
                {
                    var userId = UserId.Parse((string) parameters.userId);
                    var canCreatePlaylist = _playlistRepository
                        .CanCreatePlaylist(userId, DateTime.UtcNow.AddDays(-1));

                    return Response.AsJson(new {canCreatePlaylist});
                }
            };
        }
    }
}
