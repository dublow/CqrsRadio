using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CqrsRadio.Common.Net;
using CqrsRadio.Domain.Entities;
using CqrsRadio.Domain.Services;
using CqrsRadio.Domain.ValueTypes;

namespace CqrsRadio.Infrastructure.Api
{
    public class DeezerApi : IDeezerApi
    {
        private const string baseUri = "https://api.deezer.com";

        private readonly IRequest _request;

        public DeezerApi(IRequest request)
        {
            _request = request;
        }

        public void CreatePlaylist(string accessToken, UserId userId, string playlistName)
        {
            throw new NotImplementedException();
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
            var uri = $"{baseUri}/user/{userId.Value}/playlists?access_token={accessToken}";

            var response = _request.Get(uri, value =>
            {
                return value;
            });
            throw new NotImplementedException();
        }
    }
}
