using System;

namespace CqrsRadio.Common.StatsD
{
    public interface IMetric
    {
        string Base { get; }
        
        void Count(string keyname);
        IDisposable Timer(string keyname);
        void Gauge(string keyname, long value);
    }
}
