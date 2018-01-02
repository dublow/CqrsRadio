using System.Data.Common;
using Mono.Data.Sqlite;

namespace CqrsRadio.Infrastructure.Providers.Dbs
{
    public class MonoDomainProvider : IProvider
    {
        public string Dbname => "monodomain.sqlite";

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
