using System;

namespace CqrsRadio.Domain.Repositories
{
    public interface IRadioRepository : IRepository
    {
        void Create(string name, Uri url);
        void Delete(string name);
    }
}