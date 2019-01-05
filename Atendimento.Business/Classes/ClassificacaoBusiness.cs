using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Atendimento.Business.Interfaces.Interfaces;
using Atendimento.Entities.Entities;
using Atendimento.Repository.Interfaces.Interfaces;

namespace Atendimento.Business.Classes
{
    public class ClassificacaoBusiness : IClassificacaoBusiness
    {
        IClassificacaoRepository _repository;

        public ClassificacaoBusiness(IClassificacaoRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Classificacao> GetAll()
        {
            return _repository.GetAll();
        }

        public Classificacao GetById(int id)
        {
            return _repository.GetById(id);
        }

        public IEnumerable<Classificacao> GetList(Expression<Func<Classificacao, bool>> predicate)
        {
            return _repository.GetList(predicate);
        }

        public void Insert(ref Classificacao entity)
        {
            _repository.Insert(ref entity);
        }

        public int Insert(IEnumerable<Classificacao> list)
        {
            return _repository.Insert(list);
        }

        public bool Update(Classificacao entity)
        {
            return _repository.Update(entity);
        }

        public bool Update(IEnumerable<Classificacao> list)
        {
            return _repository.Update(list);
        }

        public bool Delete(int id)
        {
            return _repository.Delete(id);
        }

        public bool Delete(IEnumerable<Classificacao> list)
        {
            return _repository.Delete(list);
        }
    }
}
