using System.Collections.Generic;
using CqrsRadio.Domain.Repositories;
using Moq;

namespace CqrsRadio.Test.Mocks
{
    public class RadioSongRepositoryBuilder     
    {
        private Mock<IRadioSongRepository> _mock;

        public RadioSongRepositoryBuilder()
        {
            _mock = new Mock<IRadioSongRepository>();
            RadioSongs = new List<(string name, string title, string artist)>();
        }

        public static RadioSongRepositoryBuilder Create()
        {
            return new RadioSongRepositoryBuilder();
        }

        public IRadioSongRepository Build()
        {
            _mock.Setup(x => x.Add(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string, string>((name, title, artist) => RadioSongs.Add((name, title, artist)));

            return _mock.Object;
        }

        public List<(string name, string title, string artist)> RadioSongs { get; set; }
    }
}