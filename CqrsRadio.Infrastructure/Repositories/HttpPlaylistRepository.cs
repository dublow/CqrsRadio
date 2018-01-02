using System;
using System.Collections.Generic;
using CqrsRadio.Common.Net;
using CqrsRadio.Domain.Repositories;
using CqrsRadio.Domain.ValueTypes;
using Newtonsoft.Json.Linq;

namespace CqrsRadio.Infrastructure.Repositories
{
    public class HttpPlaylistRepository : IPlaylistRepository
    {
        private readonly IRequest _request;

        public HttpPlaylistRepository(IRequest request)
        {
            _request = request;
        }

        public void Add(UserId userId, PlaylistId playlistId, string name)
        {
            var success = _request.Post("http://127.0.0.1:1236/Playlist/Add", new Dictionary<string, object>
            {
                {"userId", userId.Value},
                {"playlistId", playlistId.Value},
                {"name", name}
            }, s =>
            {
                var parsed = JObject.Parse(s);
                return parsed["result"].Value<bool>();
            });
        }

        public void Update(UserId userId)
        {
            var success = _request.Post("http://127.0.0.1:1236/Playlist/Update", new Dictionary<string, object>
            {
                {"userId", userId.Value}
            }, s =>
            {
                var parsed = JObject.Parse(s);
                return parsed["result"].Value<bool>();
            });
        }

        public void Delete(UserId userId, PlaylistId playlistId, string name)
        {
            var success = _request.Post("http://127.0.0.1:1236/Playlist/Delete", new Dictionary<string, object>
            {
                {"userId", userId.Value},
                {"playlistId", playlistId.Value},
                {"name", name}
            }, s =>
            {
                var parsed = JObject.Parse(s);
                return parsed["result"].Value<bool>();
            });
        }

        public bool CanCreatePlaylist(UserId userId, DateTime interval)
        {
            return _request.Get($"http://127.0.0.1:1236/Playlist/CanCreatePlaylist/{userId.Value}/{interval:O}",  s =>
            {
                var parsed = JObject.Parse(s);
                return parsed["result"].Value<bool>();
            });
        }

        public bool Exists(UserId userId)
        {
            return _request.Get($"http://127.0.0.1:1236/Playlist/Exists/{userId.Value}", s =>
            {
                var parsed = JObject.Parse(s);
                return parsed["result"].Value<bool>();
            });
        }
    }
}
