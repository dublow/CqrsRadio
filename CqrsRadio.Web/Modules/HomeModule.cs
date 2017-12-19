using System;
using System.Configuration;
using CqrsRadio.Domain.Aggregates;
using CqrsRadio.Domain.Repositories;
using CqrsRadio.Domain.Services;
using CqrsRadio.Handlers;
using CqrsRadio.Infrastructure.Bus;
using CqrsRadio.Infrastructure.EventStores;
using CqrsRadio.Web.Models;
using Nancy;
using Nancy.Extensions;
using Nancy.ModelBinding;

namespace CqrsRadio.Web.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule(IDeezerApi deezerApi, ISongRepository songRepository, IPlaylistRepository playlistRepository)
        {
            var songSize = int.Parse(ConfigurationManager.AppSettings["songSize"]);
            var eventStream = new MemoryEventStream();
            var eventPublisher = new EventBus(eventStream);

            eventPublisher.Subscribe(new PlaylistHandler(playlistRepository));

            Get["/"] = _ =>
            {
                var local = Request.IsLocal() ? "local" : "";
                var model = new
                {
                    appid = ConfigurationManager.AppSettings[$"appid{local}"],
                    channel = ConfigurationManager.AppSettings[$"channel{local}"]
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
                

                var user = User.Create(eventStream, eventPublisher, deezerApi, songRepository,
                    playlistRepository, model.Email, model.Nickname, model.UserId, model.AccessToken, songSize);

                user.AddPlaylist(model.PlaylistName);
                
                return Response.AsJson(user.Playlist);
            };
        }
    }
}
