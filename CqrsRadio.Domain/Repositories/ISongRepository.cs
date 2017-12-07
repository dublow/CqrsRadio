using CqrsRadio.Domain.ValueTypes;

namespace CqrsRadio.Domain.Repositories
{
    public interface ISongRepository
    {
        void Add(UserId userId, string playlistName, SongId songId, string title, string artist);
    }
}