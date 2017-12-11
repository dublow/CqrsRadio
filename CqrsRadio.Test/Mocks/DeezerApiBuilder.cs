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
            _mock.Setup(x => x.DeletePlaylist(It.IsAny<string>(), It.IsAny<PlaylistId>()))
                .Callback(() => PlaylistDeleted++);

            _mock.Setup(x => x.AddSongsToPlaylist(It.IsAny<string>(), It.IsAny<PlaylistId>(), It.IsAny<SongId[]>()))
                .Callback<string, PlaylistId, SongId[]>((playlistName, playlistId, songs) => SongsAdded = songs.Length);

            return _mock.Object;
        }

        public DeezerApiBuilder SetCreatePlaylist(PlaylistId playlistId)
        {
            _mock.Setup(x => x.CreatePlaylist(It.IsAny<string>(), It.IsAny<UserId>(), It.IsAny<string>()))
                .Returns(playlistId);

            return this;
        }

        public DeezerApiBuilder SetSong(DeezerSong deezerSong)
        {
            _mock.Setup(x => x.GetSong(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(deezerSong);

            _mock.Setup(x => x.GetSong(It.IsAny<string>(), It.IsAny<SongId>()))
                .Returns(deezerSong);
            return this;
        }

        public DeezerApiBuilder SetSongsByPlaylistId(IEnumerable<DeezerSong> deezerSongs)
        {
            _mock.Setup(x => x.GetSongsByPlaylistId(It.IsAny<string>(), It.IsAny<PlaylistId>()))
                .Returns(deezerSongs);
            return this;
        }

        public DeezerApiBuilder SetPlaylistIdsByUserId(PlaylistId[] playlistIds)
        {
            _mock.Setup(x => x.GetPlaylistIdsByUserId(It.IsAny<string>(), It.IsAny<UserId>()))
                .Returns(playlistIds);

            return this;
        }
    }
}
