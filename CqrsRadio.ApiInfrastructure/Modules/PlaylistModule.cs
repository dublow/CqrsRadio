using System;
using CqrsRadio.ApiInfrastructure.ViewModel;
using CqrsRadio.Domain.Repositories;
using CqrsRadio.Domain.ValueTypes;
using Nancy;
using Nancy.ModelBinding;

namespace CqrsRadio.ApiInfrastructure.Modules
{
    public class PlaylistModule : NancyModule
    {
        public PlaylistModule(IPlaylistRepository playlistRepository) : base("Playlist")
        {
            Get["/CanCreatePlaylist/{userId}/{interval}"] = parameters =>
            {
                var userId = UserId.Parse((string) parameters.userId);
                var interval = DateTime.Parse((string) parameters.interval);

                var canCreate = playlistRepository.CanCreatePlaylist(userId, interval);

                return Response.AsJson(new {success = true, result = canCreate});
            };

            Get["/Exists/{userId}"] = parameters =>
            {
                var userId = UserId.Parse((string)parameters.userId);

                var exists = playlistRepository.Exists(userId);

                return Response.AsJson(new { success = true, result = exists });
            };

            Post["/Add"] = _ =>
            {
                var model = this.Bind<AddPlaylist>();
                playlistRepository.Add(UserId.Parse(model.UserId),PlaylistId.Parse(model.PlaylistId), model.Name);

                return Response.AsJson(new { success = true }); ;
            };

            Post["/Update"] = _ =>
            {
                var model = this.Bind<UpdatePlaylist>();
                playlistRepository.Update(UserId.Parse(model.UserId));

                return Response.AsJson(new { success = true }); ;
            };
        }
    }
}
