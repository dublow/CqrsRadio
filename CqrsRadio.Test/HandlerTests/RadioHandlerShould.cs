using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CqrsRadio.Domain.EventHandlers;
using CqrsRadio.Domain.Events;
using CqrsRadio.Domain.ValueTypes;
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
        public RadioHandler(IRadioRepository radioRepository)
        {
            throw new NotImplementedException();
        }

        public void Handle(RadioCreated evt)
        {
            throw new NotImplementedException();
        }

        public void Handle(RadioDeleted evt)
        {
            throw new NotImplementedException();
        }
    }

    public interface IRadioRepository   
    {
    }

    public class RadioRepositoryBuilder     
    {
        public static RadioRepositoryBuilder Create()
        {
            throw new NotImplementedException();
        }

        public IRadioRepository Build()
        {
            throw new NotImplementedException();
        }

        public List<(string name, Uri url)> Radios { get; set; }
    }
}
