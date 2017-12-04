using System.Linq;
using CqrsRadio.Domain.Events;
using CqrsRadio.Infrastructure.Bus;
using CqrsRadio.Infrastructure.EventStores;
using CqrsRadio.Main.Handlers;
using NUnit.Framework;

namespace CqrsRadio.Test.EventBusTests
{
    [TestFixture]
    public class EventBusShould
    {
        [Test]
        public void StoreEventWhenPublishEvents()
        {
            var stream = new MemoryEventStream();

            var eventBus = new EventBus(stream);

            eventBus.Publish(new PlaylistCreated("12345", "name"));

            Assert.AreEqual(1, stream.GetEvents().OfType<PlaylistCreated>().Count());
        }

        [Test]
        public void HandleEventWhenSubscribeToEvent()
        {
            var stream = new MemoryEventStream();

            var eventBus = new EventBus(stream);

            var playlistHandler1 = new PlaylistHandler<PlaylistCreated>();
            var playlistHandler2 = new PlaylistHandler<PlaylistCreated>();

            eventBus.Subscribe(playlistHandler1);
            eventBus.Subscribe(playlistHandler2);

            eventBus.Publish(new PlaylistCreated("12345", "name"));

            Assert.AreEqual("name", playlistHandler1.Name);
            Assert.AreEqual("name", playlistHandler2.Name);
        }
    }
}
