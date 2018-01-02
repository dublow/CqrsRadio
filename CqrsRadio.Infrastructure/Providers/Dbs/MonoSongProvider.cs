using System.Data.Common;
using Mono.Data.Sqlite;

namespace CqrsRadio.Infrastructure.Providers.Dbs
{
    public class MonoSongProvider : IProvider
    {
        public string Dbname => "monosong.sqlite";

        public DbConnection Create()
        {
            return new SqliteConnection($"URI=file:{Dbname}");
        }

        public void CreateFile(string filename)
        {
            SqliteConnection.CreateFile(filename);
        }
    }
}
