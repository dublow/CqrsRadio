using System.Collections.Generic;
using CqrsRadio.Domain.Repositories;
using CqrsRadio.Domain.ValueTypes;
using Moq;

namespace CqrsRadio.Test.Mocks
{
    public class SongRepositoryBuilder
    {
        private readonly Mock<ISongRepository> _mock;
        public readonly List<(UserId userId, PlaylistId playlistId, SongId songId, string title, string artist)> Songs;

        public SongRepositoryBuilder()
        {
            _mock = new Mock<ISongRepository>();
            Songs = new List<(UserId userId, PlaylistId playlistId, SongId songId, string title, string artist)>();
        }
        public static SongRepositoryBuilder Create()
        {
            return new SongRepositoryBuilder();
        }

        public ISongRepository Build()
        {
            _mock.Setup(x => x.Add(It.IsAny<UserId>(), It.IsAny<PlaylistId>(), It.IsAny<SongId>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback<UserId, PlaylistId, SongId, string, string>((userId, playlistId, songId, title, artist) => Songs.Add((userId, playlistId, songId, title, artist)));

            return _mock.Object;
        }
    }
}