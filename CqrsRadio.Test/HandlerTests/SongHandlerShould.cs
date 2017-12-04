using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CqrsRadio.Domain.EventHandlers;
using CqrsRadio.Domain.Events;
using CqrsRadio.Domain.ValueTypes;
using Moq;
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
            var songRepository = mockedSongRepository.Build();
            var songHandler = new SongHandler(songRepository);
            (UserId userId, string playlistName, string title, string artist) = ("12345", "bestof", "title", "artist");
            // act
            songHandler.Handle(new SongAdded("12345", "bestof", "title", "artist"));
            // assert
            var (actualUserId, actualPlaylistName, actualTitle, actualArtist) = mockedSongRepository.Songs.First();
            Assert.AreEqual(userId, actualUserId);
            Assert.AreEqual(playlistName, actualPlaylistName);
            Assert.AreEqual(title, actualTitle);
            Assert.AreEqual(artist, actualArtist);
        }
    }

    public class SongHandler : ISongHandler
    {
        private readonly ISongRepository _songRepository;

        public SongHandler(ISongRepository songRepository)
        {
            _songRepository = songRepository;
        }

        public void Handle(SongAdded evt)
        {
            _songRepository.Add(evt.UserId, evt.PlaylistName, evt.Title, evt.Artist);
        }
    }

    public interface ISongRepository
    {
        void Add(UserId userId, string playlistName, string title, string artist);
    }

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
