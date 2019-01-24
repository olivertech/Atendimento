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
    public class TemplateRespostaBusiness : ITemplateRespostaBusiness
    {
        ITemplateRespostaRepository _repository;

        public TemplateRespostaBusiness(ITemplateRespostaRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<TemplateResposta> GetAll()
        {
            return _repository.GetAll();
        }

        public TemplateResposta GetById(int id)
        {
            return _repository.GetById(id);
        }

        public IEnumerable<TemplateResposta> GetList(Expression<Func<TemplateResposta, bool>> predicate)
        {
            return _repository.GetList(predicate);
        }

        public void Insert(ref TemplateResposta entity)
        {
            _repository.Insert(ref entity);
        }

        public int Insert(IEnumerable<TemplateResposta> list)
        {
            return _repository.Insert(list);
        }

        public bool Update(TemplateResposta entity)
        {
            return _repository.Update(entity);
        }

        public bool Update(IEnumerable<TemplateResposta> list)
        {
            return _repository.Update(list);
        }

        public bool Delete(int id)
        {
            return _repository.Delete(id);
        }

        public bool Delete(IEnumerable<TemplateResposta> list)
        {
            return _repository.Delete(list);
        }

        public TemplatesResponse GetAllPaged(FilterTemplateRequest advancedFilter)
        {
            return _repository.GetAllPaged(advancedFilter);
        }
    }
}
