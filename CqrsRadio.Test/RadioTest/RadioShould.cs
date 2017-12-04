using System;
using System.Linq;
using CqrsRadio.Domain.Aggregates;
using CqrsRadio.Domain.Events;
using CqrsRadio.Infrastructure.Bus;
using CqrsRadio.Infrastructure.EventStores;
using CqrsRadio.Test.Mocks;
using NUnit.Framework;

namespace CqrsRadio.Test.RadioTest
{
    [TestFixture]
    public class RadioShould
    {
        [Test]
        public void RaiseMessageWhenCreateRadio()
        {
            var stream = new MemoryEventStream();
            var publisher = new EventBus(stream);
            var radioEngine = RadioEngineBuilder.Create().Build();
            Radio.Create(stream, publisher, radioEngine, "djam", "http://djamradio.fr");

            Assert.IsTrue(stream.GetEvents().Contains(new RadioCreated("djam", new Uri("http://djamradio.fr"))));
        }

        [Test]
        public void RaiseMessageWhenDeleteRadio()
        {
            var stream = new MemoryEventStream();
            stream.Add(new RadioCreated("djam", new Uri("http://djamradio.fr")));
            var publisher = new EventBus(stream);
            var radioEngine = RadioEngineBuilder.Create().Build();

            var radio = new Radio(stream, publisher, radioEngine);
            radio.Delete();

            Assert.IsTrue(stream.GetEvents().Contains(new RadioDeleted("djam")));
        }

        [Test]
        public void NoRaiseMessageWhenDeleteDeletedRadio()
        {
            var stream = new MemoryEventStream();
            stream.Add(new RadioCreated("djam", new Uri("http://djamradio.fr")));
            stream.Add(new RadioDeleted("djam"));
            var publisher = new EventBus(stream);
            var radioEngine = RadioEngineBuilder.Create().Build();

            var radio = new Radio(stream, publisher, radioEngine);
            radio.Delete();

            Assert.IsTrue(stream.GetEvents().Contains(new RadioDeleted("djam")));
        }

        [Test]
        public void NoRaiseMessageWhenTwiceDeleteRadio()
        {
            var stream = new MemoryEventStream();
            stream.Add(new RadioCreated("djam", new Uri("http://djamradio.fr")));
            var publisher = new EventBus(stream);
            var radioEngine = RadioEngineBuilder.Create().Build();

            var radio = new Radio(stream, publisher, radioEngine);
            radio.Delete();
            radio.Delete();

            Assert.IsTrue(stream.GetEvents().Contains(new RadioDeleted("djam")));
        }

        [Test]
        public void RaiseMessageWhenParseRadioSong()
        {
            var stream = new MemoryEventStream();
            var publisher = new EventBus(stream);
            var radioEngine = RadioEngineBuilder
                .Create()
                .SetParser("title", "artist")
                .Build();

            var radio = Radio.Create(stream, publisher, radioEngine, "djam", "http://djamradio.fr");

            radio.SearchSong();

            Assert.IsTrue(stream.GetEvents().Contains(new RadioSongParsed("djam", "title", "artist")));
        }

        [Test]
        public void NoRaiseMessageWhenParseRadioSongWithDeletedRadio()
        {
            var stream = new MemoryEventStream();
            stream.Add(new RadioCreated("djam", new Uri("http://djamradio.fr")));
            stream.Add(new RadioDeleted());
            var publisher = new EventBus(stream);
            var radioEngine = RadioEngineBuilder
                .Create()
                .SetParser("title", "artist")
                .Build();

            var radio = new Radio(stream, publisher, radioEngine);

            radio.SearchSong();

            Assert.AreEqual(0, stream.GetEvents().OfType<RadioSongParsed>().Count());
        }

        [Test]
        public void RaiseMessageWhenParseRadioSongWithAlreadyFound()
        {
            var stream = new MemoryEventStream();
            stream.Add(new RadioCreated("djam", new Uri("http://djamradio.fr")));
            stream.Add(new RadioSongParsed("djam", "title", "artist"));
            var publisher = new EventBus(stream);
            var radioEngine = RadioEngineBuilder
                .Create()
                .SetParser("title", "artist")
                .Build();

            var radio = new Radio(stream, publisher, radioEngine);

            radio.SearchSong();

            Assert.IsTrue(stream.GetEvents().Contains(new RadioSongDuplicate("djam", "title", "artist")));
        }

        [Test]
        public void RaiseMessageWhenParseRadioErrorOccured()
        {
            var stream = new MemoryEventStream();
            stream.Add(new RadioCreated("djam", new Uri("http://djamradio.fr")));
            var publisher = new EventBus(stream);
            var radioEngine = RadioEngineBuilder
                .Create()
                .SetException(new Exception("error"))
                .Build();

            var radio = new Radio(stream, publisher, radioEngine);

            radio.SearchSong();

            Assert.IsTrue(stream.GetEvents().Contains(new RadioSongError("djam", "error")));
        }
    }
}
