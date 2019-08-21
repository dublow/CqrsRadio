using System.Collections.Generic;
using CqrsRadio.Common.Net;
using CqrsRadio.Domain.Repositories;
using CqrsRadio.Domain.ValueTypes;
using Newtonsoft.Json.Linq;

namespace CqrsRadio.Infrastructure.Repositories
{
    public class HttpRadioSongRepository : IRadioSongRepository
    {
        private readonly IRequest _request;

        public HttpRadioSongRepository(IRequest request)
        {
            _request = request;
        }

        public void Add(SongId songId, string radioName, string title, string artist)
        {
            _request.Post("http://127.0.0.1:1236/RadioSong/Add", "application/x-www-form-urlencoded", new Dictionary<string, object>
            {
                {"songId", songId.Value},
                {"radioName", radioName},
                {"title", title},
                {"artist", artist}
            }, s =>
            {
                var parsed = JObject.Parse(s);
                return parsed["success"].Value<bool>();
            });
        }

        public void AddToDuplicate(string radioName, string title, string artist)
        {
            _request.Post("http://127.0.0.1:1236/RadioSong/AddToDuplicate", "application/x-www-form-urlencoded", new Dictionary<string, object>
            {
                {"radioName", radioName},
                {"title", title},
                {"artist", artist}
            }, s =>
            {
                var parsed = JObject.Parse(s);
                return parsed["success"].Value<bool>();
            });
        }

        public void AddToError(string radioName, string error)
        {
            _request.Post("http://127.0.0.1:1236/RadioSong/AddToError", "application/x-www-form-urlencoded", new Dictionary<string, object>
            {
                {"radioName", radioName},
                {"error", error}
            }, s =>
            {
                var parsed = JObject.Parse(s);
                return parsed["success"].Value<bool>();
            });
        }

        public bool SongExists(SongId songId)
        {
            return _request.Get($"http://127.0.0.1:1236/RadioSong/SongExists/{songId.Value}", s =>
            {
                var parsed = JObject.Parse(s);
                return parsed["result"].Value<bool>();
            });
        }
    }
}
