using System.Linq;
using CqrsRadio.Domain.Entities;
using CqrsRadio.Domain.Events;
using CqrsRadio.Domain.ValueTypes;
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
            var deezerApi = DeezerApiBuilder
                .Create()
                .SetSong(new DeezerSong("1234567890", "title", "artist"))
                .Build();
            var radioSongHandler = new RadioSongHandler(radioSongRepository, deezerApi);
            var (songId, name, title, artist) = (SongId.Parse("1234567890"), "djam", "title", "artist");
            // act

            var radioSongParsed = new RadioSongParsed("djam", "title", "artist");
            radioSongHandler.Handle(radioSongParsed);
            // assert
            var (actualSongId, actualName, actualTitle, actualArtist) = mockedRadioSongRepository.RadioSongs.First();
            Assert.AreEqual(songId, actualSongId);
            Assert.AreEqual(name, actualName);
            Assert.AreEqual(title, actualTitle);
            Assert.AreEqual(artist, actualArtist);
        }

        [Test]
        public void UseRepositoryWhenRadioSongWithDeezerSongIdIsParsed()
        {
            // arrange
            var mockedRadioSongRepository = RadioSongRepositoryBuilder.Create();
            var radioSongRepository = mockedRadioSongRepository.Build();
            var deezerApi = DeezerApiBuilder
                .Create()
                .SetSong(new DeezerSong("1234567890",  "title", "artist"))
                .Build();
            var radioSongHandler = new RadioSongHandler(radioSongRepository, deezerApi);
            var (songId, name, title, artist) = (SongId.Parse("1234567890"), "djam", "title", "artist");
            // act

            var radioSongParsed = new RadioSongWithDeezerSongIdParsed("djam", "1234567890");
            radioSongHandler.Handle(radioSongParsed);
            // assert
            var (actualSongId, actualName, actualTitle, actualArtist) = mockedRadioSongRepository.RadioSongs.First();
            Assert.AreEqual(songId, actualSongId);
            Assert.AreEqual(name, actualName);
            Assert.AreEqual(title, actualTitle);
            Assert.AreEqual(artist, actualArtist);
        }


        [Test]
        public void UseRepositoryWhenRadioSongIsDuplicated()
        {
            // arrange
            var mockedRadioSongRepository = RadioSongRepositoryBuilder.Create();
            var radioSongRepository = mockedRadioSongRepository.Build();
            var deezerApi = DeezerApiBuilder
                .Create()
                .Build();
            var radioSongHandler = new RadioSongHandler(radioSongRepository, deezerApi);
            (string name, string title, string artist) = ("djam", "title", "artist");
            // act
            radioSongHandler.Handle(new RadioSongDuplicate("djam", "title", "artist"));
            // assert
            var (actualName, actualTitle, actualArtist) = mockedRadioSongRepository.RadioSongDuplicate.First();
            Assert.AreEqual(name, actualName);
            Assert.AreEqual(title, actualTitle);
            Assert.AreEqual(artist, actualArtist);
        }

        [Test]
        public void UseRepositoryWhenRadioSongIsOnError()
        {
            // arrange
            var mockedRadioSongRepository = RadioSongRepositoryBuilder.Create();
            var radioSongRepository = mockedRadioSongRepository.Build();
            var deezerApi = DeezerApiBuilder
                .Create()
                .Build();
            var radioSongHandler = new RadioSongHandler(radioSongRepository, deezerApi);
            (string name, string error) = ("djam", "error");
            // act
            radioSongHandler.Handle(new RadioSongError("djam", "error"));
            // assert
            var (actualName, actualError) = mockedRadioSongRepository.RadioSongErrors.First();
            Assert.AreEqual(name, actualName);
            Assert.AreEqual(error, actualError);
        }
    }
}
