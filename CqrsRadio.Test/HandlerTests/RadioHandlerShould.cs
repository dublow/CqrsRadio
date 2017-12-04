using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CqrsRadio.Domain.EventHandlers;
using CqrsRadio.Domain.Events;
using CqrsRadio.Domain.ValueTypes;
using Moq;
using NUnit.Framework;

namespace CqrsRadio.Test.HandlerTests
{
    [TestFixture]
    public class RadioHandlerShould
    {
        [Test]
        public void UseRepositoryWhenRadioIsCreated()
        {
            // arrange
            var mockedRadioRepository = RadioRepositoryBuilder.Create();
            var radioRepository = mockedRadioRepository.Build();
            var radioHandler = new RadioHandler(radioRepository);
            (string name, Uri url) = ("djam", new Uri("http://djam.fr"));
            // act
            radioHandler.Handle(new RadioCreated("djam", new Uri("http://djam.fr")));
            // assert
            var (actualName, actualUrl) = mockedRadioRepository.Radios.First();
            Assert.AreEqual(name, actualName);
            Assert.AreEqual(url, actualUrl);
        }
    }

    public class RadioHandler : IRadioHandler
    {
        private readonly IRadioRepository _radioRepository;

        public RadioHandler(IRadioRepository radioRepository)
        {
            _radioRepository = radioRepository;
        }

        public void Handle(RadioCreated evt)
        {
            _radioRepository.Create(evt.Name, evt.Url);
        }

        public void Handle(RadioDeleted evt)
        {
            throw new NotImplementedException();
        }
    }

    public interface IRadioRepository   
    {
        void Create(string name, Uri url);
    }

    public class RadioRepositoryBuilder     
    {
        private Mock<IRadioRepository> _mock;

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

        public List<(string name, Uri url)> Radios { get; set; }
    }
}
