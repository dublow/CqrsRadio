using System;
using System.Data;
using System.Linq;
using CqrsRadio.Domain.Repositories;
using CqrsRadio.Infrastructure.Providers;

namespace CqrsRadio.Infrastructure.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly IProvider _provider;
        private readonly IDbParameter _dbParameter;
        public AdminRepository(IProvider provider, IDbParameter dbParameter)
        {
            _provider = provider;
            _dbParameter = dbParameter;
        }
        public void Add(string login, byte[] hash)
        {
            using (var cnx = _provider.Create())
            {
                var commandText = "insert into admin (login, hash) values(@login, @hash)";
                cnx.Open();
                using (var command = cnx.CreateCommand())
                {
                    command.CommandText = commandText;
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add(_dbParameter.Create("@login", login));
                    command.Parameters.Add(_dbParameter.Create("@hash", hash));
                    command.ExecuteNonQuery();
                }
            }
        }

        public bool Exists(string login)
        {
            using (var cnx = _provider.Create())
            {
                var commandText = "select login from admin where login = @login";
                cnx.Open();
                using (var command = cnx.CreateCommand())
                {
                    command.CommandText = commandText;
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add(_dbParameter.Create("@login", login));
                    using (var reader = command.ExecuteReader())
                    {
                        return reader.Read();
                    }
                }
            }
        }

        public byte[] GetPassword(string login)
        {
            using (var cnx = _provider.Create())
            {
                var commandText = "select hash from admin where login = @login";
                cnx.Open();
                using (var command = cnx.CreateCommand())
                {
                    command.CommandText = commandText;
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add(_dbParameter.Create("@login", login));
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            return (byte[])reader[0];
                        }

                        return Enumerable.Empty<byte>().ToArray();
                    }
                }
            }
        }
    }
}
