using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CqrsRadio.Domain.EventHandlers;
using CqrsRadio.Domain.Events;
using CqrsRadio.Domain.ValueTypes;
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
            (string title, string artist) = ("title", "artist");
            // act
            radioSongHandler.Handle(new RadioSongParsed("title", "artist"));
            // assert
            var (actualTitle, actualArtist) = mockedRadioSongRepository.RadioSongs.First();
            Assert.AreEqual(title, actualTitle);
            Assert.AreEqual(artist, actualArtist);
        }
    }

    public class RadioSongHandler : IRadioSongHandler
    {
        public RadioSongHandler(IRadioSongRepository radioSongRepository)
        {
            throw new NotImplementedException();
        }

        public void Handle(RadioSongParsed evt)
        {
            throw new NotImplementedException();
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
    }

    public class RadioSongRepositoryBuilder     
    {
        public static RadioSongRepositoryBuilder Create()
        {
            throw new NotImplementedException();
        }

        public IRadioSongRepository Build()
        {
            throw new NotImplementedException();
        }

        public List<(string title, string artist)> RadioSongs { get; set; }
    }
}
