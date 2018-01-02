using System.Collections.Generic;
using CqrsRadio.Common.Net;
using CqrsRadio.Domain.Repositories;
using Newtonsoft.Json.Linq;

namespace CqrsRadio.Infrastructure.Repositories
{
    public class HttpAdminRepository : IAdminRepository
    {
        private readonly IRequest _request;

        public HttpAdminRepository(IRequest request)
        {
            _request = request;
        }

        public void Add(string login, byte[] hash)
        {
            var success = _request.Post("http://127.0.0.1:1236/Admin/Add", new Dictionary<string, object>
            {
                {"login", login},
                {"password", hash}
            }, s =>
            {
                var r = JObject.Parse(s);
                return r["success"].Value<bool>();
            });
        }

        public bool Exists(string login)
        {
            return _request.Get($"http://127.0.0.1:1236/Admin/Exists/{login}", s =>
            {
                var r = JObject.Parse(s);
                return r["result"].Value<bool>();
            });
        }

        public byte[] GetPassword(string login)
        {
            return _request.Get($"http://127.0.0.1:1236/Admin/GetPassword/{login}", s =>
            {
                var r = JObject.Parse(s);
                return r["result"].Value<byte[]>();
            });
        }
    }
}
