using System.Collections.Generic;
using System.Linq;
using CqrsRadio.Domain.Repositories;
using CqrsRadio.Domain.ValueTypes;
using Moq;

namespace CqrsRadio.Test.Mocks
{
    public class UserRepositoryBuilder  
    {
        private readonly Mock<IUserRepository> _mock;

        public UserRepositoryBuilder()
        {
            _mock = new Mock<IUserRepository>();
            Users = new List<(Email email, Nickname nickname, UserId userId)>();
        }
        public static UserRepositoryBuilder Create()
        {
            return new UserRepositoryBuilder();
        }

        public IUserRepository Build()
        {
            _mock.Setup(x => x.Create(It.IsAny<Email>(), It.IsAny<Nickname>(), It.IsAny<UserId>()))
                .Callback<Email, Nickname, UserId>((email, nickname, userId) => Users.Add((email, nickname, userId)));

            _mock.Setup(x => x.Delete(It.IsAny<UserId>()))
                .Callback<UserId>(userId =>
                {
                    var user = Users.First(x => x.userId == userId);
                    Users.Remove(user);
                });

            return _mock.Object;
        }

        public List<(Email email, Nickname nickname, UserId userId)> Users { get; set; }
    }
}