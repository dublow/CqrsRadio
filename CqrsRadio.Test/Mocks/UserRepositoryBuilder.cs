using System;
using System.Collections.Generic;
using System.Linq;
using CqrsRadio.Domain.Repositories;
using Moq;

namespace CqrsRadio.Test.Mocks
{
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

            _mock.Setup(x => x.Delete(It.IsAny<string>()))
                .Callback<string>(userId =>
                {
                    var user = Users.First(x => x.userId.Equals(userId, StringComparison.InvariantCultureIgnoreCase));
                    Users.Remove(user);
                });

            return _mock.Object;
        }

        public List<(string email, string nickname, string userId)> Users { get; set; }
    }
}