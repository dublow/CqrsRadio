﻿using System.Collections.Generic;
using CqrsRadio.Domain.Repositories;
using CqrsRadio.Domain.ValueTypes;
using Moq;

namespace CqrsRadio.Test.Mocks
{
    public class PlaylistRepositoryBuilder
    {
        private readonly Mock<IPlaylistRepository> _mock;
        public readonly List<(UserId userId, string name)> Playlists;

        public PlaylistRepositoryBuilder()
        {
            _mock = new Mock<IPlaylistRepository>();
            Playlists = new List<(UserId userId, string name)>();
        }
        public static PlaylistRepositoryBuilder Create()
        {
            return new PlaylistRepositoryBuilder();
        }

        public IPlaylistRepository Build()
        {
            _mock.Setup(x => x.Add(It.IsAny<UserId>(), It.IsAny<string>()))
                .Callback<UserId, string>((userId, name) =>
                {
                    Playlists.Add((userId, name));
                });

            _mock.Setup(x => x.Delete(It.IsAny<UserId>(), It.IsAny<string>()))
                .Callback<UserId, string>((userId, name) =>
                {
                    Playlists.Remove((userId, name));
                });
            return _mock.Object;
        }
    }
}