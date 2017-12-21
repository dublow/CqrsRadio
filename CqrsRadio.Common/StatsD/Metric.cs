using System;

namespace CqrsRadio.Common.StatsD
{
    public class Metric : IMetric
    {
        private readonly IStatsDRequest _request;

        public string Base => "magicplaylist";

        public Metric(IStatsDRequest request)
        {
            _request = request;
        }

        public void Count(string keyname)
        {
            _request.Send($"{Base}.{keyname.ToLower()}:1|c|@0.1");
        }

        public IDisposable Timer(string keyname)
        {
            return new Timer(elapsed =>
            {
                _request.Send($"{Base}.{keyname.ToLower()}:{(int)elapsed.TotalMilliseconds}|ms");
            });
        }

        public void Gauge(string keyname, int value)
        {
            _request.Send($"{Base}.{keyname.ToLower()}:{value}|g");
        }
    }
}
