using System;
using System.Collections.Generic;
using CqrsRadio.Domain.EventHandlers;
using CqrsRadio.Domain.Events;
using CqrsRadio.Domain.ValueTypes;
using Moq;
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

    public class PlaylistEntity : IEquatable<PlaylistEntity>
    {
        public UserId UserId;
        public string Name;

        public PlaylistEntity(UserId userId, string name)
        {
            UserId = userId;
            Name = name;
        }

        public bool Equals(PlaylistEntity other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return UserId.Equals(other.UserId) && string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PlaylistEntity) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (UserId.GetHashCode() * 397) ^ (Name != null ? Name.GetHashCode() : 0);
            }
        }

        public static bool operator ==(PlaylistEntity left, PlaylistEntity right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PlaylistEntity left, PlaylistEntity right)
        {
            return !Equals(left, right);
        }
    }

    public class PlaylistHandler : IPlaylistHandler
    {
        private readonly IPlaylistRepository _playlistRepository;

        public PlaylistHandler(IPlaylistRepository playlistRepository)
        {
            _playlistRepository = playlistRepository;
        }

        public void Handle(PlaylistAdded evt)
        {
            _playlistRepository.AddPlaylist(evt.UserId, evt.Name);
        }

        public void Handle(PlaylistDeleted evt)
        {
            throw new System.NotImplementedException();
        }
    }

    public interface IPlaylistRepository    
    {
        void AddPlaylist(UserId userId, string name);
    }

    public class PlaylistRepositoryBuilder
    {
        public PlaylistRepositoryBuilder()
        {
            _mock = new Mock<IPlaylistRepository>();
            Playlists = new List<PlaylistEntity>();
        }
        public static PlaylistRepositoryBuilder Create()
        {
            return new PlaylistRepositoryBuilder();
        }

        public IPlaylistRepository Build()
        {
            _mock.Setup(x => x.AddPlaylist(It.IsAny<UserId>(), It.IsAny<string>()))
                .Callback<UserId, string>((userId, name) =>
                {
                    Playlists.Add(new PlaylistEntity(userId, name));
                });
                   

            return _mock.Object;
        }

        public readonly List<PlaylistEntity> Playlists;
        private Mock<IPlaylistRepository> _mock;
    }
}
