using System.Collections.Generic;
using CqrsRadio.Domain.Entities;
using CqrsRadio.Domain.ValueTypes;

namespace CqrsRadio.Domain.Services
{
    public interface IDeezerApi
    {
        PlaylistId CreatePlaylist(string accessToken, UserId userId, string playlistName);
        void DeletePlaylist(string accessToken, string playlistId);
        void AddSongsToPlaylist(string accessToken, string playlistId, string[] songIds);
        DeezerSong GetSong(string accessToken, string title, string artist);
        DeezerSong GetSong(string accessToken, string songId);
        IEnumerable<DeezerSong> GetSongsByPlaylistId(string accessToken, string playlistId);
        IEnumerable<string> GetPlaylistIdsByUserId(string accessToken, UserId userId);
    }
}
