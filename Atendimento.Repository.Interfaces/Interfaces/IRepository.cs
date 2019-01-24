using System.Collections.Generic;
using Atendimento.Entities.Entities;

namespace Atendimento.Repository.Interfaces.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T Get(long id);
        long Insert(T obj);
        long Insert(IEnumerable<T> list);
        bool Update(T obj);
        bool Update(IEnumerable<T> list);
        bool Delete(T obj);
        bool Delete(IEnumerable<T> list);
    }
}
