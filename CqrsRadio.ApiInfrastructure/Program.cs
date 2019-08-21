using System;
using Nancy.Hosting.Self;

namespace CqrsRadio.ApiInfrastructure
{
    class Program
    {
        static void Main(string[] args)
        {
            var url = "http://127.0.0.1:1236";
            using (var host = new NancyHost(new Uri(url)))
            {
                host.Start();
                Console.WriteLine($"web server running on {url}");
                Console.ReadLine();
            }
        }
    }
}
