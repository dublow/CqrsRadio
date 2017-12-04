using CqrsRadio.Domain.ValueTypes;

namespace CqrsRadio.Domain.Repositories
{
    public interface IPlaylistRepository    
    {
        void Add(UserId userId, string name);
        void Delete(UserId userId, string name);
    }
}