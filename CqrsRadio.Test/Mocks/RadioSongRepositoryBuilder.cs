using System.Collections.Generic;
using CqrsRadio.Domain.Repositories;
using CqrsRadio.Domain.ValueTypes;
using Moq;

namespace CqrsRadio.Test.Mocks
{
    public class RadioSongRepositoryBuilder     
    {
        private readonly Mock<IRadioSongRepository> _mock;
        public readonly List<(SongId songId, string genre, string name, string title, string artist)> RadioSongs;
        public readonly List<(string name, string title, string artist)> RadioSongDuplicate;
        public readonly List<(string name, string error)> RadioSongErrors;

        public RadioSongRepositoryBuilder()
        {
            _mock = new Mock<IRadioSongRepository>();
            RadioSongs = new List<(SongId songId, string genre, string name, string title, string artist)>();
            RadioSongDuplicate = new List<(string name, string title, string artist)>();
            RadioSongErrors = new List<(string name, string error)>();
        }

        public static RadioSongRepositoryBuilder Create()
        {
            return new RadioSongRepositoryBuilder();
        }

        public IRadioSongRepository Build()
        {
            _mock.Setup(x => x.Add(It.IsAny<SongId>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback<SongId, string, string, string, string>((songId, genre, name, title, artist) => RadioSongs.Add((songId, genre, name, title, artist)));

            _mock.Setup(x => x.AddToDuplicate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string, string>((name, title, artist) => RadioSongDuplicate.Add((name, title, artist)));

            _mock.Setup(x => x.AddToError(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((name, error) => RadioSongErrors.Add((name, error)));

            return _mock.Object;
        }

        
    }
}