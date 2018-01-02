using System.Data.Common;
using Mono.Data.Sqlite;

namespace CqrsRadio.Infrastructure.Providers.Dbs
{
    public class MonoProvider : IProvider
    {
        public string DbName { get; }
        public MonoProvider(string dbname)
        {
            DbName = dbname;
        }

        public DbConnection Create()
        {
            return new SqliteConnection($"URI=file:{DbName}");
        }

        public void CreateDb(string dbname)
        {
            SqliteConnection.CreateFile(dbname);
        }
    }
}
