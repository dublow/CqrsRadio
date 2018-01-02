using System.Data.Common;

namespace CqrsRadio.Infrastructure.Providers
{
    public interface IDbParameter
    {
        DbParameter Create(string name, object value);
    }
}
