using CqrsRadio.Infrastructure.Persistences;

namespace CqrsRadio.Tools
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlLiteDb.CreateDomain();

            
        }
    }
}
