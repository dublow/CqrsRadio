using System.Linq;
using CqrsRadio.Domain.Events;
using CqrsRadio.Handlers;
using CqrsRadio.Test.Mocks;
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
}
