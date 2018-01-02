using System.Data.Common;
using System.Data.SQLite;

namespace CqrsRadio.Infrastructure.Providers.Dbs
{
    public class DomainProvider : IProvider
    {
        public string Dbname => "domain.sqlite";

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
