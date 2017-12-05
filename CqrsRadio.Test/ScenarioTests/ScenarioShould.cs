using System;
using System.Collections.Generic;
using System.Linq;
using CqrsRadio.Domain.Aggregates;
using CqrsRadio.Domain.Entities;
using CqrsRadio.Domain.Events;
using CqrsRadio.Domain.ValueTypes;
using CqrsRadio.Infrastructure.Bus;
using CqrsRadio.Infrastructure.EventStores;
using CqrsRadio.Test.Mocks;
using NUnit.Framework;

namespace CqrsRadio.Test.ScenarioTests
{
    [TestFixture]
    public class ScenarioShould
    {
        [Test]
        public void CreateUserWithOnePlaylistAndTwoSong()
        {
            var stream = new MemoryEventStream();
            var publisher = new EventBus(stream);

            var user = User.Create(stream, publisher, "nicolas.dfr@gmail.com", "nicolas", "12345");
            user.AddPlaylist("bestof");
            user.AddSongToPlaylist("bestof", "123", "rock", "titleOne", "artistOne");
            user.AddSongToPlaylist("bestof", "456", "rock", "titleTwo", "artistOne");

            Assert.IsTrue(stream.GetEvents().Contains(new UserCreated(Identity.Create("nicolas.dfr@gmail.com", "nicolas", "12345"))));
            Assert.IsTrue(stream.GetEvents().Contains(new PlaylistAdded("12345", "bestof")));
            Assert.IsTrue(stream.GetEvents().Contains(new SongAdded("12345", "bestof", "123", "rock", "titleOne", "artistOne")));
            Assert.IsTrue(stream.GetEvents().Contains(new SongAdded("12345", "bestof", "456", "rock", "titleTwo", "artistOne")));
        }

        [Test]
        public void CreateRadioAndSearchSong()
        {
            var stream = new MemoryEventStream();
            var publisher = new EventBus(stream);

            var radioEngine = RadioEngineBuilder
                .Create()
                .SetParser("title", "artist")
                .Build();

            var radio = Radio.Create(stream, publisher, radioEngine, "djam", "http://djam.fr");
            radio.SearchSong();

            Assert.IsTrue(stream.GetEvents().Contains(new RadioCreated("djam", new Uri("http://djam.fr"))));
            Assert.IsTrue(stream.GetEvents().Contains(new RadioSongParsed("djam", "title", "artist")));
        }

        [Test]
        public void CreateUserWithOnePlaylistAndAddSongsWithRandomizedSongs()
        {
            // arrange
            var deezerSong = new List<DeezerSong>()
            {
                new DeezerSong("123", "rock", "title1", "artist1"),
                new DeezerSong("456", "jazz", "title2", "artist2"),
                new DeezerSong("789", "rap", "title3", "artist3")
            };
            var stream = new MemoryEventStream();
            var publisher = new EventBus(stream);
            var songEngine = SongEngineBuilder
                .Create()
                .SetRandomisedSongs(deezerSong)
                .Build();
            var deezerApi = DeezerApiBuilder
                .Create();
            // act
            var user = User.Create(stream, publisher, "nicolas.dfr@gmail.com", "nicolas", "12345");
            user.AddPlaylist("bestof");

            var songs = songEngine.GetRandomisedSongs(3);
            foreach (var song in songs)
            {
                user.AddSongToPlaylist("bestof", song.Id, song.Genre, song.Title, song.Artist);
            }

            var playlist = user.GetPlaylist("bestof");
            deezerApi.Build().CreatePlaylist(playlist.UserId, playlist.Name);
            deezerApi.Build().AddSongsToPlaylist(playlist.Name, playlist.Songs.Select(x=>x.SongId).ToArray());

            // assert
            Assert.IsTrue(stream.GetEvents().Contains(new UserCreated(Identity.Create("nicolas.dfr@gmail.com", "nicolas", "12345"))));
            Assert.IsTrue(stream.GetEvents().Contains(new PlaylistAdded("12345", "bestof")));
            Assert.IsTrue(stream.GetEvents().Contains(new SongAdded("12345", "bestof", "123", "rock", "title1", "artist1")));
            Assert.IsTrue(stream.GetEvents().Contains(new SongAdded("12345", "bestof", "456", "jazz", "title2", "artist2")));
            Assert.IsTrue(stream.GetEvents().Contains(new SongAdded("12345", "bestof", "789", "rap", "title3", "artist3")));
            Assert.AreEqual(1, deezerApi.PlaylistAdded);
            Assert.AreEqual(0, deezerApi.PlaylistDeleted);
            Assert.AreEqual(3, deezerApi.SongsAdded);
        }
    }
}
