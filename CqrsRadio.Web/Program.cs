﻿using System;
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
            var currentEnvironment = GetHardCodedEnvironment();
            using (var host = new NancyHost(new Uri(currentEnvironment.Url), new NancyBootstrapper(currentEnvironment)))
            {
                host.Start();
                Console.WriteLine($"web server running on {currentEnvironment.Url}");
                Console.ReadLine();
            }
        }

        static RadioEnvironment GetCurrentEnvironment()
        {
            var environmentType = Type.GetType("Mono.Runtime") != null
                ? EnvironmentType.Production
                : EnvironmentType.Local;

            return MagicPlaylistConfiguration
                .Current
                .Environments
                .Single(x => x.Name == environmentType);
        }

        static RadioEnvironment GetHardCodedEnvironment()
        {
            var environmentType = Type.GetType("Mono.Runtime") != null
                ? EnvironmentType.Production
                : EnvironmentType.Local;

            return new HardCodedMagicPlaylistConfiguration()
                .Environments
                .Single(x => x.Name == environmentType);
        }
    }
}
