using System;
using CqrsRadio.Domain.ValueTypes;

namespace CqrsRadio.Domain.Repositories
{
    public interface IPlaylistRepository : IRepository
    {
        void Add(UserId userId, PlaylistId playlistId, string name);
        void Update(UserId userId);
        void Delete(UserId userId, PlaylistId playlistId, string name);
        bool CanCreatePlaylist(UserId userId, DateTime interval);
        bool Exists(UserId userId);
    }
}