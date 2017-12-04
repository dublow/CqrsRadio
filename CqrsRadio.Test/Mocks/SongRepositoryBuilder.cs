using System.Collections.Generic;
using CqrsRadio.Domain.Repositories;
using CqrsRadio.Domain.ValueTypes;
using Moq;

namespace CqrsRadio.Test.Mocks
{
    public class SongRepositoryBuilder
    {
        private readonly Mock<ISongRepository> _mock;
        public readonly List<(UserId userId, string playlistName, string title, string artist)> Songs;

        public SongRepositoryBuilder()
        {
            _mock = new Mock<ISongRepository>();
            Songs = new List<(UserId userId, string playlistName, string title, string artist)>();
        }
        public static SongRepositoryBuilder Create()
        {
            return new SongRepositoryBuilder();
        }

        public ISongRepository Build()
        {
            _mock.Setup(x => x.Add(It.IsAny<UserId>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback<UserId, string, string, string>((userId, name, title, artist) => Songs.Add((userId, name, title, artist)));

            return _mock.Object;
        }
    }
}