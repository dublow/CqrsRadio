using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace CqrsRadio.Common.Net
{
    public class RadioRequest : IRequest
    {
        public T Get<T>(string url, Func<string, T> parser)
        {
            var httpWebRequest = WebRequest.Create(url);
            using (var response = httpWebRequest.GetResponse())
            {
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    return parser(streamReader.ReadToEnd());
                }
            }
        }

        public T Post<T>(string url, Dictionary<string, object> values, Func<string, T> parser)
        {
            var httpWebRequest = WebRequest.Create(url);
            httpWebRequest.Method = "POST";

            var body = GetBody(values);
            if (body.Any())
            {
                httpWebRequest.ContentLength = body.Length;

                using (var bodyStream = httpWebRequest.GetRequestStream())
                {
                    bodyStream.Write(body, 0, body.Length);
                }
            }

            using (var response = httpWebRequest.GetResponse())
            {
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    return parser(streamReader.ReadToEnd());
                }
            }
        }

        public T Post<T>(string url, Func<string, T> parser)
        {
            var httpWebRequest = WebRequest.Create(url);
            httpWebRequest.Method = "POST";

            using (var response = httpWebRequest.GetResponse())
            {
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    return parser(streamReader.ReadToEnd());
                }
            }
        }

        private byte[] GetBody(Dictionary<string, object> values)
        {
            if (!values.Any())
                return Enumerable.Empty<byte>().ToArray();

            var body = values.Aggregate(new StringBuilder(),
                            (builder, pair) => builder.Append($"{pair.Key}={pair.Value};"),
                            builder => builder.ToString());

            return Encoding.UTF8.GetBytes(body);
        }
    }
}
