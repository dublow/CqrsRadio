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
        private readonly IUserRepository _userRepository;

        public UserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void Handle(UserCreated evt)
        {
            _userRepository.Create(evt.Identity.Email.Value, evt.Identity.Nickname.Value,
                evt.Identity.UserId.Value.ToString());
        }

        public void Handle(UserDeleted evt)
        {
            throw new NotImplementedException();
        }
    }

    public interface IUserRepository    
    {
        void Create(string email, string nickname, string userId);
    }

    public class UserRepositoryBuilder  
    {
        private readonly Mock<IUserRepository> _mock;

        public UserRepositoryBuilder()
        {
            _mock = new Mock<IUserRepository>();
            Users = new List<(string email, string nickname, string userId)>();
        }
        public static UserRepositoryBuilder Create()
        {
            return new UserRepositoryBuilder();
        }

        public IUserRepository Build()
        {
            _mock.Setup(x => x.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string, string>((email, nickname, userId) => Users.Add((email, nickname, userId)));

            return _mock.Object;
        }

        public List<(string email, string nickname, string userId)> Users { get; set; }
    }
}
