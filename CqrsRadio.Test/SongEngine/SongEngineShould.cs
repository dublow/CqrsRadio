using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public static SongEngineBuilder Create()
        {
            throw new NotImplementedException();
        }

        public SongEngineBuilder SetRandomisedSongs(string[] songIds)
        {
            throw new NotImplementedException();
        }

        public ISongEngine Build()
        {
            throw new NotImplementedException();
        }
    }

    public interface ISongEngine    
    {
        IEnumerable<string> GetRandomisedSongs(int length);
    }
}
