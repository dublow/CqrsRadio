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
                                 "radiosong (songid int primary key, radioname varchar(20), title varchar(50), artist varchar(50))";

            ExecuteCommand(connectionString, radioSongTable);
        }

        private static void ExecuteCommand(string connectionString, string commandText)
        {
            using (var cnx = new SQLiteConnection(connectionString))
            {
                cnx.Open();
                new SQLiteCommand(commandText, cnx)
                    .ExecuteNonQuery();
            }
        }
    }
}
