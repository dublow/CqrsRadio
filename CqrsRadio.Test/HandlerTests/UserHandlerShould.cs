using System.Linq;
using CqrsRadio.Domain.Events;
using CqrsRadio.Domain.ValueTypes;
using CqrsRadio.Handlers;
using CqrsRadio.Test.Mocks;
using NUnit.Framework;

namespace CqrsRadio.Test.HandlerTests
{
    [TestFixture]
    public class UserHandlerShould
    {
        [Test]
        public void UseRepositoryWhenUserIsCreated()
        {
            // arrange
            var mockedUserRepository = UserRepositoryBuilder.Create();
            var userRepository = mockedUserRepository.Build();
            var userHandler = new UserHandler(userRepository);
            var (email, nickname, userId) = ("email@email.fr", "nickname", "12345");
            // act
            userHandler.Handle(new UserCreated(Identity.Create("email@email.fr", "nickname", "12345")));
            // assert
            var (actualEmail, actualNickname, actualUserId) = mockedUserRepository.Users.First();
            Assert.AreEqual(email, actualEmail);
            Assert.AreEqual(nickname, actualNickname);
            Assert.AreEqual(userId, actualUserId);
        }

        [Test]
        public void UseRepositoryWhenUserIsDeleted()
        {
            // arrange
            var mockedUserRepository = UserRepositoryBuilder.Create();
            mockedUserRepository.Users.Add(("email@email.fr", "nickname", "12345"));
            var userRepository = mockedUserRepository.Build();
            var userHandler = new UserHandler(userRepository);
            // act
            userHandler.Handle(new UserDeleted("12345"));
            // assert
            
            Assert.AreEqual(0, mockedUserRepository.Users.Count);
        }
    }
}
