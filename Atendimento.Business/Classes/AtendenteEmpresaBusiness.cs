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
    public class AtendenteEmpresaBusiness : IAtendenteEmpresaBusiness
    {
        IAtendenteEmpresaRepository _repository;

        public AtendenteEmpresaBusiness(IAtendenteEmpresaRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<AtendenteEmpresa> GetAll()
        {
            return _repository.GetAll();
        }

        public AtendenteEmpresa GetById(int id)
        {
            return _repository.GetById(id);
        }

        public IEnumerable<AtendenteEmpresa> GetList(Expression<Func<AtendenteEmpresa, bool>> predicate)
        {
            return _repository.GetList(predicate);
        }

        public void Insert(ref AtendenteEmpresa entity)
        {
            _repository.Insert(ref entity);
        }

        public int Insert(IEnumerable<AtendenteEmpresa> list)
        {
            return _repository.Insert(list);
        }

        public bool Update(AtendenteEmpresa entity)
        {
            return _repository.Update(entity);
        }

        public bool Update(IEnumerable<AtendenteEmpresa> list)
        {
            return _repository.Update(list);
        }

        public bool Delete(int id)
        {
            return _repository.Delete(id);
        }

        public bool Delete(IEnumerable<AtendenteEmpresa> list)
        {
            return _repository.Delete(list);
        }

        public IEnumerable<AtendenteEmpresa> GetAll(int idEmpresa)
        {
            return _repository.GetAll(idEmpresa);
        }

        public AtendentesEmpresaResponse GetAllPaged(FilterAtendenteEmpresaRequest advancedFilter)
        {
            return _repository.GetAllPaged(advancedFilter);
        }

        public AtendenteEmpresa GetByUsernameAndPassword(string username, string password)
        {
            return _repository.GetByUsernameAndPassword(username, password);
        }

        public bool UpdatePassword(AtendenteEmpresa atendente)
        {
            return _repository.UpdatePassword(atendente);
        }
    }
}
