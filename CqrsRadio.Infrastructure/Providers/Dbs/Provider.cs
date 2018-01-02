using System.Data.Common;
using System.Data.SQLite;

namespace CqrsRadio.Infrastructure.Providers.Dbs
{
    public class Provider : IProvider
    {
        public string DbName { get; }
        public Provider(string dbname)
        {
            DbName = dbname;
        }

        public DbConnection Create()
        {
            return new SQLiteConnection($"Data Source={DbName};Version=3;");
        }

        public void CreateDb(string dbname)
        {
            SQLiteConnection.CreateFile(dbname);
        }
    }
}
