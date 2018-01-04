using System;
using System.Configuration;
using System.Linq;
using System.Threading;
using CqrsRadio.Common.StatsD;

namespace CqrsRadio.Process
{
    class Program
    {
        static void Main(string[] args)
        {
            var metric = new Metric(new StatsDRequest("127.0.0.1", 8125));

            while (true)
            {
                MemProcess(metric);
                Thread.Sleep(TimeSpan.FromSeconds(10));
            }
        }

        static void MemProcess(IMetric metric)
        {
            var web = int.Parse(ConfigurationManager.AppSettings["web"]);
            var api = int.Parse(ConfigurationManager.AppSettings["api"]);
            var rdo = int.Parse(ConfigurationManager.AppSettings["rdo"]);

            System.Diagnostics.Process.GetProcesses().Select(x =>
            {
                var key = x.Id == web ? "web" :
                    x.Id == api ? "api" :
                    x.Id == rdo ? "rdo" : "none";

                return new {name = key, memory = x.WorkingSet64};
            }).Where(x=>x.name != "none").ToList().ForEach(x => metric.Gauge(x.name, x.memory));
            
        }
    }
}
