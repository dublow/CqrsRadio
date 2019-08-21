using System.Data;
using System.IO;
using Mono.Data.Sqlite;

namespace CqrsRadio.Infrastructure.Providers.Dbs
{
    public class Domain
    {
        public void CreateSong(IProvider provider)
        {
            var songdbname = "monosong.sqlite";
            if (!File.Exists(songdbname))
            {
                SqliteConnection.CreateFile(songdbname);
                ExecuteCommand(Tables.RadioSong, provider);
            }
        }

        public void CreateDomain(IProvider provider)
        {
            var domaindbname = "monodomain.sqlite";
            if (!File.Exists(domaindbname))
            {
                SqliteConnection.CreateFile(domaindbname);
                ExecuteCommand(Tables.Admin, provider);
                ExecuteCommand(Tables.Playlist, provider);
            }
        }

        private void ExecuteCommand(string commandText, IProvider provider)
        {
            using (var cnx = provider.Create())
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