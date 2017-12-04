namespace CqrsRadio.Domain.Repositories
{
    public interface IUserRepository    
    {
        void Create(string email, string nickname, string userId);
    }
}