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
            var mockedPlaylistRepository = PlaylistRepositoryBuilder.Create();
            var playlistRepository = mockedPlaylistRepository.Build();
            var playlistHandler = new PlaylistHandler(playlistRepository);
            (UserId userId, string name) = ("12345", "bestof");
            // act
            playlistHandler.Handle(new PlaylistAdded("12345", "bestof"));
            // assert
            var (actualUserId, actualName) = mockedPlaylistRepository.Playlists.First();
            Assert.AreEqual(userId, actualUserId);
            Assert.AreEqual(name, actualName);
        }
    }
}
