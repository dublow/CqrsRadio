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
            var email = "email@email.fr";
            var nickName = "nickname";
            var userId = "12345";
            var accessToken = "accessToken";

            var mockedUserRepository = UserRepositoryBuilder.Create();
            var userRepository = mockedUserRepository.Build();
            var userHandler = new UserHandler(userRepository);
            // act
            var identity = Identity.Parse(email, nickName, userId, accessToken);
            userHandler.Handle(new UserCreated(identity));
            // assert
            var (actualEmail, actualNickname, actualUserId) = mockedUserRepository.Users.First();
            Assert.AreEqual(Email.Parse(email), actualEmail);
            Assert.AreEqual(Nickname.Parse(nickName), actualNickname);
            Assert.AreEqual(UserId.Parse(userId), actualUserId);
        }

        [Test]
        public void UseRepositoryWhenUserIsDeleted()
        {
            // arrange
            var email = Email.Parse("email@email.fr");
            var nickName = Nickname.Parse("nickname");
            var userId = UserId.Parse("12345");

            var mockedUserRepository = UserRepositoryBuilder.Create();
            mockedUserRepository.Users.Add((email, nickName, userId));
            var userRepository = mockedUserRepository.Build();
            var userHandler = new UserHandler(userRepository);
            // act
            userHandler.Handle(new UserDeleted(userId));
            // assert
            Assert.AreEqual(0, mockedUserRepository.Users.Count);
        }
    }
}
