using CqrsRadio.Domain.ValueTypes;

namespace CqrsRadio.Domain.Repositories
{
    public interface IPlaylistRepository    
    {
        void AddPlaylist(UserId userId, string name);
    }
}