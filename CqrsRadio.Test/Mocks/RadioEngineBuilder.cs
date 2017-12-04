using System;
using CqrsRadio.Domain.Entities;
using CqrsRadio.Domain.Services;
using Moq;

namespace CqrsRadio.Test.Mocks
{
    public class RadioEngineBuilder
    {
        private readonly Mock<IRadioEngine> _mock;

        private RadioEngineBuilder()
        {
            _mock = new Mock<IRadioEngine>();
        }

        public static RadioEngineBuilder Create()
            => new RadioEngineBuilder();

        public RadioEngineBuilder SetParser(string title, string artist)
        {
            _mock.Setup(x => x.Parse(It.IsAny<Uri>())).Returns(RadioSong.Create(title, artist));
            return this;
        }

        public IRadioEngine Build()
        {
            return _mock.Object;
        }

        public RadioEngineBuilder SetException(Exception exception)
        {
            _mock.Setup(x => x.Parse(It.IsAny<Uri>())).Throws(exception);
            return this;
        }
    }
}
