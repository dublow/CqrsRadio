using System;
using System.Linq;
using CqrsRadio.Domain.Aggregates;
using CqrsRadio.Domain.Events;
using CqrsRadio.Domain.EventStores;
using CqrsRadio.Domain.Handlers;
using CqrsRadio.Domain.Services;
using CqrsRadio.Domain.ValueTypes;
using CqrsRadio.Infrastructure.Bus;
using CqrsRadio.Infrastructure.EventStores;
using CqrsRadio.Web.Models;
using Nancy;
using Nancy.ModelBinding;

namespace CqrsRadio.Web.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule(IDeezerApi deezerApi)
        {
            var eventStream = new MemoryEventStream();
            var eventPublisher = new EventBus(eventStream);

            Get["/"] = _ => View["index"];
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

                var user = User.Create(eventStream, eventPublisher, model.Email, model.Nickname, model.UserId, model.AccessToken);

                var playlistId = deezerApi.CreatePlaylist(user.Identity.AccessToken, user.Identity.UserId, model.PlaylistName);
                user.AddPlaylist(playlistId, model.PlaylistName);
                
                return Response.AsJson(user.Playlist);
            };
        }
    }
}
