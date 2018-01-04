using System;
using System.Linq;
using CqrsRadio.Domain.Configuration;
using CqrsRadio.Web.Configuration;
using Nancy.Hosting.Self;

namespace CqrsRadio.Web
{
    class Program
    {
        static void Main(string[] args)
        {
            var environmentType = Type.GetType("Mono.Runtime") != null
                ? EnvironmentType.Production
                : EnvironmentType.Local;

            var currentEnvironment = GetHardCodedEnvironment(environmentType);
            var nancyBootstrapper = new NancyBootstrapper(currentEnvironment);
            using (var host = new NancyHost(new Uri(currentEnvironment.Url), nancyBootstrapper))
            {
                host.Start();
                Console.WriteLine($"web server running on {currentEnvironment.Url}");
                Console.ReadLine();
            }
        }

        static RadioEnvironment GetCurrentEnvironment(EnvironmentType environmentType)
        {
            return MagicPlaylistConfiguration
                .Current
                .Environments
                .Single(x => x.Name == environmentType);
        }

        static RadioEnvironment GetHardCodedEnvironment(EnvironmentType environmentType)
        {
            return new HardCodedMagicPlaylistConfiguration()
                .Environments
                .Single(x => x.Name == environmentType);
        }
    }
}
