using CqrsRadio.Domain.Services;
using CqrsRadio.Domain.ValueTypes;
using Moq;

namespace CqrsRadio.Test.Mocks
{
    public class DeezerApiBuilder
    {
        private readonly Mock<IDeezerApi> _mock;
        public string PlaylistAdded { get; private set; }
        public string PlaylistDeleted { get; private set; }
        public bool SongsAdded { get; private set; }

        public DeezerApiBuilder()
        {
            _mock = new Mock<IDeezerApi>();
        }

        public static DeezerApiBuilder Create()
        {
            return new DeezerApiBuilder();
        }

        public IDeezerApi Build()
        {
            return _mock.Object;
        }

        public DeezerApiBuilder SetCreatePlaylist(string name)
        {
            _mock.Setup(x => x.CreatePlaylist(It.IsAny<UserId>(), It.IsAny<string>()))
                .Callback(() => PlaylistAdded = $"{name}-added");

            return this;
        }

        public DeezerApiBuilder SetDeletePlaylist(string playlistId)
        {
            _mock.Setup(x => x.DeletePlaylist(It.IsAny<string>()))
                .Callback(() => PlaylistDeleted = $"{playlistId}-deleted");

            return this;
        }

        public DeezerApiBuilder SetAddSongsToPlaylist()
        {
            _mock.Setup(x => x.AddSongsToPlaylist(It.IsAny<string>(), It.IsAny<string[]>()))
                .Callback(() => SongsAdded = true);

            return this;
        }
    }
}
