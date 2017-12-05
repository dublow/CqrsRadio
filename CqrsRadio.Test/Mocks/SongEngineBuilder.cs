using System.Collections.Generic;
using CqrsRadio.Domain.Entities;
using CqrsRadio.Test.SongEngine;
using Moq;

namespace CqrsRadio.Test.Mocks
{
    public class SongEngineBuilder  
    {
        private readonly Mock<ISongEngine> _mock;

        public SongEngineBuilder()
        {
            _mock = new Mock<ISongEngine>();
        }

        public static SongEngineBuilder Create()
        {
            return new SongEngineBuilder();
        }

        public SongEngineBuilder SetRandomisedSongs(List<DeezerSong> songIds)
        {
            _mock.Setup(x => x.GetRandomisedSongs(It.IsAny<int>()))
                .Returns(songIds);

            return this;
        }

        public ISongEngine Build()
        {
            return _mock.Object;
        }
    }
}