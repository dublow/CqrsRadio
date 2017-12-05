using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;

namespace CqrsRadio.Test.SongEngine
{
    [TestFixture]
    public class SongEngineShould
    {
        [Test]
        public void ReturnThreeDeezerSongIdsWhenPassLengthToThree()
        {
            // arrange
            var songEngine = SongEngineBuilder
                .Create()
                .SetRandomisedSongs(new[] { "123", "456", "789" }).Build();
            // act
            var actual = songEngine.GetRandomisedSongs(3);
            // assert
            Assert.AreEqual(3, actual.Count());
            Assert.AreEqual("123", actual.ElementAt(0));
            Assert.AreEqual("456", actual.ElementAt(1));
            Assert.AreEqual("789", actual.ElementAt(2));
        }
    }

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

        public SongEngineBuilder SetRandomisedSongs(string[] songIds)
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

    public interface ISongEngine    
    {
        IEnumerable<string> GetRandomisedSongs(int length);
    }
}
