using CqrsRadio.Domain.ValueTypes;

namespace CqrsRadio.Domain.Services
{
    public interface IDeezerApi
    {
        void CreatePlaylist(UserId userId, string playlistName);
    }
}
