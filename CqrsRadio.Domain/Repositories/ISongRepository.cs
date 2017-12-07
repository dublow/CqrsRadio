using CqrsRadio.Domain.ValueTypes;

namespace CqrsRadio.Domain.Repositories
{
    public interface ISongRepository
    {
        void Add(UserId userId, string playlistName, SongId songId, string genre, string title, string artist);
    }
}