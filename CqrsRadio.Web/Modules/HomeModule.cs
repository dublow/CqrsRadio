using System;
using System.Linq;
using CqrsRadio.Domain.Aggregates;
using CqrsRadio.Domain.Events;
using CqrsRadio.Domain.EventStores;
using CqrsRadio.Domain.Handlers;
using CqrsRadio.Domain.Services;
using CqrsRadio.Domain.ValueTypes;
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
                    .Any(x => x.Identity.UserId == UserId.Parse(model.UserId));

                var user = userExists 
                    ? new User(eventStream, eventPublisher) 
                    : User.Create(eventStream, eventPublisher, model.Email, model.Nickname, model.UserId, model.AccessToken);


                var playlistId = deezerApi.CreatePlaylist(user.Identity.AccessToken, user.Identity.UserId, model.PlaylistName);
                user.AddPlaylist(playlistId, model.PlaylistName);
                
                return Response.AsJson(user.Playlist);
            };
        }
    }
}
