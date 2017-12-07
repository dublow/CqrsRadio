using System;
using System.Linq;
using CqrsRadio.Domain.Aggregates;
using CqrsRadio.Domain.Events;
using CqrsRadio.Domain.EventStores;
using CqrsRadio.Domain.Handlers;
using CqrsRadio.Domain.Services;
using CqrsRadio.Web.Models;
using Nancy;
using Nancy.ModelBinding;

namespace CqrsRadio.Web.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule(IEventStream eventStream, IEventPublisher eventPublisher, IDeezerApi deezerApi)
        {
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

                var userExists = eventStream.GetEvents().OfType<UserCreated>()
                    .Any(x => x.Identity.UserId == model.UserId);

                var user = userExists 
                    ? new User(eventStream, eventPublisher) 
                    : User.Create(eventStream, eventPublisher, model.Email, model.Nickname, model.UserId);

                user.ClearPlaylists();

                user.AddAccessToken(model.AccessToken);
                user.AddPlaylist(model.PlaylistName);

                deezerApi.CreatePlaylist(user.AccessToken, user.Identity.UserId, model.PlaylistName);
                var playlists = deezerApi.GetPlaylistIdsByUserId(user.AccessToken, user.Identity.UserId);

                return Response.AsJson(user.GetPlaylist(model.PlaylistName));
            };
        }
    }
}
