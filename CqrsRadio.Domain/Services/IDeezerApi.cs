using System.Collections.Generic;
using CqrsRadio.Domain.Entities;
using CqrsRadio.Domain.ValueTypes;

namespace CqrsRadio.Domain.Services
{
    public interface IDeezerApi
    {
        void CreatePlaylist(UserId userId, string playlistName);
        void DeletePlaylist(string playlistId);
        void AddSongsToPlaylist(string playlistId, string[] songIds);
        DeezerSong GetSong(string title, string artist);
        DeezerSong GetSong(string songId);
        IEnumerable<DeezerSong> GetSongsByPlaylistId(string playlistId);
    }
}
