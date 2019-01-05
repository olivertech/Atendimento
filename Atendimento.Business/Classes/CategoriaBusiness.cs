using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Atendimento.Business.Interfaces.Interfaces;
using Atendimento.Entities.Entities;
using Atendimento.Repository.Interfaces.Interfaces;

namespace Atendimento.Business.Classes
{
    public class CategoriaBusiness : ICategoriaBusiness
    {
        ICategoriaRepository _repository;

        public CategoriaBusiness(ICategoriaRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Categoria> GetAll()
        {
            return _repository.GetAll();
        }

        public Categoria GetById(int id)
        {
            return _repository.GetById(id);
        }

        public IEnumerable<Categoria> GetList(Expression<Func<Categoria, bool>> predicate)
        {
            return _repository.GetList(predicate);
        }

        public void Insert(ref Categoria entity)
        {
            _repository.Insert(ref entity);
        }

        public int Insert(IEnumerable<Categoria> list)
        {
            return _repository.Insert(list);
        }

        public bool Update(Categoria entity)
        {
            return _repository.Update(entity);
        }

        public bool Update(IEnumerable<Categoria> list)
        {
            return _repository.Update(list);
        }

        public bool Delete(int id)
        {
            return _repository.Delete(id);
        }

        public bool Delete(IEnumerable<Categoria> list)
        {
            return _repository.Delete(list);
        }
    }
}
