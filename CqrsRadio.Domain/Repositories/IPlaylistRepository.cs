using CqrsRadio.Domain.ValueTypes;

namespace CqrsRadio.Domain.Repositories
{
    public interface IPlaylistRepository    
    {
        void Add(UserId userId, PlaylistId isAny, string name);
        void Delete(UserId userId, PlaylistId isAny, string name);
    }
}