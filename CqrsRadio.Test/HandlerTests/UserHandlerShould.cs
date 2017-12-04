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
    }

    public class UserHandler : IUserHandler
    {
        public UserHandler(IUserRepository userRepository)
        {
            throw new NotImplementedException();
        }

        public void Handle(UserCreated evt)
        {
            throw new NotImplementedException();
        }

        public void Handle(UserDeleted evt)
        {
            throw new NotImplementedException();
        }
    }

    public interface IUserRepository    
    {
    }

    public class UserRepositoryBuilder  
    {
        public static UserRepositoryBuilder Create()
        {
            throw new NotImplementedException();
        }

        public IUserRepository Build()
        {
            throw new NotImplementedException();
        }

        public List<(string email, string nickname, string userId)> Users { get; set; }
    }
}
