using System.Collections.Generic;

namespace Atendimento.Business.Interfaces.Interfaces
{
    public interface IBusinessOLD<T> where T : class
    {
        IEnumerable<T> GetAll();
        T Get(int id);
        int Insert(T obj);
        int Insert(IEnumerable<T> list);
        bool Update(T obj);
        bool Update(IEnumerable<T> list);
        bool Delete(T obj);
        bool Delete(IEnumerable<T> list);
    }
}
