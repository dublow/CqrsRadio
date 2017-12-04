using CqrsRadio.Domain.ValueTypes;

namespace CqrsRadio.Domain.Repositories
{
    public interface ISongRepository
    {
        void Add(UserId userId, string playlistName, string title, string artist);
    }
}