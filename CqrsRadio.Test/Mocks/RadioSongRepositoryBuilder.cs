using System.Collections.Generic;
using CqrsRadio.Domain.Repositories;
using Moq;

namespace CqrsRadio.Test.Mocks
{
    public class RadioSongRepositoryBuilder     
    {
        private readonly Mock<IRadioSongRepository> _mock;
        public readonly List<(string name, string title, string artist)> RadioSongs;
        public readonly List<(string name, string title, string artist)> RadioSongDuplicate;
        public readonly List<(string name, string error)> RadioSongErrors;

        public RadioSongRepositoryBuilder()
        {
            _mock = new Mock<IRadioSongRepository>();
            RadioSongs = new List<(string name, string title, string artist)>();
            RadioSongDuplicate = new List<(string name, string title, string artist)>();
        }

        public static RadioSongRepositoryBuilder Create()
        {
            return new RadioSongRepositoryBuilder();
        }

        public IRadioSongRepository Build()
        {
            _mock.Setup(x => x.Add(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string, string>((name, title, artist) => RadioSongs.Add((name, title, artist)));

            _mock.Setup(x => x.AddToDuplicate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string, string>((name, title, artist) => RadioSongDuplicate.Add((name, title, artist)));

            return _mock.Object;
        }

        
    }
}