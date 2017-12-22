using CqrsRadio.Domain.Repositories;
using CqrsRadio.Infrastructure.Providers;

namespace CqrsRadio.Infrastructure.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly IProvider _provider;
        public AdminRepository(IProvider provider)
        {
            _provider = provider;
        }
        public void Add(string login, byte[] salt, byte[] hash)
        {
            using (var cnx = _provider.Create())
            {
                var commandText = $"insert into admin (login, salt, hash) values('{login}', '{salt}', '{hash}')";
                cnx.Open();
                using (var command = cnx.CreateCommand())
                {
                    command.CommandText = commandText;
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
