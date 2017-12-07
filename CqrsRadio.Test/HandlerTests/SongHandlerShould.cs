using System.Linq;
using CqrsRadio.Domain.Events;
using CqrsRadio.Domain.ValueTypes;
using CqrsRadio.Handlers;
using CqrsRadio.Test.Mocks;
using NUnit.Framework;

namespace CqrsRadio.Test.HandlerTests
{
    [TestFixture]
    public class SongHandlerShould
    {
        [Test]
        public void UseRepositoryWhenSongIsAdded()
        {
            // arrange
            var mockedSongRepository = SongRepositoryBuilder.Create();
            var songRepository = mockedSongRepository.Build();
            var songHandler = new SongHandler(songRepository);
            var (userId, playlistName, songId, title, artist) = ("12345", "bestof", SongId.Parse("123"), "title", "artist");
            // act
            songHandler.Handle(new SongAdded("12345", "bestof", "123", "title", "artist"));
            // assert
            var (actualUserId, actualPlaylistName, actualSongId, actualTitle, actualArtist) = mockedSongRepository.Songs.First();
            Assert.AreEqual(userId, actualUserId.Value);
            Assert.AreEqual(playlistName, actualPlaylistName);
            Assert.AreEqual(songId, actualSongId);
            Assert.AreEqual(title, actualTitle);
            Assert.AreEqual(artist, actualArtist);
        }
    }
}
