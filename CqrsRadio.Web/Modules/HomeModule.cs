using System;
using CqrsRadio.Common.StatsD;
using CqrsRadio.Domain.Aggregates;
using CqrsRadio.Domain.Configuration;
using CqrsRadio.Domain.Repositories;
using CqrsRadio.Domain.Services;
using CqrsRadio.Domain.ValueTypes;
using CqrsRadio.Handlers;
using CqrsRadio.Infrastructure.Bus;
using CqrsRadio.Infrastructure.EventStores;
using CqrsRadio.Web.Models;
using Nancy;
using Nancy.ModelBinding;

namespace CqrsRadio.Web.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule(IDeezerApi deezerApi, ISongRepository songRepository, 
            IPlaylistRepository playlistRepository, RadioEnvironment environment,
            IMetric metric)
        {
            var eventStream = new MemoryEventStream();
            var eventPublisher = new EventBus(eventStream);

            eventPublisher.Subscribe(new PlaylistHandler(playlistRepository));

            Get["/"] = _ =>
            {
                using (var timer = metric.Timer("home"))
                {
                    var model = new
                    {
                        appid = environment.AppId,
                        channel = environment.Channel
                    };

                    return View["index", model];
                }   
            };

            Get["/channel"] = _ =>
            {
                using (var timer = metric.Timer("channel"))
                {
                    var cacheExpire = 60 * 60 * 24 * 365;

                    return View["channel"]
                        .WithHeader("Pragma", "public")
                        .WithHeader("Cache-Control", $"maxage={cacheExpire}")
                        .WithHeader("Expires", DateTime.Now.AddMinutes(cacheExpire).ToString("F"));
                }
            };

            Post["/createPlaylist"] = _ =>
            {
                using (var timer = metric.Timer("createplaylist"))
                {
                    var model = this.Bind<LoginViewModel>();

                    var user = User.Create(eventStream, eventPublisher, deezerApi, songRepository,
                        playlistRepository, model.Email, model.Nickname, model.UserId, model.AccessToken,
                        environment.Size);

                    user.AddPlaylist(model.PlaylistName);
                    return Response.AsJson(new {playlistCreated = !user.Playlist.PlaylistId.IsEmpty});
                }
            };

            Get["/canCreatePlaylist/{userId}"] = parameters =>
            {
                using (var timer = metric.Timer("hasplaylist"))
                {
                    var userId = UserId.Parse((string) parameters.userId);
                    var canCreatePlaylist = playlistRepository
                        .CanCreatePlaylist(userId, DateTime.UtcNow.AddDays(-1));

                    return Response.AsJson(new {canCreatePlaylist});
                }
            };
        }
    }
}
