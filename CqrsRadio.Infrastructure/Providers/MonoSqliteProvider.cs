using System.Data.Common;
using Mono.Data.Sqlite;

namespace CqrsRadio.Infrastructure.Providers
{
    public class MonoSqliteProvider : IProvider
    {
        public DbConnection Create()
        {
            return new SqliteConnection("URI=file:cqrsradio.sqlite");
        }
    }
}
