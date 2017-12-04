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
            (UserId userId, string playlistName, string title, string artist) = ("12345", "bestof", "title", "artist");
            // act
            songHandler.Handle(new SongAdded("12345", "bestof", "title", "artist"));
            // assert
            var (actualUserId, actualPlaylistName, actualTitle, actualArtist) = mockedSongRepository.Songs.First();
            Assert.AreEqual(userId, actualUserId);
            Assert.AreEqual(playlistName, actualPlaylistName);
            Assert.AreEqual(title, actualTitle);
            Assert.AreEqual(artist, actualArtist);
        }
    }
}
