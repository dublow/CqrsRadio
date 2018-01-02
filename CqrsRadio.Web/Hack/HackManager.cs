using System.Collections.Generic;
using System.Linq;
using CqrsRadio.Common.Net;
using Newtonsoft.Json.Linq;

namespace CqrsRadio.Web.Hack
{
    public class HackManager
    {
        private readonly IRequest _request;

        public HackManager(IRequest request)
        {
            _request = request;
        }

        public JArray GetLocalization(List<DateIp> dateIps)
        {
            var jArray = new JArray();
            dateIps
                .ForEach(x =>
                {
                    var jObject = _request.Get($"http://ip-api.com/json/{x.Ip}?fields=lat,lon", JObject.Parse);

                    if (jObject.HasValues)
                        jArray.Add(jObject);
                });

            return jArray;
        }
    }
}
