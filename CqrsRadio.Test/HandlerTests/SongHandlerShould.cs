using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CqrsRadio.Domain.EventHandlers;
using CqrsRadio.Domain.Events;
using NUnit.Framework;

namespace CqrsRadio.Test.HandlerTests
{
    [TestFixture]
    public class SongHandlerShould
    {
        [Test]
        public void UseRepositoryWhenSongIsAdded()
        {
            // arrange
            var mockedSongRepository = SongRepositoryBuilder.Create();
            var radioRepository = mockedSongRepository.Build();
            var songHandler = new SongHandler(radioRepository);
            (string playlistName, string title, string artist) = ("bestof", "title", "artist");
            // act
            songHandler.Handle(new SongAdded("bestof", "title", "artist"));
            // assert
            var (actualPlaylistName, actualTitle, actualArtist) = mockedSongRepository.Songs.First();
            Assert.AreEqual(playlistName, actualPlaylistName);
            Assert.AreEqual(title, actualTitle);
            Assert.AreEqual(artist, actualArtist);
        }
    }

    public class SongHandler : ISongHandler
    {
        public SongHandler(ISongRepository radioRepository)
        {
            throw new NotImplementedException();
        }

        public void Handle(SongAdded evt)
        {
            throw new NotImplementedException();
        }
    }

    public interface ISongRepository
    {
    }

    public class SongRepositoryBuilder
    {
        public static SongRepositoryBuilder Create()
        {
            throw new NotImplementedException();
        }

        public ISongRepository Build()
        {
            throw new NotImplementedException();
        }

        public List<(string playlistName, string title, string artist)> Songs { get; set; }
    }
}
