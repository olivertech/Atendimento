using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Atendimento.Entities.Entities;

namespace Atendimento.Repository.Interfaces.Interfaces.Base
{
    public interface IRepositoryBase<T> where T : BaseEntity
    {
        IEnumerable<T> GetAll();
        T GetById(Int32 id);
        void Insert(ref T entity);
        int Insert(IEnumerable<T> list);
        bool Update(T entity);
        bool Update(IEnumerable<T> list);
        bool Delete(Int32 id);
        bool Delete(IEnumerable<T> list);
        IEnumerable<T> GetList(Expression<Func<T, bool>> predicate);
    }
}
