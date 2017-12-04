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

            Assert.IsTrue(deezerService.PlaylistAdded);
        }

        [Test]
        public void DeletePlaylistDeezer()
        {
            var playlistId = "67890";

            var deezerService = DeezerApiBuilder.Create();

            deezerService.Build().DeletePlaylist(playlistId);

            Assert.IsTrue(deezerService.PlaylistDeleted);
        }

        [Test]
        public void AddSongsToPlaylistDeezer()
        {
            var playlistId = "67890";
            var songIds = new[]{ "12345, 23456, 34567" };

            var deezerService = DeezerApiBuilder.Create();

            deezerService.Build().AddSongsToPlaylist(playlistId, songIds);

            Assert.IsTrue(deezerService.SongsAdded);
        }
    }
}
