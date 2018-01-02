using System.Collections.Generic;
using CqrsRadio.Common.Net;
using CqrsRadio.Domain.Entities;
using CqrsRadio.Domain.Repositories;
using CqrsRadio.Domain.ValueTypes;
using Newtonsoft.Json.Linq;

namespace CqrsRadio.Infrastructure.Repositories
{
    public class HttpSongRepository : ISongRepository
    {
        private readonly IRequest _request;

        public HttpSongRepository(IRequest request)
        {
            _request = request;
        }

        public void Add(UserId userId, PlaylistId playlistId, SongId songId, string title, string artist)
        {
            var success = _request.Post("http://127.0.0.1:1236/Song/Add", new Dictionary<string, object>
            {
                {"userId", userId.Value},
                {"playlistId", playlistId.Value},
                {"songId", songId.Value},
                {"title", title},
                {"artist", artist}
            }, s =>
            {
                var parsed = JObject.Parse(s);
                return parsed["result"].Value<bool>();
            });
        }

        public IEnumerable<Song> GetRandomSongs(int size)
        {
            return _request.Get($"http://127.0.0.1:1236/Song/GetRandomSongs/{size}", s =>
            {
                var parsed = JObject.Parse(s);
                return parsed["result"].Value<Song[]>();
            });
        }
    }
}
