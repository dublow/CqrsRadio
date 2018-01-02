using System.Data.Common;
using System.Data.SQLite;

namespace CqrsRadio.Infrastructure.Providers.Dbs
{
    public class SongProvider : IProvider
    {
        public string Dbname => "song.sqlite";

        public DbConnection Create()
        {
            return new SQLiteConnection($"Data Source={Dbname};Version=3;");
        }

        public void CreateFile(string filename)
        {
            SQLiteConnection.CreateFile(filename);
        }
    }
}
