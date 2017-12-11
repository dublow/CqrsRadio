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
            var userId = UserId.Parse("12345");
            var playlistId = PlaylistId.Parse("100");
            var songId = SongId.Parse("001");
            var title = "title";
            var artist = "artist";

            var mockedSongRepository = SongRepositoryBuilder.Create();
            var songRepository = mockedSongRepository.Build();
            var songHandler = new SongHandler(songRepository);
            // act
            songHandler.Handle(new SongAdded(userId, playlistId, songId, title, artist));
            // assert
            var (actualUserId, actualPlaylistId, actualSongId, actualTitle, actualArtist) = mockedSongRepository.Songs.First();
            Assert.AreEqual(userId, actualUserId);
            Assert.AreEqual(playlistId, actualPlaylistId);
            Assert.AreEqual(songId, actualSongId);
            Assert.AreEqual(title, actualTitle);
            Assert.AreEqual(artist, actualArtist);
        }
    }
}
