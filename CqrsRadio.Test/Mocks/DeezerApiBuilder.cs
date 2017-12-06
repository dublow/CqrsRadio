using System.Collections.Generic;
using CqrsRadio.Domain.Entities;
using CqrsRadio.Domain.Services;
using CqrsRadio.Domain.ValueTypes;
using Moq;

namespace CqrsRadio.Test.Mocks
{
    public class DeezerApiBuilder
    {
        private readonly Mock<IDeezerApi> _mock;
        public int PlaylistAdded { get; private set; }
        public int PlaylistDeleted { get; private set; }
        public int SongsAdded { get; private set; }

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
                .Callback(() => PlaylistAdded++);

            _mock.Setup(x => x.DeletePlaylist(It.IsAny<string>()))
                .Callback(() => PlaylistDeleted++);

            _mock.Setup(x => x.AddSongsToPlaylist(It.IsAny<string>(), It.IsAny<string[]>()))
                .Callback<string, string[]>((playlistName, songs) => SongsAdded = songs.Length);

            return _mock.Object;
        }

        public DeezerApiBuilder SetSong(DeezerSong deezerSong)
        {
            _mock.Setup(x => x.GetSong(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(deezerSong);

            _mock.Setup(x => x.GetSong(It.IsAny<string>()))
                .Returns(deezerSong);
            return this;
        }

        public DeezerApiBuilder SetSongsByPlaylistId(IEnumerable<DeezerSong> deezerSongs)
        {
            _mock.Setup(x => x.GetSongsByPlaylistId(It.IsAny<string>()))
                .Returns(deezerSongs);
            return this;
        }
    }
}
