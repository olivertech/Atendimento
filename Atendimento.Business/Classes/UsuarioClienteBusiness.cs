using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Atendimento.Business.Interfaces.Interfaces;
using Atendimento.Entities.Entities;
using Atendimento.Entities.Requests;
using Atendimento.Entities.Responses;
using Atendimento.Repository.Interfaces.Interfaces;

namespace Atendimento.Business.Classes
{
    public class UsuarioClienteBusiness : IUsuarioClienteBusiness
    {
        IUsuarioClienteRepository _repository;

        public UsuarioClienteBusiness(IUsuarioClienteRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<UsuarioCliente> GetAll()
        {
            return _repository.GetAll();
        }

        public UsuarioCliente GetById(int id)
        {
            return _repository.GetById(id);
        }

        public IEnumerable<UsuarioCliente> GetList(Expression<Func<UsuarioCliente, bool>> predicate)
        {
            return _repository.GetList(predicate);
        }

        public void Insert(ref UsuarioCliente entity)
        {
            _repository.Insert(ref entity);
        }

        public int Insert(IEnumerable<UsuarioCliente> list)
        {
            return _repository.Insert(list);
        }

        public bool Update(UsuarioCliente entity)
        {
            return _repository.Update(entity);
        }

        public bool Update(IEnumerable<UsuarioCliente> list)
        {
            return _repository.Update(list);
        }

        public bool Delete(int id)
        {
            return _repository.Delete(id);
        }

        public bool Delete(IEnumerable<UsuarioCliente> list)
        {
            return _repository.Delete(list);
        }

        public IEnumerable<UsuarioCliente> GetAllById(int idCliente)
        {
            return _repository.GetAllById(idCliente);
        }

        public UsuarioCliente GetByUsernameAndPassword(string username, string password)
        {
            return _repository.GetByUsernameAndPassword(username, password);
        }

        public bool UpdatePassword(UsuarioCliente usuarioCliente)
        {
            return _repository.UpdatePassword(usuarioCliente);
        }

        public int GetCount(int idCliente)
        {
            return _repository.GetCount(idCliente);
        }

        public UsuariosClienteResponse GetAllPaged(FilterUsuarioRequest advancedFilter)
        {
            return _repository.GetAllPaged(advancedFilter);
        }

        public UsuarioCliente GetFullById(int idUsuario)
        {
            return _repository.GetFullById(idUsuario);
        }
    }
}
