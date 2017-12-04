using CqrsRadio.Domain.Services;
using CqrsRadio.Domain.ValueTypes;
using Moq;

namespace CqrsRadio.Test.Mocks
{
    public class DeezerApiBuilder
    {
        private readonly Mock<IDeezerApi> _mock;
        public string PlaylistAdded { get; private set; }

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
    }
}
