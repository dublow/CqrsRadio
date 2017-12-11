using CqrsRadio.Domain.ValueTypes;

namespace CqrsRadio.Domain.Repositories
{
    public interface ISongRepository
    {
        void Add(UserId userId, PlaylistId playlistId, SongId songId, string title, string artist);
    }
}