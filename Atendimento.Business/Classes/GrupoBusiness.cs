using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Atendimento.Business.Interfaces.Interfaces;
using Atendimento.Entities.Entities;
using Atendimento.Repository.Interfaces.Interfaces;

namespace Atendimento.Business.Classes
{
    public class GrupoBusiness : IGrupoBusiness
    {
        IGrupoRepository _repository;

        public GrupoBusiness(IGrupoRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Grupo> GetAll()
        {
            return _repository.GetAll();
        }

        public Grupo GetById(int id)
        {
            return _repository.GetById(id);
        }

        public IEnumerable<Grupo> GetList(Expression<Func<Grupo, bool>> predicate)
        {
            return _repository.GetList(predicate);
        }

        public void Insert(ref Grupo entity)
        {
            _repository.Insert(ref entity);
        }

        public int Insert(IEnumerable<Grupo> list)
        {
            return _repository.Insert(list);
        }

        public bool Update(Grupo entity)
        {
            return _repository.Update(entity);
        }

        public bool Update(IEnumerable<Grupo> list)
        {
            return _repository.Update(list);
        }

        public bool Delete(int id)
        {
            return _repository.Delete(id);
        }

        public bool Delete(IEnumerable<Grupo> list)
        {
            return _repository.Delete(list);
        }
    }
}
