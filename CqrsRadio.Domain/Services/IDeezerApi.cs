using System.Collections.Generic;
using CqrsRadio.Domain.Entities;
using CqrsRadio.Domain.ValueTypes;

namespace CqrsRadio.Domain.Services
{
    public interface IDeezerApi
    {
        PlaylistId CreatePlaylist(string accessToken, UserId userId, string playlistName);
        void DeletePlaylist(string accessToken, PlaylistId playlistId);
        void AddSongsToPlaylist(string accessToken, PlaylistId playlistId, SongId[] songIds);
        DeezerSong GetSong(string accessToken, string title, string artist);
        DeezerSong GetSong(string accessToken, SongId songId);
        IEnumerable<DeezerSong> GetSongsByPlaylistId(string accessToken, PlaylistId playlistId);
        IEnumerable<string> GetPlaylistIdsByUserId(string accessToken, UserId userId);
    }
}
