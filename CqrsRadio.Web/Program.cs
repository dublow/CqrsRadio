using System;
using System.Diagnostics;
using System.Linq;
using CqrsRadio.Domain.Configuration;
using Nancy.Hosting.Self;

namespace CqrsRadio.Web
{
    class Program
    {
        static void Main(string[] args)
        {
            var currentEnvironment = GetCurrentEnvironment();
            using (var host = new NancyHost(new Uri(currentEnvironment.Url), new NancyBootstrapper(currentEnvironment)))
            {
                host.Start();
                Console.WriteLine($"Running on {currentEnvironment.Url}");
                Console.ReadLine();
            }
        }

        static Domain.Configuration.Environment GetCurrentEnvironment()
        {
            var environmentType = Type.GetType("Mono.Runtime") != null
                ? EnvironmentType.Production
                : EnvironmentType.Local;

            return MagicPlaylistConfiguration
                .Current
                .Environments
                .Single(x => x.Name == environmentType);
        }
    }
}
