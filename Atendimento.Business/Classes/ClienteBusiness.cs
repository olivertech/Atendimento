using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Atendimento.Business.Interfaces.Interfaces;
using Atendimento.Entities.Entities;
using Atendimento.Repository.Interfaces.Interfaces;

namespace Atendimento.Business.Classes
{
    public class ClienteBusiness : IClienteBusiness
    {
        IClienteRepository _repository;

        public ClienteBusiness(IClienteRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Cliente> GetAll()
        {
            return _repository.GetAll();
        }

        public Cliente GetById(int id)
        {
            return _repository.GetById(id);
        }

        public IEnumerable<Cliente> GetList(Expression<Func<Cliente, bool>> predicate)
        {
            return _repository.GetList(predicate);
        }

        public void Insert(ref Cliente entity)
        {
            _repository.Insert(ref entity);
        }

        public int Insert(IEnumerable<Cliente> list)
        {
            return _repository.Insert(list);
        }

        public bool Update(Cliente entity)
        {
            return _repository.Update(entity);
        }

        public bool Update(IEnumerable<Cliente> list)
        {
            return _repository.Update(list);
        }

        public bool Delete(int id)
        {
            return _repository.Delete(id);
        }

        public bool Delete(IEnumerable<Cliente> list)
        {
            return _repository.Delete(list);
        }
    }
}
