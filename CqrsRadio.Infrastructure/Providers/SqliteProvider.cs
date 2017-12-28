using System.Data.Common;
using System.Data.SQLite;

namespace CqrsRadio.Infrastructure.Providers
{
    public class SqliteProvider : IProvider
    {
        public DbConnection Create()
        {
            return new SQLiteConnection("Data Source=cqrsradio.sqlite;Version=3;");
        }
    }
}
