﻿using System;
using System.Collections.Generic;
using System.Linq;
using CqrsRadio.Common.Net;
using CqrsRadio.Deezer.Models;
using CqrsRadio.Domain.Entities;
using CqrsRadio.Domain.Services;
using CqrsRadio.Domain.ValueTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CqrsRadio.Deezer
{
    public class DeezerApi : IDeezerApi
    {
        private readonly IRequest _request;

        public DeezerApi(IRequest request)
        {
            _request = request;
        }

        public PlaylistId CreatePlaylist(string accessToken, UserId userId, string playlistName)
        {
            return _request.Post(string.Format(Endpoints.CreatePlaylist, userId.Value, playlistName, accessToken), response =>
            {
                if (!JObject
                    .Parse(response)
                    .TryGetValue("id", StringComparison.InvariantCultureIgnoreCase, out var playlistId))
                {
                    return PlaylistId.Empty;
                }

                return PlaylistId.Parse(playlistId.ToString());
            });
        }

        public void DeletePlaylist(string accessToken, string playlistId)
        {
            throw new NotImplementedException();
        }

        public void AddSongsToPlaylist(string accessToken, string playlistId, string[] songIds)
        {
            throw new NotImplementedException();
        }

        public DeezerSong GetSong(string accessToken, string title, string artist)
        {
            throw new NotImplementedException();
        }

        public DeezerSong GetSong(string accessToken, string songId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DeezerSong> GetSongsByPlaylistId(string accessToken, string playlistId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetPlaylistIdsByUserId(string accessToken, UserId userId)
        {
            var uri = string.Format(Endpoints.GetPlaylists, userId.Value, accessToken);

            PlaylistDeezer GetP(string url)
            {
                return _request.Get(url, JsonConvert.DeserializeObject<PlaylistDeezer>);
            }

            var playlistIds = new List<string>();
            while (!string.IsNullOrEmpty(uri))
            {
                var playlistDeezer = GetP(uri);
                playlistIds.AddRange(playlistDeezer.Playlists.Select(x=>x.Id));
                uri = playlistDeezer.Next;
            }

            return playlistIds;
        }
    }
}
