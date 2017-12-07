using System.CodeDom;
using System.Linq;
using CqrsRadio.Domain.Entities;
using CqrsRadio.Domain.ValueTypes;
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
            var playlistId = "123";
            var playlistName = "name";

            var deezerService = DeezerApiBuilder.Create();

            var actualPlaylistId = deezerService
                .SetCreatePlaylist(playlistId)
                .Build()
                .CreatePlaylist("accesstoken", userId, playlistName);

            Assert.AreEqual(actualPlaylistId, actualPlaylistId);
        }

        [Test]
        public void DeletePlaylistDeezer()
        {
            var playlistId = "67890";

            var deezerService = DeezerApiBuilder.Create();

            deezerService.Build().DeletePlaylist("accesstoken", playlistId);

            Assert.AreEqual(1, deezerService.PlaylistDeleted);
        }

        [Test]
        public void AddSongsToPlaylistDeezer()
        {
            var playlistId = "67890";
            var songIds = new []{ SongId.Parse("12345"), SongId.Parse("23456"), SongId.Parse("34567") };

            var deezerService = DeezerApiBuilder.Create();

            deezerService.Build().AddSongsToPlaylist("accesstoken", playlistId, songIds);

            Assert.AreEqual(3, deezerService.SongsAdded);
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
            var actual = deezerService.GetSong("accessToken", title, artist);
            // assert
            Assert.AreEqual(SongId.Parse("1234567890"), actual.Id);
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
            var actual = deezerService.GetSong("accessToken", title, artist);
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
            var actual = deezerService.GetSong("accesstoken", "1234567890");
            // assert
            Assert.AreEqual(SongId.Parse("1234567890"), actual.Id);
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
            var actual = deezerService.GetSongsByPlaylistId("accesstoken", "123");
            // assert
            Assert.AreEqual(1, actual.Count());
            Assert.AreEqual(SongId.Parse("1234567890"), actual.First().Id);
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
            var actual = deezerService.GetPlaylistIdsByUserId("accesstoken", "12345");
            // assert
            Assert.AreEqual(1, actual.Count());
            Assert.AreEqual("123", actual.First());
        }
    }
}
