using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CqrsRadio.Domain.EventHandlers;
using CqrsRadio.Domain.Events;
using CqrsRadio.Domain.ValueTypes;
using Moq;
using NUnit.Framework;

namespace CqrsRadio.Test.HandlerTests
{
    [TestFixture]
    public class RadioSongHandlerShould
    {
        [Test]
        public void UseRepositoryWhenRadioSongIsParsed()
        {
            // arrange
            var mockedRadioSongRepository = RadioSongRepositoryBuilder.Create();
            var radioSongRepository = mockedRadioSongRepository.Build();
            var radioSongHandler = new RadioSongHandler(radioSongRepository);
            (string name, string title, string artist) = ("djam", "title", "artist");
            // act
            radioSongHandler.Handle(new RadioSongParsed("djam", "title", "artist"));
            // assert
            var (actualName, actualTitle, actualArtist) = mockedRadioSongRepository.RadioSongs.First();
            Assert.AreEqual(name, actualName);
            Assert.AreEqual(title, actualTitle);
            Assert.AreEqual(artist, actualArtist);
        }
    }

    public class RadioSongHandler : IRadioSongHandler
    {
        private readonly IRadioSongRepository _radioSongRepository;

        public RadioSongHandler(IRadioSongRepository radioSongRepository)
        {
            _radioSongRepository = radioSongRepository;
        }

        public void Handle(RadioSongParsed evt)
        {
            _radioSongRepository.Add(evt.RadioName, evt.Title, evt.Artist);
        }

        public void Handle(RadioSongDuplicate evt)
        {
            throw new NotImplementedException();
        }

        public void Handle(RadioSongError evt)
        {
            throw new NotImplementedException();
        }
    }

    public interface IRadioSongRepository
    {
        void Add(string radioName, string title, string artist);
    }

    public class RadioSongRepositoryBuilder     
    {
        private Mock<IRadioSongRepository> _mock;

        public RadioSongRepositoryBuilder()
        {
            _mock = new Mock<IRadioSongRepository>();
            RadioSongs = new List<(string name, string title, string artist)>();
        }

        public static RadioSongRepositoryBuilder Create()
        {
            return new RadioSongRepositoryBuilder();
        }

        public IRadioSongRepository Build()
        {
            _mock.Setup(x => x.Add(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string, string>((name, title, artist) => RadioSongs.Add((name, title, artist)));

            return _mock.Object;
        }

        public List<(string name, string title, string artist)> RadioSongs { get; set; }
    }
}
