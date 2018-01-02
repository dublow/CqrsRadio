using System.Data;
using System.IO;
using System.Threading.Tasks;
using CqrsRadio.Infrastructure.Providers;

namespace CqrsRadio.Infrastructure.Persistences
{
    public class DatabaseDomain : IDatabase
    {
        private readonly IProvider _provider;

        public DatabaseDomain(IProvider provider)
        {
            _provider = provider;
        }

        public void Create()
        {
            if (!File.Exists(_provider.Dbname))
            {
                _provider.CreateFile(_provider.Dbname);
            }

            var playlistSongTable = "create table if not exists " +
                                    "playlist (userid int primary key, createdAt varchar(20))";

            var adminTable = "create table if not exists " +
                             "admin (login varchar(20) primary key, hash blob)";

            ExecuteCommand(playlistSongTable);
            ExecuteCommand(adminTable);
        }

        public void Restore()
        {
            
        }

        private void ExecuteCommand(string commandText)
        {
            using (var cnx = _provider.Create())
            {
                cnx.Open();
                
                using (var command = cnx.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = commandText;
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
