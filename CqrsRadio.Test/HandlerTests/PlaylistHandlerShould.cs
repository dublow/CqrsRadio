using System.Collections.Generic;
using CqrsRadio.Domain.EventHandlers;
using CqrsRadio.Domain.Events;
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
            var exected = new PlaylistEntity("12345", "bestof");
            // act
            playlistHandler.Handle(new PlaylistAdded("12345", "bestof"));
            // assert
            Assert.IsTrue(mockedPlaylistRepository.Playlists.Contains(exected));
        }
    }

    public class PlaylistEntity
    {
        public PlaylistEntity(string userId, string playlistName)
        {
            throw new System.NotImplementedException();
        }
    }

    public class PlaylistHandler : IPlaylistHandler
    {
        public PlaylistHandler(IPlaylistRepository playlistRepository)
        {
            throw new System.NotImplementedException();
        }

        public void Handle(PlaylistAdded evt)
        {
            throw new System.NotImplementedException();
        }

        public void Handle(PlaylistDeleted evt)
        {
            throw new System.NotImplementedException();
        }
    }

    public interface IPlaylistRepository    
    {
    }

    public class PlaylistRepositoryBuilder
    {
        public static PlaylistRepositoryBuilder Create()
        {
            throw new System.NotImplementedException();
        }

        public IPlaylistRepository Build()
        {
            throw new System.NotImplementedException();
        }

        public readonly List<PlaylistEntity> Playlists;
    }
}
