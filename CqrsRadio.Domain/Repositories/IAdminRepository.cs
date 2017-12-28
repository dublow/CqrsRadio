namespace CqrsRadio.Domain.Repositories
{
    public interface IAdminRepository : IRepository
    {
        void Add(string login, byte[] hash);
        bool Exists(string login);
        byte[] GetPassword(string login);
    }
}