namespace CqrsRadio.Domain.Repositories
{
    public interface IDatabase
    {
        void CreateDatabase(string dbName);
        void CreateTable(string query);
    }
}
