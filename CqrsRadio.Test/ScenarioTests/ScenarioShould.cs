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

            var email = "nicolas.dfr@gmail.com";
            var nickName = "nicolas";
            var userId = "12345";
            var accessToken = "accessToken";
            var playlistId = PlaylistId.Parse("100");
            var playlistname = "playlistName";

            var user = User.Create(stream, publisher, email, nickName, userId, accessToken);
            user.AddPlaylist(playlistId, playlistname);
            user.AddSongToPlaylist(SongId.Parse("101"), "titleOne", "artistOne");
            user.AddSongToPlaylist(SongId.Parse("102"), "titleTwo", "artistOne");

            var identity = Identity.Parse(email, nickName, userId, accessToken);

            Assert.IsTrue(stream.GetEvents().Contains(new UserCreated(identity)));
            Assert.IsTrue(stream.GetEvents().Contains(new PlaylistAdded(UserId.Parse(userId), playlistId, playlistname)));
            Assert.IsTrue(stream.GetEvents().Contains(new SongAdded(UserId.Parse(userId), playlistId, SongId.Parse("101"), "titleOne", "artistOne")));
            Assert.IsTrue(stream.GetEvents().Contains(new SongAdded(UserId.Parse(userId), playlistId, SongId.Parse("102"), "titleTwo", "artistOne")));
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
            var email = "nicolas.dfr@gmail.com";
            var nickName = "nicolas";
            var userId = "12345";
            var accessToken = "accessToken";
            var playlistId = PlaylistId.Parse("100");

            var deezerSong = new List<DeezerSong>
            {
                new DeezerSong(SongId.Parse("101"), "title1", "artist1"),
                new DeezerSong(SongId.Parse("102"), "title2", "artist2"),
                new DeezerSong(SongId.Parse("103"), "title3", "artist3")
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
            var user = User.Create(stream, publisher, email, nickName, userId, accessToken);
            user.AddPlaylist(playlistId, "bestof");

            var songs = songEngine.GetRandomisedSongs(3);
            foreach (var song in songs)
            {
                user.AddSongToPlaylist(song.Id, song.Title, song.Artist);
            }

            var actualPlaylistId = deezerApi.SetCreatePlaylist(playlistId).Build().CreatePlaylist(user.Identity.AccessToken, user.Identity.UserId, user.Playlist.Name);
            deezerApi.Build().AddSongsToPlaylist("accessToken", user.Playlist.PlaylistId, user.Playlist.Songs.Select(x=>x.SongId).ToArray());

            // assert

            var identity = Identity.Parse("nicolas.dfr@gmail.com", "nicolas", "12345", "accessToken");
            Assert.IsTrue(stream.GetEvents().Contains(new UserCreated(identity)));
            Assert.IsTrue(stream.GetEvents().Contains(new PlaylistAdded(UserId.Parse(userId), playlistId, "bestof")));
            Assert.IsTrue(stream.GetEvents().Contains(new SongAdded(UserId.Parse(userId), playlistId, SongId.Parse("101"), "title1", "artist1")));
            Assert.IsTrue(stream.GetEvents().Contains(new SongAdded(UserId.Parse(userId), playlistId, SongId.Parse("102"), "title2", "artist2")));
            Assert.IsTrue(stream.GetEvents().Contains(new SongAdded(UserId.Parse(userId), playlistId, SongId.Parse("103"), "title3", "artist3")));
            Assert.AreEqual(playlistId, actualPlaylistId);
            Assert.AreEqual(0, deezerApi.PlaylistDeleted);
            Assert.AreEqual(3, deezerApi.SongsAdded);
        }
    }
}
