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
            var userId = UserId.Parse("12345");
            var playlistId = PlaylistId.Parse("100");
            var playlistName = "playlistName";
            var accessToken = "accessToken";

            var deezerService = DeezerApiBuilder.Create();

            var actualPlaylistId = deezerService
                .SetCreatePlaylist(playlistId)
                .Build()
                .CreatePlaylist(accessToken, userId, playlistName);

            Assert.AreEqual(actualPlaylistId, actualPlaylistId);
        }

        [Test]
        public void DeletePlaylistDeezer()
        {
            var playlistId = PlaylistId.Parse("100");
            var accessToken = "accessToken";

            var deezerService = DeezerApiBuilder.Create();

            deezerService.Build().DeletePlaylist(accessToken, playlistId);

            Assert.AreEqual(1, deezerService.PlaylistDeleted);
        }

        [Test]
        public void AddSongsToPlaylistDeezer()
        {
            var playlistId = PlaylistId.Parse("100");
            var accessToken = "accessToken";
            var songIds = new []
            {
                SongId.Parse("001"),
                SongId.Parse("002"),
                SongId.Parse("003")
            };

            var deezerService = DeezerApiBuilder.Create();

            deezerService.Build().AddSongsToPlaylist(accessToken, playlistId, songIds);

            Assert.AreEqual(3, deezerService.SongsAdded);
        }

        [Test]
        public void GetSongFromDeezerWhenSongExists()
        {
            // arrange
            var songId = SongId.Parse("001");
            string title = "title";
            string artist = "artist";
            var accessToken = "accessToken";

            var deezerService = DeezerApiBuilder
                .Create()
                .SetSong(new DeezerSong(songId, title, artist))
                .Build();
            // act
            var actual = deezerService.GetSong(accessToken, title, artist);
            // assert
            Assert.AreEqual(songId, actual.Id);
            Assert.AreEqual(title, actual.Title);
            Assert.AreEqual(artist, actual.Artist);
        }

        [Test]
        public void GetSongFromDeezerWhenSongNotFound()
        {
            // arrange
            string title = "title";
            string artist = "artist";
            var accessToken = "accessToken";

            var deezerService = DeezerApiBuilder
                .Create()
                .SetSong(DeezerSong.Empty)
                .Build();
            // act
            var actual = deezerService.GetSong(accessToken, title, artist);
            // assert
            Assert.AreEqual(DeezerSong.Empty, actual);
        }

        [Test]
        public void GetSongFromDeezerWithSongId()
        {
            // arrange
            var songId = SongId.Parse("001");
            string title = "title";
            string artist = "artist";
            var accessToken = "accessToken";

            var deezerService = DeezerApiBuilder
                .Create()
                .SetSong(new DeezerSong(songId, title, artist))
                .Build();
            // act
            var actual = deezerService.GetSong(accessToken, songId);
            // assert
            Assert.AreEqual(songId, actual.Id);
            Assert.AreEqual(title, actual.Title);
            Assert.AreEqual(artist, actual.Artist);
        }

        [Test]
        public void GetSongsByPlaylistIdWithPlaylistId()
        {
            // arrange
            var playlistId = PlaylistId.Parse("100");
            var songId = SongId.Parse("001");
            var title = "title";
            var artist = "artist";
            var accessToken = "accessToken";

            var deezerService = DeezerApiBuilder
                .Create()
                .SetSongsByPlaylistId(new []{new DeezerSong(songId, title, artist) })
                .Build();
            // act
            var actual = deezerService.GetSongsByPlaylistId(accessToken, playlistId);
            // assert
            Assert.AreEqual(1, actual.Count());
            Assert.AreEqual(songId, actual.First().Id);
            Assert.AreEqual(title, actual.First().Title);
            Assert.AreEqual(artist, actual.First().Artist);
        }

        [Test]
        public void GetPlaylistsByUserIdWithUserId()
        {
            // arrange
            var playlistId = PlaylistId.Parse("100");
            var userId = UserId.Parse("12345");
            var accessToken = "accessToken";

            var deezerService = DeezerApiBuilder
                .Create()
                .SetPlaylistIdsByUserId(new[] { playlistId })
                .Build();
            // act
            var actual = deezerService.GetPlaylistIdsByUserId(accessToken, userId, s => true);
            // assert
            Assert.AreEqual(1, actual.Count());
            Assert.AreEqual(playlistId, actual.First());
        }
    }
}
