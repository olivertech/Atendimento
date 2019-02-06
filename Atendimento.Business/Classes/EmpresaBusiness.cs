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
    public class EmpresaBusiness : IEmpresaBusiness
    {
        IEmpresaRepository _repository;

        public EmpresaBusiness(IEmpresaRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Empresa> GetAll()
        {
            return _repository.GetAll();
        }

        public Empresa GetById(int id)
        {
            return _repository.GetById(id);
        }

        public IEnumerable<Empresa> GetList(Expression<Func<Empresa, bool>> predicate)
        {
            return _repository.GetList(predicate);
        }

        public void Insert(ref Empresa entity)
        {
            _repository.Insert(ref entity);
        }

        public int Insert(IEnumerable<Empresa> list)
        {
            return _repository.Insert(list);
        }

        public bool Update(Empresa entity)
        {
            return _repository.Update(entity);
        }

        public bool Update(IEnumerable<Empresa> list)
        {
            return _repository.Update(list);
        }

        public bool Delete(int id)
        {
            return _repository.Delete(id);
        }

        public bool Delete(IEnumerable<Empresa> list)
        {
            return _repository.Delete(list);
        }

        public EmpresasResponse GetAllPaged(FilterEmpresaRequest advancedFilter)
        {
            return _repository.GetAllPaged(advancedFilter);
        }

        public int GetTotalAtendentesEmpresa(int idEmpresa)
        {
            return _repository.GetTotalAtendentesEmpresa(idEmpresa);
        }
    }
}
