using CqrsRadio.Domain.ValueTypes;

namespace CqrsRadio.Domain.Repositories
{
    public interface IUserRepository    
    {
        void Create(Email email, Nickname nickname, UserId userId);
        void Delete(UserId userId);
    }
}