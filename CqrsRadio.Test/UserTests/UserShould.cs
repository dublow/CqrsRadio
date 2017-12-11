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

            User.Create(stream, publisher, "nicolas.dfr@gmail.com", "dublow", "12345", "accessToken");

            var identity = Identity.Parse("nicolas.dfr@gmail.com", "dublow", "12345", "accessToken");
            Assert.IsTrue(stream.GetEvents().Contains(new UserCreated(identity)));
        }

        [Test]
        public void RaiseMessageWhenDeleteUser()
        {
            var stream = new MemoryEventStream();
            stream.Add(new UserCreated(Identity.Parse("nicolas.dfr@gmail.com", "dublow", "12345", "accessToken")));

            var publisher = new EventBus(stream);

            var user = new User(stream, publisher);

            user.Delete();
            
            Assert.IsTrue(stream.GetEvents().Contains(new UserDeleted(UserId.Parse("12345"))));
        }

        [Test]
        public void NoRaiseMessageWhenDeleteDeletedUser()
        {
            var stream = new MemoryEventStream();
            stream.Add(new UserCreated(Identity.Parse("nicolas.dfr@gmail.com", "dublow", "12345", "accessToken")));
            stream.Add(new UserDeleted(UserId.Parse("12345")));

            var publisher = new EventBus(stream);

            var user = new User(stream, publisher);

            user.Delete();

            Assert.IsTrue(stream.GetEvents().Contains(new UserDeleted(UserId.Parse("12345"))));
        }

        [Test]
        public void NoRaiseMessageWhenTwiceDeleteUser()
        {
            var stream = new MemoryEventStream();
            stream.Add(new UserCreated(Identity.Parse("nicolas.dfr@gmail.com", "dublow", "12345", "accessToken")));
            
            var publisher = new EventBus(stream);

            var user = new User(stream, publisher);

            user.Delete();
            user.Delete();

            Assert.IsTrue(stream.GetEvents().Contains(new UserDeleted(UserId.Parse("12345"))));
        }

        [Test]
        public void RaiseMessageWhenAddPlaylistToUser()
        {
            var stream = new MemoryEventStream();
            stream.Add(new UserCreated(Identity.Parse("nicolas.dfr@gmail.com", "dublow", "12345", "accessToken")));

            var publisher = new EventBus(stream);

            var user = new User(stream, publisher);

            user.AddPlaylist(PlaylistId.Parse("100"), "playlistName");

            Assert.IsTrue(stream.GetEvents().Contains(new PlaylistAdded(UserId.Parse("12345"), PlaylistId.Parse("100"), "playlistName")));
        }

        [Test]
        public void NoRaiseMessageWhenAddExistingPlaylistToUser()
        {
            var stream = new MemoryEventStream();
            stream.Add(new UserCreated(Identity.Parse("nicolas.dfr@gmail.com", "dublow", "12345", "accessToken")));
            stream.Add(new PlaylistAdded(UserId.Parse("12345"), PlaylistId.Parse("100"), "name"));

            var publisher = new EventBus(stream);

            var user = new User(stream, publisher);

            user.AddPlaylist(PlaylistId.Parse("100"), "playlistName");

            Assert.IsTrue(stream.GetEvents().Contains(new PlaylistAdded(UserId.Parse("12345"), PlaylistId.Parse("100"), "name")));
            Assert.AreEqual(1, stream.GetEvents().OfType<PlaylistAdded>().Count());
        }

        [Test]
        public void NoRaiseMessageWhenAddPlaylistWithDeletedUser()
        {
            var stream = new MemoryEventStream();
            stream.Add(new UserCreated(Identity.Parse("nicolas.dfr@gmail.com", "dublow", "12345", "accessToken")));
            stream.Add(new UserDeleted(UserId.Parse("12345")));

            var publisher = new EventBus(stream);

            var user = new User(stream, publisher);

            user.AddPlaylist(PlaylistId.Parse("100"), "playlistName");

            Assert.AreEqual(0, stream.GetEvents().OfType<PlaylistAdded>().Count());
        }

        [Test]
        public void RaiseMessageWhenDeletePlaylist()
        {
            var stream = new MemoryEventStream();
            stream.Add(new UserCreated(Identity.Parse("nicolas.dfr@gmail.com", "dublow", "12345", "accessToken")));
            stream.Add(new PlaylistAdded(UserId.Parse("12345"), PlaylistId.Parse("100"), "playlistName"));

            var publisher = new EventBus(stream);

            var user = new User(stream, publisher);

            user.DeletePlaylist(PlaylistId.Parse("100"), "playlistName");

            Assert.IsTrue(stream.GetEvents().Contains(new PlaylistDeleted(UserId.Parse("12345"), PlaylistId.Parse("100"), "playlistName")));
            Assert.AreEqual(1, stream.GetEvents().OfType<PlaylistDeleted>().Count());
        }

        [Test]
        public void NoRaiseMessageWhenDeleteDeletedPlaylist()
        {
            var stream = new MemoryEventStream();
            stream.Add(new UserCreated(Identity.Parse("nicolas.dfr@gmail.com", "dublow", "12345", "accessToken")));
            stream.Add(new PlaylistAdded(UserId.Parse("12345"), PlaylistId.Parse("100"), "playlistName"));
            stream.Add(new PlaylistDeleted(UserId.Parse("12345"), PlaylistId.Parse("100"), "playlistName"));

            var publisher = new EventBus(stream);

            var user = new User(stream, publisher);

            user.DeletePlaylist(PlaylistId.Parse("100"), "playlistName");

            Assert.IsTrue(stream.GetEvents().Contains(new PlaylistDeleted(UserId.Parse("12345"), PlaylistId.Parse("100"), "playlistName")));
            Assert.AreEqual(1, stream.GetEvents().OfType<PlaylistDeleted>().Count());
        }

        [Test]
        public void NoRaiseMessageWhenDeletePlaylistWithDeletedUser()
        {
            var stream = new MemoryEventStream();
            stream.Add(new UserCreated(Identity.Parse("nicolas.dfr@gmail.com", "dublow", "12345", "accessToken")));
            stream.Add(new PlaylistAdded(UserId.Parse("12345"), PlaylistId.Parse("100"), "playlistName"));
            stream.Add(new UserDeleted(UserId.Parse("12345")));

            var publisher = new EventBus(stream);

            var user = new User(stream, publisher);

            user.DeletePlaylist(PlaylistId.Parse("100"), "playlistName");

            Assert.AreEqual(0, stream.GetEvents().OfType<PlaylistDeleted>().Count());
        }

        [Test]
        public void RaiseMessageWhenAddSongToPlaylist()
        {
            var stream = new MemoryEventStream();
            stream.Add(new UserCreated(Identity.Parse("nicolas.dfr@gmail.com", "dublow", "12345", "accessToken")));
            stream.Add(new PlaylistAdded(UserId.Parse("12345"), PlaylistId.Parse("100"), "playlistName"));

            var publisher = new EventBus(stream);

            var user = new User(stream, publisher);

            user.AddSongToPlaylist(SongId.Parse("101"), "title", "artist");

            Assert.IsTrue(stream.GetEvents().Contains(new SongAdded(UserId.Parse("12345"), PlaylistId.Parse("100"), SongId.Parse("101"), "title", "artist")));
            Assert.AreEqual(1, stream.GetEvents().OfType<SongAdded>().Count());
        }

        [Test]
        public void NoRaiseMessageWhenAddSongAlreadyAddedToPlaylist()
        {
            var stream = new MemoryEventStream();
            stream.Add(new UserCreated(Identity.Parse("nicolas.dfr@gmail.com", "dublow", "12345", "accessToken")));
            stream.Add(new PlaylistAdded(UserId.Parse("12345"), PlaylistId.Parse("100"), "playlistName"));
            stream.Add(new SongAdded(UserId.Parse("12345"), PlaylistId.Parse("100"), SongId.Parse("101"), "title", "artist"));

            var publisher = new EventBus(stream);

            var user = new User(stream, publisher);

            user.AddSongToPlaylist(SongId.Parse("101"), "title", "artist");

            Assert.IsTrue(stream.GetEvents().Contains(new SongAdded(UserId.Parse("12345"), PlaylistId.Parse("100"), SongId.Parse("101"), "title", "artist")));
            Assert.AreEqual(1, stream.GetEvents().OfType<SongAdded>().Count());
        }

        [Test]
        public void NoRaiseMessageWhenAddSongWithDeletedUser()
        {
            var stream = new MemoryEventStream();
            stream.Add(new UserCreated(Identity.Parse("nicolas.dfr@gmail.com", "dublow", "12345", "accessToken")));
            stream.Add(new PlaylistAdded(UserId.Parse("12345"), PlaylistId.Parse("100"), "playlistName"));
            stream.Add(new UserDeleted(UserId.Parse("12345")));

            var publisher = new EventBus(stream);

            var user = new User(stream, publisher);

            user.AddSongToPlaylist(SongId.Parse("101"), "title", "artist");

            Assert.AreEqual(0, stream.GetEvents().OfType<SongAdded>().Count());
        }
    }
}
