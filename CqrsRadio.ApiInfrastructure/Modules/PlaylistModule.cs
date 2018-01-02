using System;
using CqrsRadio.Domain.Repositories;
using CqrsRadio.Domain.ValueTypes;
using Nancy;

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
        }
    }
}
