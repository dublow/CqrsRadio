using System.Linq;
using CqrsRadio.Domain.Events;
using CqrsRadio.Domain.ValueTypes;
using CqrsRadio.Handlers;
using CqrsRadio.Test.Mocks;
using NUnit.Framework;

namespace CqrsRadio.Test.HandlerTests
{
    [TestFixture]
    public class PlaylistHandlerShould
    {
        [Test]
        public void UseRepositoryWhenPlaylistIsAdded()
        {
            // arrange
            var userId = UserId.Parse("12345");
            var playlistId = PlaylistId.Parse("100");
            var playlistName = "playlistName";

            var mockedPlaylistRepository = PlaylistRepositoryBuilder.Create();
            var playlistRepository = mockedPlaylistRepository.Build();
            var playlistHandler = new PlaylistHandler(playlistRepository);
            
            // act
            playlistHandler.Handle(new PlaylistAdded(userId, playlistId, playlistName));
            // assert
            var (actualUserId, actualPlaylisId, actualPlaylistName) = mockedPlaylistRepository.Playlists.First();
            Assert.AreEqual(userId, actualUserId);
            Assert.AreEqual(playlistId, actualPlaylisId);
            Assert.AreEqual(playlistName, actualPlaylistName);
        }

        [Test]
        public void UseRepositoryWhenPlaylistIsDeleted()
        {
            // arrange
            var userId = UserId.Parse("12345");
            var playlistId = PlaylistId.Parse("100");
            var playlistName = "playlistName";

            var mockedPlaylistRepository = PlaylistRepositoryBuilder.Create();
            mockedPlaylistRepository.Playlists.Add((userId, playlistId, playlistName));
            var playlistRepository = mockedPlaylistRepository.Build();
            var playlistHandler = new PlaylistHandler(playlistRepository);
            // act
            playlistHandler.Handle(new PlaylistDeleted(userId, playlistId, playlistName));
            // assert
            Assert.AreEqual(0, mockedPlaylistRepository.Playlists.Count);
        }
    }
}
