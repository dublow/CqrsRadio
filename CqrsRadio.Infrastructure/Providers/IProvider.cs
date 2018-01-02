using System.Data.Common;

namespace CqrsRadio.Infrastructure.Providers
{
    public interface IProvider
    {
        string Dbname { get; }
        DbConnection Create();
        void CreateFile(string filename);
    }
}
