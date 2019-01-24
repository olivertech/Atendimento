using System.Collections.Generic;

namespace Atendimento.Business.Interfaces.Interfaces
{
    public interface IBusiness<T> where T : class
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
