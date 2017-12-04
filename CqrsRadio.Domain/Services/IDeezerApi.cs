﻿using CqrsRadio.Domain.ValueTypes;

namespace CqrsRadio.Domain.Services
{
    public interface IDeezerApi
    {
        void CreatePlaylist(UserId userId, string playlistName);
        void DeletePlaylist(string playlistId);
        void AddSongsToPlaylist(string playlistId, string[] songIds);
    }
}
