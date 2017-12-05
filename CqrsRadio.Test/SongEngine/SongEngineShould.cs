using System.Linq;
using CqrsRadio.Test.Mocks;
using NUnit.Framework;

namespace CqrsRadio.Test.SongEngine
{
    [TestFixture]
    public class SongEngineShould
    {
        [Test]
        public void ReturnDeezerSongIdsWhenPassLength()
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
}
