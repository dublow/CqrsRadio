using System;

namespace CqrsRadio.Domain.Repositories
{
    public interface IAdminRepository : IRepository
    {
        void Add(string login, byte[] salt, byte[] hash);
    }
}