using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CqrsRadio.Common.Net
{
    public interface IRequest
    {
        T Get<T>(string url, Func<string, T> parser);
        T Post<T>(string url, string contentType, Dictionary<string, object> values, Func<string, T> parser);
        T Post<T>(string url, string contentType, Func<string, T> parser);
    }
}
