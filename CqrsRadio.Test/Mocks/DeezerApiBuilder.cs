using CqrsRadio.Domain.Services;
using CqrsRadio.Domain.ValueTypes;
using Moq;

namespace CqrsRadio.Test.Mocks
{
    public class DeezerApiBuilder
    {
        private readonly Mock<IDeezerApi> _mock;
        public bool PlaylistAdded { get; private set; }
        public bool PlaylistDeleted { get; private set; }
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
            _mock.Setup(x => x.CreatePlaylist(It.IsAny<UserId>(), It.IsAny<string>()))
                .Callback(() => PlaylistAdded = true);

            _mock.Setup(x => x.DeletePlaylist(It.IsAny<string>()))
                .Callback(() => PlaylistDeleted = true);

            _mock.Setup(x => x.AddSongsToPlaylist(It.IsAny<string>(), It.IsAny<string[]>()))
                .Callback(() => SongsAdded = true);

            return _mock.Object;
        }
    }
}
