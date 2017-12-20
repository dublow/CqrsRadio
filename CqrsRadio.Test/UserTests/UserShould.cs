using System.Linq;
using CqrsRadio.Domain.Aggregates;
using CqrsRadio.Domain.Entities;
using CqrsRadio.Domain.Events;
using CqrsRadio.Domain.ValueTypes;
using CqrsRadio.Infrastructure.Bus;
using CqrsRadio.Infrastructure.EventStores;
using CqrsRadio.Test.Mocks;
using NUnit.Framework;

namespace CqrsRadio.Test.UserTests
{
    [TestFixture]
    public class UserShould
    {
        [Test]
        public void RaiseMessageWhenCreateUser()
        {
            var stream = new MemoryEventStream();
            var publisher = new EventBus(stream);
            var deezerApi = DeezerApiBuilder.Create().Build();
            var songRepository = SongRepositoryBuilder.Create().Build();
            var playlistRepository = PlaylistRepositoryBuilder.Create().Build();

            User.Create(stream, publisher, deezerApi, songRepository, playlistRepository, "nicolas.dfr@gmail.com", "dublow", "12345", "accessToken");

            var identity = Identity.Parse("nicolas.dfr@gmail.com", "dublow", "12345", "accessToken");
            Assert.IsTrue(stream.GetEvents().Contains(new UserCreated(identity)));
        }

        [Test]
        public void RaiseMessageWhenDeleteUser()
        {
            var stream = new MemoryEventStream();
            var deezerApi = DeezerApiBuilder.Create().Build();
            var songRepository = SongRepositoryBuilder.Create().Build();
            var playlistRepository = PlaylistRepositoryBuilder.Create().Build();
            stream.Add(new UserCreated(Identity.Parse("nicolas.dfr@gmail.com", "dublow", "12345", "accessToken")));
            
            var publisher = new EventBus(stream);

            var user = new User(stream, publisher, deezerApi, songRepository, playlistRepository, 1);

            user.Delete();
            
            Assert.IsTrue(stream.GetEvents().Contains(new UserDeleted(UserId.Parse("12345"))));
        }

        [Test]
        public void NoRaiseMessageWhenDeleteDeletedUser()
        {
            var stream = new MemoryEventStream();
            var deezerApi = DeezerApiBuilder.Create().Build();
            var songRepository = SongRepositoryBuilder.Create().Build();
            var playlistRepository = PlaylistRepositoryBuilder.Create().Build();
            stream.Add(new UserCreated(Identity.Parse("nicolas.dfr@gmail.com", "dublow", "12345", "accessToken")));
            stream.Add(new UserDeleted(UserId.Parse("12345")));

            var publisher = new EventBus(stream);

            var user = new User(stream, publisher, deezerApi, songRepository, playlistRepository, 1);

            user.Delete();

            Assert.IsTrue(stream.GetEvents().Contains(new UserDeleted(UserId.Parse("12345"))));
        }

        [Test]
        public void NoRaiseMessageWhenTwiceDeleteUser()
        {
            var stream = new MemoryEventStream();
            var deezerApi = DeezerApiBuilder.Create().Build();
            var songRepository = SongRepositoryBuilder.Create().Build();
            var playlistRepository = PlaylistRepositoryBuilder.Create().Build();
            stream.Add(new UserCreated(Identity.Parse("nicolas.dfr@gmail.com", "dublow", "12345", "accessToken")));
            
            var publisher = new EventBus(stream);

            var user = new User(stream, publisher, deezerApi, songRepository, playlistRepository, 1);

            user.Delete();
            user.Delete();

            Assert.IsTrue(stream.GetEvents().Contains(new UserDeleted(UserId.Parse("12345"))));
        }

        [Test]
        public void RaiseMessageWhenAddPlaylistToUser()
        {
            var stream = new MemoryEventStream();
            var deezerApi = DeezerApiBuilder.Create().SetCreatePlaylist(PlaylistId.Parse("100")).Build();
            var songRepository = SongRepositoryBuilder.Create()
                .SetRandomSongs(1, new[] {new Song(SongId.Parse("100"), "title", "artist")}).Build();
            var playlistRepository = PlaylistRepositoryBuilder.Create().Build();
            stream.Add(new UserCreated(Identity.Parse("nicolas.dfr@gmail.com", "dublow", "12345", "accessToken")));

            var publisher = new EventBus(stream);

            var user = new User(stream, publisher, deezerApi, songRepository, playlistRepository, 1);

            user.AddPlaylist("playlistName");

            Assert.IsTrue(stream.GetEvents().Contains(new PlaylistAdded(UserId.Parse("12345"), PlaylistId.Parse("100"), "playlistName")));
        }

        [Test]
        public void NoRaiseMessageWhenAddExistingPlaylistToUser()
        {
            var stream = new MemoryEventStream();
            var deezerApi = DeezerApiBuilder.Create().Build();
            var songRepository = SongRepositoryBuilder.Create().Build();
            var playlistRepository = PlaylistRepositoryBuilder.Create().Build();
            stream.Add(new UserCreated(Identity.Parse("nicolas.dfr@gmail.com", "dublow", "12345", "accessToken")));
            stream.Add(new PlaylistAdded(UserId.Parse("12345"), PlaylistId.Parse("100"), "playlistName"));

            var publisher = new EventBus(stream);

            var user = new User(stream, publisher, deezerApi, songRepository, playlistRepository, 1);

            user.AddPlaylist("playlistName");

            Assert.IsTrue(stream.GetEvents().Contains(new PlaylistAdded(UserId.Parse("12345"), PlaylistId.Parse("100"), "playlistName")));
            Assert.AreEqual(1, stream.GetEvents().OfType<PlaylistAdded>().Count());
        }

        [Test]
        public void NoRaiseMessageWhenAddPlaylistWithDeletedUser()
        {
            var stream = new MemoryEventStream();
            var deezerApi = DeezerApiBuilder.Create().Build();
            var songRepository = SongRepositoryBuilder.Create().Build();
            var playlistRepository = PlaylistRepositoryBuilder.Create().Build();
            stream.Add(new UserCreated(Identity.Parse("nicolas.dfr@gmail.com", "dublow", "12345", "accessToken")));
            stream.Add(new UserDeleted(UserId.Parse("12345")));

            var publisher = new EventBus(stream);

            var user = new User(stream, publisher, deezerApi, songRepository, playlistRepository, 1);

            user.AddPlaylist("playlistName");

            Assert.AreEqual(0, stream.GetEvents().OfType<PlaylistAdded>().Count());
        }
    }
}
