using CqrsRadio.Domain.EventHandlers;
using CqrsRadio.Domain.Events;
using CqrsRadio.Domain.Repositories;

namespace CqrsRadio.Handlers
{
    public class SongHandler : ISongHandler
    {
        private readonly ISongRepository _songRepository;

        public SongHandler(ISongRepository songRepository)
        {
            _songRepository = songRepository;
        }

        public void Handle(SongAdded evt)
        {
            _songRepository.Add(evt.UserId, evt.PlaylistId, evt.SongId, evt.Title, evt.Artist);
        }
    }
}