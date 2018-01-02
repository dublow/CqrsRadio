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
        public HomeModule(IDeezerApi deezerApi, ISongRepository songRepository, 
            IPlaylistRepository playlistRepository, Environment environment,
            IMetric metric)
        {
            var playlistRepository1 = playlistRepository;
            var environment1 = environment;
            var metric1 = metric;
            var eventStream = new MemoryEventStream();
            var eventPublisher = new EventBus(eventStream);

            eventPublisher.Subscribe(new PlaylistHandler(playlistRepository));

            Get["/"] = _ =>
            {
                using (var timer = metric1.Timer("home"))
                {
                    var model = new
                    {
                        appid = environment1.AppId,
                        channel = environment1.Channel
                    };

                    return View["index", model];
                }   
            };

            Get["/channel"] = _ =>
            {
                using (var timer = metric1.Timer("channel"))
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
                using (var timer = metric1.Timer("createplaylist"))
                {
                    var model = this.Bind<LoginViewModel>();

                    var user = User.Create(eventStream, eventPublisher, deezerApi, songRepository,
                        playlistRepository1, model.Email, model.Nickname, model.UserId, model.AccessToken,
                        environment1.Size);

                    user.AddPlaylist(model.PlaylistName);
                    Console.WriteLine(user.Playlist.PlaylistId.Value);
                    return Response.AsJson(new {playlistCreated = !user.Playlist.PlaylistId.IsEmpty});
                }
            };

            Get["/canCreatePlaylist/{userId}"] = parameters =>
            {
                using (var timer = metric1.Timer("hasplaylist"))
                {
                    var userId = UserId.Parse((string) parameters.userId);
                    var canCreatePlaylist = playlistRepository1
                        .CanCreatePlaylist(userId, DateTime.UtcNow.AddDays(-1));

                    return Response.AsJson(new {canCreatePlaylist});
                }
            };
        }
    }
}
