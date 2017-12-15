using System;
using CqrsRadio.Domain.ValueTypes;

namespace CqrsRadio.Domain.Repositories
{
    public interface IPlaylistRepository    
    {
        void Add(UserId userId, PlaylistId isAny, string name);
        void Update(UserId userId);
        void Delete(UserId userId, PlaylistId isAny, string name);
        bool CanCreatePlaylist(UserId userId, DateTime interval);
        bool Exists(UserId userId);
    }
}