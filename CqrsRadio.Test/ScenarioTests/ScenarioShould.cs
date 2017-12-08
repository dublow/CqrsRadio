﻿using System;
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
            user.AddPlaylist("123", "bestof");
            user.AddSongToPlaylist("bestof", "123", "titleOne", "artistOne");
            user.AddSongToPlaylist("bestof", "456", "titleTwo", "artistOne");

            Assert.IsTrue(stream.GetEvents().Contains(new UserCreated(Identity.Create("nicolas.dfr@gmail.com", "nicolas", "12345"))));
            Assert.IsTrue(stream.GetEvents().Contains(new PlaylistAdded("12345", "123", "bestof")));
            Assert.IsTrue(stream.GetEvents().Contains(new SongAdded("12345", "bestof", "123", "titleOne", "artistOne")));
            Assert.IsTrue(stream.GetEvents().Contains(new SongAdded("12345", "bestof", "456", "titleTwo", "artistOne")));
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
                new DeezerSong("123", "title1", "artist1"),
                new DeezerSong("456", "title2", "artist2"),
                new DeezerSong("789", "title3", "artist3")
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
            user.AddPlaylist("123", "bestof");

            var songs = songEngine.GetRandomisedSongs(3);
            foreach (var song in songs)
            {
                user.AddSongToPlaylist("bestof", song.Id, song.Title, song.Artist);
            }

            var playlist = user.GetPlaylist("bestof");
            var playlistId = deezerApi.SetCreatePlaylist("123").Build().CreatePlaylist("accessToken", playlist.UserId, playlist.Name);
            deezerApi.Build().AddSongsToPlaylist("accessToken", playlist.PlaylistId, playlist.Songs.Select(x=>x.SongId).ToArray());

            // assert
            Assert.IsTrue(stream.GetEvents().Contains(new UserCreated(Identity.Create("nicolas.dfr@gmail.com", "nicolas", "12345"))));
            Assert.IsTrue(stream.GetEvents().Contains(new PlaylistAdded("12345", "123", "bestof")));
            Assert.IsTrue(stream.GetEvents().Contains(new SongAdded("12345", "bestof", "123", "title1", "artist1")));
            Assert.IsTrue(stream.GetEvents().Contains(new SongAdded("12345", "bestof", "456", "title2", "artist2")));
            Assert.IsTrue(stream.GetEvents().Contains(new SongAdded("12345", "bestof", "789", "title3", "artist3")));
            Assert.AreEqual(PlaylistId.Parse("123"), playlistId);
            Assert.AreEqual(0, deezerApi.PlaylistDeleted);
            Assert.AreEqual(3, deezerApi.SongsAdded);
        }
    }
}
