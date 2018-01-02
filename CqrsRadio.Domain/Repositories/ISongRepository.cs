using System.Collections.Generic;
using CqrsRadio.Domain.Entities;
using CqrsRadio.Domain.ValueTypes;

namespace CqrsRadio.Domain.Repositories
{
    public interface ISongRepository : IRepository
    {
        void Add(UserId userId, PlaylistId playlistId, SongId songId, string title, string artist);
        IEnumerable<Song> GetRandomSongs(int size);
    }
}