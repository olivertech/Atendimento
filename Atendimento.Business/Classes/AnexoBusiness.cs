using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Atendimento.Business.Interfaces.Interfaces;
using Atendimento.Entities.Entities;
using Atendimento.Repository.Interfaces.Interfaces;

namespace Atendimento.Business.Classes
{
    public class AnexoBusiness : IAnexoBusiness
    {
        IAnexoRepository _repository;

        public AnexoBusiness(IAnexoRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Anexo> GetAll()
        {
            return _repository.GetAll();
        }

        public Anexo GetById(int id)
        {
            return _repository.GetById(id);
        }

        public IEnumerable<Anexo> GetList(Expression<Func<Anexo, bool>> predicate)
        {
            return _repository.GetList(predicate);
        }

        public void Insert(ref Anexo entity)
        {
            _repository.Insert(ref entity);
        }

        public int Insert(IEnumerable<Anexo> list)
        {
            return _repository.Insert(list);
        }

        public bool Update(Anexo entity)
        {
            return _repository.Update(entity);
        }

        public bool Update(IEnumerable<Anexo> list)
        {
            return _repository.Update(list);
        }

        public bool Delete(int id)
        {
            return _repository.Delete(id);
        }

        public bool Delete(IEnumerable<Anexo> list)
        {
            return _repository.Delete(list);
        }
    }
}
