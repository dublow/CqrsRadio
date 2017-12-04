using System;
using CqrsRadio.Di;
using CqrsRadio.Domain.Events;
using CqrsRadio.Domain.EventStores;
using CqrsRadio.Infrastructure.Bus;
using CqrsRadio.Main.Handlers;

namespace CqrsRadio.Main
{
    class Program
    {
        static void Main(string[] args)
        {
            var bootstrapper = Bootstrapper.Initialize();
            var eventBus = bootstrapper.Get<EventBus>();
            var stream = bootstrapper.Get<IEventStream>();

            eventBus.Subscribe(new SongCounterHandler<SongAdded>());
            eventBus.Subscribe(new PlaylistHandler<PlaylistCreated>());


            Console.ReadLine();
        }
    }
}
