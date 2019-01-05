using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Atendimento.Repository.Interfaces.Interfaces
{
    public interface IRepositoryOLD<T> where T : class
    {
        IEnumerable<T> GetAll();
        T Get(int id);
        int Insert(T obj);
        int Insert(IEnumerable<T> list);
        bool Update(T obj);
        bool Update(IEnumerable<T> list);
        bool Delete(T obj);
        bool Delete(IEnumerable<T> list);
        IEnumerable<T> GetList(Expression<Func<T, bool>> predicate);
    }
}
