using System.Linq;
using CqrsRadio.Domain.Aggregates;
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

            User.Create(stream, publisher, deezerApi, "nicolas.dfr@gmail.com", "dublow", "12345", "accessToken");

            var identity = Identity.Parse("nicolas.dfr@gmail.com", "dublow", "12345", "accessToken");
            Assert.IsTrue(stream.GetEvents().Contains(new UserCreated(identity)));
        }

        [Test]
        public void RaiseMessageWhenDeleteUser()
        {
            var stream = new MemoryEventStream();
            var deezerApi = DeezerApiBuilder.Create().Build();
            stream.Add(new UserCreated(Identity.Parse("nicolas.dfr@gmail.com", "dublow", "12345", "accessToken")));
            
            var publisher = new EventBus(stream);

            var user = new User(stream, publisher, deezerApi);

            user.Delete();
            
            Assert.IsTrue(stream.GetEvents().Contains(new UserDeleted(UserId.Parse("12345"))));
        }

        [Test]
        public void NoRaiseMessageWhenDeleteDeletedUser()
        {
            var stream = new MemoryEventStream();
            var deezerApi = DeezerApiBuilder.Create().Build();
            stream.Add(new UserCreated(Identity.Parse("nicolas.dfr@gmail.com", "dublow", "12345", "accessToken")));
            stream.Add(new UserDeleted(UserId.Parse("12345")));

            var publisher = new EventBus(stream);

            var user = new User(stream, publisher, deezerApi);

            user.Delete();

            Assert.IsTrue(stream.GetEvents().Contains(new UserDeleted(UserId.Parse("12345"))));
        }

        [Test]
        public void NoRaiseMessageWhenTwiceDeleteUser()
        {
            var stream = new MemoryEventStream();
            var deezerApi = DeezerApiBuilder.Create().Build();
            stream.Add(new UserCreated(Identity.Parse("nicolas.dfr@gmail.com", "dublow", "12345", "accessToken")));
            
            var publisher = new EventBus(stream);

            var user = new User(stream, publisher, deezerApi);

            user.Delete();
            user.Delete();

            Assert.IsTrue(stream.GetEvents().Contains(new UserDeleted(UserId.Parse("12345"))));
        }

        [Test]
        public void RaiseMessageWhenAddPlaylistToUser()
        {
            var stream = new MemoryEventStream();
            var deezerApi = DeezerApiBuilder.Create().SetCreatePlaylist(PlaylistId.Parse("100")).Build();
            stream.Add(new UserCreated(Identity.Parse("nicolas.dfr@gmail.com", "dublow", "12345", "accessToken")));

            var publisher = new EventBus(stream);

            var user = new User(stream, publisher, deezerApi);

            user.AddPlaylist("playlistName");

            Assert.IsTrue(stream.GetEvents().Contains(new PlaylistAdded(UserId.Parse("12345"), PlaylistId.Parse("100"), "playlistName")));
        }

        [Test]
        public void NoRaiseMessageWhenAddExistingPlaylistToUser()
        {
            var stream = new MemoryEventStream();
            var deezerApi = DeezerApiBuilder.Create().Build();
            stream.Add(new UserCreated(Identity.Parse("nicolas.dfr@gmail.com", "dublow", "12345", "accessToken")));
            stream.Add(new PlaylistAdded(UserId.Parse("12345"), PlaylistId.Parse("100"), "playlistName"));

            var publisher = new EventBus(stream);

            var user = new User(stream, publisher, deezerApi);

            user.AddPlaylist("playlistName");

            Assert.IsTrue(stream.GetEvents().Contains(new PlaylistAdded(UserId.Parse("12345"), PlaylistId.Parse("100"), "playlistName")));
            Assert.AreEqual(1, stream.GetEvents().OfType<PlaylistAdded>().Count());
        }

        [Test]
        public void NoRaiseMessageWhenAddPlaylistWithDeletedUser()
        {
            var stream = new MemoryEventStream();
            var deezerApi = DeezerApiBuilder.Create().Build();
            stream.Add(new UserCreated(Identity.Parse("nicolas.dfr@gmail.com", "dublow", "12345", "accessToken")));
            stream.Add(new UserDeleted(UserId.Parse("12345")));

            var publisher = new EventBus(stream);

            var user = new User(stream, publisher, deezerApi);

            user.AddPlaylist("playlistName");

            Assert.AreEqual(0, stream.GetEvents().OfType<PlaylistAdded>().Count());
        }
    }
}
