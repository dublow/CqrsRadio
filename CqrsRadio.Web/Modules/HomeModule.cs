using System;
using CqrsRadio.Domain.Aggregates;
using CqrsRadio.Domain.Repositories;
using CqrsRadio.Domain.Services;
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

        public HomeModule(IDeezerApi deezerApi, ISongRepository songRepository, 
            IPlaylistRepository playlistRepository, Environment environment)
        {
            _deezerApi = deezerApi;
            _songRepository = songRepository;
            _playlistRepository = playlistRepository;
            _environment = environment;
            var eventStream = new MemoryEventStream();
            var eventPublisher = new EventBus(eventStream);

            eventPublisher.Subscribe(new PlaylistHandler(playlistRepository));

            Get["/"] = _ =>
            {
                var model = new
                {
                    appid = _environment.AppId,
                    channel = _environment.Channel
                };
                return View["index", model];
            };
            Get["/channel"] = _ =>
            {
                var cacheExpire = 60 * 60 * 24 * 365;

                return View["channel"]
                    .WithHeader("Pragma", "public")
                    .WithHeader("Cache-Control", $"maxage={cacheExpire}")
                    .WithHeader("Expires", DateTime.Now.AddMinutes(cacheExpire).ToString("F"));
            };

            Post["/login"] = _ =>
            {
                var model = this.Bind<LoginViewModel>();
                
                var user = User.Create(eventStream, eventPublisher, _deezerApi, _songRepository,
                    _playlistRepository, model.Email, model.Nickname, model.UserId, model.AccessToken, _environment.Size);

                user.AddPlaylist(model.PlaylistName);
                
                return Response.AsJson(user.Playlist);
            };
        }
    }
}
