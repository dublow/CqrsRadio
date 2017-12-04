using System;
using System.Linq;
using CqrsRadio.Domain.Events;
using CqrsRadio.Handlers;
using CqrsRadio.Test.Mocks;
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
}
