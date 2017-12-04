using System.Linq;
using CqrsRadio.Domain.Aggregates;
using CqrsRadio.Domain.Events;
using CqrsRadio.Domain.ValueTypes;
using CqrsRadio.Infrastructure.Bus;
using CqrsRadio.Infrastructure.EventStores;
using NUnit.Framework;

namespace CqrsRadio.Test.ScenarioTests
{
    [TestFixture]
    public class ScenarioShould
    {
        [Test]
        public void CreateUserWithOnePlaylistAndTwoSong()
        {
            var stream = new MemoryEventStream();
            var publisher = new EventBus(stream);

            var user = User.Create(stream, publisher, "nicolas.dfr@gmail.com", "nicolas", "12345");
            user.AddPlaylist("bestof");
            user.AddSongToPlaylist("bestof", "titleOne", "artistOne");
            user.AddSongToPlaylist("bestof", "titleTwo", "artistOne");

            Assert.IsTrue(stream.GetEvents().Contains(new UserCreated(Identity.Create("nicolas.dfr@gmail.com", "nicolas", "12345"))));
            Assert.IsTrue(stream.GetEvents().Contains(new PlaylistAdded("12345", "bestof")));
            Assert.IsTrue(stream.GetEvents().Contains(new SongAdded("bestof", "titleOne", "artistOne")));
            Assert.IsTrue(stream.GetEvents().Contains(new SongAdded("bestof", "titleTwo", "artistOne")));
        }
    }
}
