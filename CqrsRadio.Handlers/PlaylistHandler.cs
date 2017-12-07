using CqrsRadio.Domain.EventHandlers;
using CqrsRadio.Domain.Events;
using CqrsRadio.Domain.Repositories;

namespace CqrsRadio.Handlers
{
    public class PlaylistHandler : IPlaylistHandler
    {
        private readonly IPlaylistRepository _playlistRepository;

        public PlaylistHandler(IPlaylistRepository playlistRepository)
        {
            _playlistRepository = playlistRepository;
        }

        public void Handle(PlaylistAdded evt)
        {
            _playlistRepository.Add(evt.UserId, evt.PlaylistId, evt.Name);
        }

        public void Handle(PlaylistDeleted evt)
        {
            _playlistRepository.Delete(evt.UserId, evt.PlaylistId, evt.Name);
        }
    }
}
