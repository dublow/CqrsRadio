using System.Linq;
using CqrsRadio.Domain.Aggregates;
using CqrsRadio.Domain.Events;
using CqrsRadio.Domain.ValueTypes;
using CqrsRadio.Infrastructure.Bus;
using CqrsRadio.Infrastructure.EventStores;
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

            User.Create(stream, publisher, "nicolas.dfr@gmail.com", "dublow", "12345");

            var identity = Identity.Create("nicolas.dfr@gmail.com", "dublow", "12345");
            Assert.IsTrue(stream.GetEvents().Contains(new UserCreated(identity)));
        }

        [Test]
        public void RaiseMessageWhenDeleteUser()
        {
            var stream = new MemoryEventStream();
            stream.Add(new UserCreated(Identity.Create("nicolas.dfr@gmail.com", "dublow", "12345")));

            var publisher = new EventBus(stream);

            var user = new User(stream, publisher);

            user.Delete();
            
            Assert.IsTrue(stream.GetEvents().Contains(new UserDeleted("12345")));
        }

        [Test]
        public void NoRaiseMessageWhenDeleteDeletedUser()
        {
            var stream = new MemoryEventStream();
            stream.Add(new UserCreated(Identity.Create("nicolas.dfr@gmail.com", "dublow", "12345")));
            stream.Add(new UserDeleted("12345"));

            var publisher = new EventBus(stream);

            var user = new User(stream, publisher);

            user.Delete();

            Assert.IsTrue(stream.GetEvents().Contains(new UserDeleted("12345")));
        }

        [Test]
        public void NoRaiseMessageWhenTwiceDeleteUser()
        {
            var stream = new MemoryEventStream();
            stream.Add(new UserCreated(Identity.Create("nicolas.dfr@gmail.com", "dublow", "12345")));
            
            var publisher = new EventBus(stream);

            var user = new User(stream, publisher);

            user.Delete();
            user.Delete();

            Assert.IsTrue(stream.GetEvents().Contains(new UserDeleted("12345")));
        }

        [Test]
        public void RaiseMessageWhenAddPlaylistToUser()
        {
            var stream = new MemoryEventStream();
            stream.Add(new UserCreated(Identity.Create("nicolas.dfr@gmail.com", "dublow", "12345")));

            var publisher = new EventBus(stream);

            var user = new User(stream, publisher);

            user.AddPlaylist("123", "name");

            Assert.IsTrue(stream.GetEvents().Contains(new PlaylistAdded("12345", "123", "name")));
        }

        [Test]
        public void NoRaiseMessageWhenAddExistingPlaylistToUser()
        {
            var stream = new MemoryEventStream();
            stream.Add(new UserCreated(Identity.Create("nicolas.dfr@gmail.com", "dublow", "12345")));
            stream.Add(new PlaylistAdded("12345", "123", "name"));

            var publisher = new EventBus(stream);

            var user = new User(stream, publisher);

            user.AddPlaylist("123", "name");

            Assert.IsTrue(stream.GetEvents().Contains(new PlaylistAdded("12345", "123", "name")));
            Assert.AreEqual(1, stream.GetEvents().OfType<PlaylistAdded>().Count());
        }

        [Test]
        public void NoRaiseMessageWhenAddPlaylistWithDeletedUser()
        {
            var stream = new MemoryEventStream();
            stream.Add(new UserCreated(Identity.Create("nicolas.dfr@gmail.com", "dublow", "12345")));
            stream.Add(new UserDeleted());

            var publisher = new EventBus(stream);

            var user = new User(stream, publisher);

            user.AddPlaylist("123", "name");

            Assert.AreEqual(0, stream.GetEvents().OfType<PlaylistAdded>().Count());
        }

        [Test]
        public void RaiseMessageWhenDeletePlaylist()
        {
            var stream = new MemoryEventStream();
            stream.Add(new UserCreated(Identity.Create("nicolas.dfr@gmail.com", "dublow", "12345")));
            stream.Add(new PlaylistAdded("12345", "123", "name"));

            var publisher = new EventBus(stream);

            var user = new User(stream, publisher);

            user.DeletePlaylist("123", "name");

            Assert.IsTrue(stream.GetEvents().Contains(new PlaylistDeleted("12345", "123", "name")));
            Assert.AreEqual(1, stream.GetEvents().OfType<PlaylistDeleted>().Count());
        }

        [Test]
        public void NoRaiseMessageWhenDeleteDeletedPlaylist()
        {
            var stream = new MemoryEventStream();
            stream.Add(new UserCreated(Identity.Create("nicolas.dfr@gmail.com", "dublow", "12345")));
            stream.Add(new PlaylistAdded("12345", "123", "name"));
            stream.Add(new PlaylistDeleted("12345", "123", "name"));

            var publisher = new EventBus(stream);

            var user = new User(stream, publisher);

            user.DeletePlaylist("123", "name");

            Assert.IsTrue(stream.GetEvents().Contains(new PlaylistDeleted("12345", "123", "name")));
            Assert.AreEqual(1, stream.GetEvents().OfType<PlaylistDeleted>().Count());
        }

        [Test]
        public void NoRaiseMessageWhenDeletePlaylistWithDeletedUser()
        {
            var stream = new MemoryEventStream();
            stream.Add(new UserCreated(Identity.Create("nicolas.dfr@gmail.com", "dublow", "12345")));
            stream.Add(new PlaylistAdded("12345", "123", "name"));
            stream.Add(new UserDeleted());

            var publisher = new EventBus(stream);

            var user = new User(stream, publisher);

            user.DeletePlaylist("123", "name");

            Assert.AreEqual(0, stream.GetEvents().OfType<PlaylistDeleted>().Count());
        }

        [Test]
        public void RaiseMessageWhenAddSongToPlaylist()
        {
            var stream = new MemoryEventStream();
            stream.Add(new UserCreated(Identity.Create("nicolas.dfr@gmail.com", "dublow", "12345")));
            stream.Add(new PlaylistAdded("12345", "123", "name"));

            var publisher = new EventBus(stream);

            var user = new User(stream, publisher);

            user.AddSongToPlaylist("name", "123", "title", "artist");

            Assert.IsTrue(stream.GetEvents().Contains(new SongAdded("12345", "name", "123", "title", "artist")));
            Assert.AreEqual(1, stream.GetEvents().OfType<SongAdded>().Count());
        }

        [Test]
        public void NoRaiseMessageWhenAddSongAlreadyAddedToPlaylist()
        {
            var stream = new MemoryEventStream();
            stream.Add(new UserCreated(Identity.Create("nicolas.dfr@gmail.com", "dublow", "12345")));
            stream.Add(new PlaylistAdded("12345", "123", "name"));
            stream.Add(new SongAdded("12345", "name", "123", "title", "artist"));

            var publisher = new EventBus(stream);

            var user = new User(stream, publisher);

            user.AddSongToPlaylist("name", "123", "title", "artist");

            Assert.IsTrue(stream.GetEvents().Contains(new SongAdded("12345", "name", "123", "title", "artist")));
            Assert.AreEqual(1, stream.GetEvents().OfType<SongAdded>().Count());
        }

        [Test]
        public void NoRaiseMessageWhenAddSongWithDeletedUser()
        {
            var stream = new MemoryEventStream();
            stream.Add(new UserCreated(Identity.Create("nicolas.dfr@gmail.com", "dublow", "12345")));
            stream.Add(new PlaylistAdded("12345", "123", "name"));
            stream.Add(new UserDeleted());

            var publisher = new EventBus(stream);

            var user = new User(stream, publisher);

            user.AddSongToPlaylist("name", "123", "title", "artist");

            Assert.AreEqual(0, stream.GetEvents().OfType<SongAdded>().Count());
        }

        [Test]
        public void RaiseMessageWhenClearPlaylist()
        {
            var stream = new MemoryEventStream();
            stream.Add(new UserCreated(Identity.Create("nicolas.dfr@gmail.com", "dublow", "12345")));
            stream.Add(new PlaylistAdded("12345", "123", "name"));

            var publisher = new EventBus(stream);

            var user = new User(stream, publisher);

            user.ClearPlaylists();

            Assert.IsTrue(stream.GetEvents().Contains(new PlaylistsCleared("12345")));
            Assert.AreEqual(1, stream.GetEvents().OfType<PlaylistsCleared>().Count());
        }

        [Test]
        public void RaiseMessageWhenAddAccessToken()
        {
            var stream = new MemoryEventStream();
            stream.Add(new UserCreated(Identity.Create("nicolas.dfr@gmail.com", "dublow", "12345")));

            var publisher = new EventBus(stream);

            var user = new User(stream, publisher);

            user.AddAccessToken("accessToken");
            Assert.IsTrue(stream.GetEvents().Contains(new AccessTokenAdded("12345")));
            Assert.AreEqual(1, stream.GetEvents().OfType<AccessTokenAdded>().Count());
        }

        [Test]
        public void NoRaiseMessageWhenAddAccessTokenWithDeletedUser()
        {
            var stream = new MemoryEventStream();
            stream.Add(new UserCreated(Identity.Create("nicolas.dfr@gmail.com", "dublow", "12345")));
            stream.Add(new UserDeleted("12345"));

            var publisher = new EventBus(stream);

            var user = new User(stream, publisher);

            user.AddAccessToken("accessToken");
            Assert.AreEqual(0, stream.GetEvents().OfType<AccessTokenAdded>().Count());
        }
    }
}
