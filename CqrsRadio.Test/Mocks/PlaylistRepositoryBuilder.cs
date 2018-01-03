using System;
using System.Collections.Generic;
using CqrsRadio.Domain.Repositories;
using CqrsRadio.Domain.ValueTypes;
using Moq;

namespace CqrsRadio.Test.Mocks
{
    public class PlaylistRepositoryBuilder
    {
        private readonly Mock<IPlaylistRepository> _mock;
        public readonly List<(UserId userId, PlaylistId playlistId, string name)> Playlists;

        public PlaylistRepositoryBuilder()
        {
            _mock = new Mock<IPlaylistRepository>();
            Playlists = new List<(UserId userId, PlaylistId playlistId, string name)>();
        }
        public static PlaylistRepositoryBuilder Create()
        {
            return new PlaylistRepositoryBuilder();
        }

        public PlaylistRepositoryBuilder SetCanCreatePlaylist(bool value)
        {
            _mock.Setup(x => x.CanCreatePlaylist(It.IsAny<UserId>(), It.IsAny<DateTime>())).Returns(value);
            return this;
        }

        public IPlaylistRepository Build()
        {
            _mock.Setup(x => x.Add(It.IsAny<UserId>(), It.IsAny<PlaylistId>(), It.IsAny<string>()))
                .Callback<UserId, PlaylistId, string>((userId, playlistId, name) =>
                {
                    Playlists.Add((userId, playlistId, name));
                });

            _mock.Setup(x => x.Delete(It.IsAny<UserId>(), It.IsAny<PlaylistId>(), It.IsAny<string>()))
                .Callback<UserId, PlaylistId, string>((userId, playlistId, name) =>
                {
                    Playlists.Remove((userId, playlistId, name));
                });
            return _mock.Object;
        }
    }
}