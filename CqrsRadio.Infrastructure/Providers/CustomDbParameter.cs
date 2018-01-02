using System.Data.Common;
using System.Data.SQLite;

namespace CqrsRadio.Infrastructure.Providers
{
    public class CustomDbParameter : IDbParameter
    {
        public DbParameter Create(string name, object value)
        {
            return new SQLiteParameter(name, value);
        }
    }
}
