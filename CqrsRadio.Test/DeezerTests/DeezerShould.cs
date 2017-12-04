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

            var deezerService = DeezerApiBuilder.Create().SetCreatePlaylist("name");

            deezerService.Build().CreatePlaylist(userId, playlistName);

            Assert.AreEqual("name-added", deezerService.PlaylistAdded);
        }
    }

    

    
}
