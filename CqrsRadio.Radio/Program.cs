using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CqrsRadio.Common.Net;
using CqrsRadio.Common.StatsD;
using CqrsRadio.Deezer;
using CqrsRadio.Domain.Events;
using CqrsRadio.Domain.Handlers;
using CqrsRadio.Domain.Repositories;
using CqrsRadio.Domain.Services;
using CqrsRadio.Handlers;
using CqrsRadio.Infrastructure.Bus;
using CqrsRadio.Infrastructure.EventStores;
using CqrsRadio.Infrastructure.Repositories;

namespace CqrsRadio.Radio
{
    public class Program
    {
        static void Main(string[] args)
        {
            var request = new RadioRequest();
            var radioEngine = new DjamRadioEngine(request);
            var radioSongRepository = new HttpRadioSongRepository(request);
            var deezerApi = new DeezerApi(request);
            var metric = new Metric(new StatsDRequest("127.0.0.1", 8125));

            while (true)
            {
                Console.WriteLine("Running radio");
                metric.Count("getsong");
                GetSong(radioEngine, radioSongRepository, deezerApi, metric);
                Thread.Sleep(TimeSpan.FromMinutes(3));
            }
        }

        static void GetSong(IRadioEngine radioEngine, IRadioSongRepository radioSongRepository, IDeezerApi deezerApi, IMetric metric)
        {
            var memStream = new MemoryEventStream();
            var bus = new EventBus(memStream);

            bus.Subscribe(new RadioSongHandler(radioSongRepository, deezerApi, metric));

            Domain.Aggregates.Radio
                .Create(memStream, bus, radioEngine, "djam", "http://www.djamradio.com/actions/infos.php")
                .SearchSong();
        }
    }
}
