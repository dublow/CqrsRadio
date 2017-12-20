using System.Data.Common;
using System.Data.SQLite;
using Mono.Data.Sqlite;

namespace CqrsRadio.Infrastructure.Providers
{
    public class SqliteProvider : IProvider
    {
        public DbConnection Create()
        {
            return new SQLiteConnection("Data Source=cqrsradio.sqlite;Version=3;");
        }
    }

    public class MonoSqliteProvider : IProvider
    {
        public DbConnection Create()
        {
            return new SqliteConnection("URI=file:cqrsradio.sqlite");
        }
    }
}
