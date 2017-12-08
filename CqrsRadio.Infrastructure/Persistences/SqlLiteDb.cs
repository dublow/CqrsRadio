using System.Data.SQLite;
using System.IO;

namespace CqrsRadio.Infrastructure.Persistences
{
    public static class SqlLiteDb 
    {
        public static void CreateDomain()
        {
            var dbname = "cqrsradio";
            var connectionString = $"Data Source={dbname}.sqlite;Version=3;";

            if (!File.Exists($"{dbname}.sqlite"))
            {
                SQLiteConnection.CreateFile($"{dbname}.sqlite");
            }

            var radioSongTable = "create table if not exists " +
                                 "nd.radiosong (songid int, radioname varchar(20), title varchar(100), artist varchar(100) " +
                                 "constraint fksongid primary key (songid)";


            ExecuteCommand(connectionString, radioSongTable);
        }

        public static void ExecuteCommand(string connectionString, string commandText)
        {
            using (var cnx = new SQLiteConnection(connectionString))
            {
                new SQLiteCommand(commandText, cnx)
                    .ExecuteNonQuery();

            }
        }
    }
}
