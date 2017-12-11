using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

        public void DeletePlaylist(string accessToken, PlaylistId playlistId)
        {
            throw new NotImplementedException();
        }

        public void AddSongsToPlaylist(string accessToken, PlaylistId playlistId, SongId[] songIds)
        {
            throw new NotImplementedException();
        }

        public DeezerSong GetSong(string accessToken, string title, string artist)
        {
            throw new NotImplementedException();
        }

        public DeezerSong GetSong(string accessToken, SongId songId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DeezerSong> GetSongsByPlaylistId(string accessToken, PlaylistId playlistId)
        {
            var uri = String.Format(Endpoints.GetSongsByPlaylist, playlistId.Value, accessToken);

            TrackDeezer GetT(string url)
            {
                Thread.Sleep(2000);
                return _request.Get(url, JsonConvert.DeserializeObject<TrackDeezer>);
            }

            var trackItemDeezer = new List<TrackItemDeezer>();
            while (!string.IsNullOrEmpty(uri))
            {
                
                var trackDeezer = GetT(uri);
                trackItemDeezer.AddRange(trackDeezer.Tracks);
                uri = trackDeezer.Next;
            }

            return trackItemDeezer.Select(x=> new DeezerSong(SongId.Parse(x.Id), x.Title, x.Artist.Name));
        }

        public IEnumerable<PlaylistId> GetPlaylistIdsByUserId(string accessToken, UserId userId)
        {
            var uri = string.Format(Endpoints.GetPlaylists, userId.Value, accessToken);

            PlaylistDeezer GetP(string url)
            {
                return _request.Get(url, JsonConvert.DeserializeObject<PlaylistDeezer>);
            }

            var playlistIds = new List<PlaylistId>();
            while (!string.IsNullOrEmpty(uri))
            {
                var playlistDeezer = GetP(uri);
                playlistIds.AddRange(playlistDeezer.Playlists.Select(x=>PlaylistId.Parse(x.Id)));
                uri = playlistDeezer.Next;
            }

            return playlistIds;
        }
    }
}
