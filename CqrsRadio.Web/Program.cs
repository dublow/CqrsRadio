using System;
using System.Configuration;
using Nancy;
using Nancy.Hosting.Self;

namespace CqrsRadio.Web
{
    class Program
    {
        static void Main(string[] args)
        {
            var local = Type.GetType("Mono.Runtime") != null ? "" : "local";
            var url = ConfigurationManager.AppSettings[$"url{local}"];
            using (var host = new NancyHost(new Uri(url)))
            {
                host.Start();
                Console.WriteLine($"Running on {url}");
                Console.ReadLine();
            }
        }
    }
}
