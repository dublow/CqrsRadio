using System.Linq;
using CqrsRadio.Domain.Entities;
using CqrsRadio.Test.Mocks;
using NUnit.Framework;

namespace CqrsRadio.Test.DeezerTests
{
    [TestFixture]
    public class DeezerShould
    {
        [Test]
        public void CreatePlaylistDeezer()
        {
            var userId = "12345";
            var playlistName = "name";

            var deezerService = DeezerApiBuilder.Create();

            deezerService.Build().CreatePlaylist(userId, playlistName);

            Assert.AreEqual(1, deezerService.PlaylistAdded);
        }

        [Test]
        public void DeletePlaylistDeezer()
        {
            var playlistId = "67890";

            var deezerService = DeezerApiBuilder.Create();

            deezerService.Build().DeletePlaylist(playlistId);

            Assert.AreEqual(1, deezerService.PlaylistDeleted);
        }

        [Test]
        public void AddSongsToPlaylistDeezer()
        {
            var playlistId = "67890";
            var songIds = new[]{ "12345, 23456, 34567" };

            var deezerService = DeezerApiBuilder.Create();

            deezerService.Build().AddSongsToPlaylist(playlistId, songIds);

            Assert.AreEqual(1, deezerService.SongsAdded);
        }

        [Test]
        public void GetSongFromDeezerWhenSongExists()
        {
            // arrange
            (string title, string artist) = ("title", "artist");
            var deezerService = DeezerApiBuilder
                .Create()
                .SetSong(new DeezerSong("1234567890", "rock", title, artist))
                .Build();
            // act
            var actual = deezerService.GetSong(title, artist);
            // assert
            Assert.AreEqual("1234567890", actual.Id);
            Assert.AreEqual("rock", actual.Genre);
            Assert.AreEqual("title", actual.Title);
            Assert.AreEqual("artist", actual.Artist);
        }

        [Test]
        public void GetSongFromDeezerWhenSongNotFound()
        {
            // arrange
            (string title, string artist) = ("title", "artist");
            var deezerService = DeezerApiBuilder
                .Create()
                .SetSong(DeezerSong.Empty)
                .Build();
            // act
            var actual = deezerService.GetSong(title, artist);
            // assert
            Assert.AreEqual(DeezerSong.Empty, actual);
        }

        [Test]
        public void GetSongFromDeezerWithSongId()
        {
            // arrange
            var deezerService = DeezerApiBuilder
                .Create()
                .SetSong(new DeezerSong("1234567890", "rock", "title", "artist"))
                .Build();
            // act
            var actual = deezerService.GetSong("1234567890");
            // assert
            Assert.AreEqual("1234567890", actual.Id);
            Assert.AreEqual("rock", actual.Genre);
            Assert.AreEqual("title", actual.Title);
            Assert.AreEqual("artist", actual.Artist);
        }

        [Test]
        public void GetSongsByPlaylistIdWithPlaylistId()
        {
            // arrange
            var deezerService = DeezerApiBuilder
                .Create()
                .SetSongsByPlaylistId(new []{new DeezerSong("1234567890", "rock", "title", "artist")})
                .Build();
            // act
            var actual = deezerService.GetSongsByPlaylistId("123");
            // assert
            Assert.AreEqual(1, actual.Count());
            Assert.AreEqual("1234567890", actual.First().Id);
            Assert.AreEqual("rock", actual.First().Genre);
            Assert.AreEqual("title", actual.First().Title);
            Assert.AreEqual("artist", actual.First().Artist);
        }

        [Test]
        public void GetPlaylistsByUserIdWithUserId()
        {
            // arrange
            var deezerService = DeezerApiBuilder
                .Create()
                .SetPlaylistIdsByUserId(new[] { "123" })
                .Build();
            // act
            var actual = deezerService.GetPlaylistIdsByUserId("12345");
            // assert
            Assert.AreEqual(1, actual.Count());
            Assert.AreEqual("123", actual.First());
        }
    }
}
