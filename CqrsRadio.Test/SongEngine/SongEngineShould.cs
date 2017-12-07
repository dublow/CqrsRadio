using System.Collections.Generic;
using System.Linq;
using CqrsRadio.Domain.Entities;
using CqrsRadio.Domain.ValueTypes;
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
            var deezerSong = new List<DeezerSong>()
            {
                new DeezerSong("123", "rock", "title1", "artist1"),
                new DeezerSong("456", "jazz", "title2", "artist2"),
                new DeezerSong("789", "rap", "title3", "artist3")
            };
            var songEngine = SongEngineBuilder
                .Create()
                .SetRandomisedSongs(deezerSong).Build();
            // act
            var actual = songEngine.GetRandomisedSongs(3);
            // assert
            Assert.AreEqual(3, actual.Count());
            Assert.AreEqual(SongId.Parse("123"), actual.ElementAt(0).Id);
            Assert.AreEqual(SongId.Parse("456"), actual.ElementAt(1).Id);
            Assert.AreEqual(SongId.Parse("789"), actual.ElementAt(2).Id);
            Assert.AreEqual("rock", actual.ElementAt(0).Genre);
            Assert.AreEqual("jazz", actual.ElementAt(1).Genre);
            Assert.AreEqual("rap", actual.ElementAt(2).Genre);
            Assert.AreEqual("title1", actual.ElementAt(0).Title);
            Assert.AreEqual("title2", actual.ElementAt(1).Title);
            Assert.AreEqual("title3", actual.ElementAt(2).Title);
            Assert.AreEqual("artist1", actual.ElementAt(0).Artist);
            Assert.AreEqual("artist2", actual.ElementAt(1).Artist);
            Assert.AreEqual("artist3", actual.ElementAt(2).Artist);
        }
    }
}
