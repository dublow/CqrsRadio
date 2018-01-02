using System.Threading.Tasks;

namespace CqrsRadio.Infrastructure.Persistences
{
    public interface IDatabase
    {
        void Create();
        void Restore();
    }
}