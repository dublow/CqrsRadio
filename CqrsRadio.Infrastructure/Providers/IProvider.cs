using System.Data.Common;

namespace CqrsRadio.Infrastructure.Providers
{
    public interface IProvider
    {
        string DbName { get; }
        DbConnection Create();
        void CreateDb(string dbname);
    }
}
