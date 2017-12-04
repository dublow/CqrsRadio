using System;
using System.Collections.Generic;
using CqrsRadio.Domain.Repositories;
using Moq;

namespace CqrsRadio.Test.Mocks
{
    public class RadioRepositoryBuilder     
    {
        private readonly Mock<IRadioRepository> _mock;
        public readonly List<(string name, Uri url)> Radios;

        public RadioRepositoryBuilder()
        {
            _mock = new Mock<IRadioRepository>();
            Radios = new List<(string name, Uri url)>();
        }

        public static RadioRepositoryBuilder Create()
        {
            return new RadioRepositoryBuilder();
        }

        public IRadioRepository Build()
        {
            _mock.Setup(x => x.Create(It.IsAny<string>(), It.IsAny<Uri>()))
                .Callback<string, Uri>((name, url) => Radios.Add((name, url)));

            return _mock.Object;
        }
    }
}