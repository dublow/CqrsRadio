using System;
using System.Linq;
using CqrsRadio.Domain.Aggregates;
using CqrsRadio.Domain.Events;
using CqrsRadio.Domain.ValueTypes;
using CqrsRadio.Infrastructure.Bus;
using CqrsRadio.Infrastructure.EventStores;
using CqrsRadio.Test.Mocks;
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
            Assert.IsTrue(stream.GetEvents().Contains(new SongAdded("12345", "bestof", "titleOne", "artistOne")));
            Assert.IsTrue(stream.GetEvents().Contains(new SongAdded("12345", "bestof", "titleTwo", "artistOne")));
        }

        [Test]
        public void CreateRadioAndSearchSong()
        {
            var stream = new MemoryEventStream();
            var publisher = new EventBus(stream);

            var radioEngine = RadioEngineBuilder
                .Create()
                .SetParser("title", "artist")
                .Build();

            var radio = Radio.Create(stream, publisher, radioEngine, "djam", "http://djam.fr");
            radio.SearchSong();

            Assert.IsTrue(stream.GetEvents().Contains(new RadioCreated("djam", new Uri("http://djam.fr"))));
            Assert.IsTrue(stream.GetEvents().Contains(new RadioSongParsed("djam", "title", "artist")));
        }
    }
}
