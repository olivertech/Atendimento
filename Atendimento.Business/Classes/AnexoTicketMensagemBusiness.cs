using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Atendimento.Business.Interfaces.Interfaces;
using Atendimento.Entities.Entities;
using Atendimento.Repository.Interfaces.Interfaces;

namespace Atendimento.Business.Classes
{
    public class AnexoTicketMensagemBusiness : IAnexoTicketMensagemBusiness
    {
        IAnexoTicketMensagemRepository _repository;

        public AnexoTicketMensagemBusiness(IAnexoTicketMensagemRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<AnexoTicketMensagem> GetAll()
        {
            return _repository.GetAll();
        }

        public AnexoTicketMensagem GetById(int id)
        {
            return _repository.GetById(id);
        }

        public IEnumerable<AnexoTicketMensagem> GetList(Expression<Func<AnexoTicketMensagem, bool>> predicate)
        {
            return _repository.GetList(predicate);
        }

        public void Insert(ref AnexoTicketMensagem entity)
        {
            _repository.Insert(ref entity);
        }

        public int Insert(IEnumerable<AnexoTicketMensagem> list)
        {
            return _repository.Insert(list);
        }

        public bool Update(AnexoTicketMensagem entity)
        {
            return _repository.Update(entity);
        }

        public bool Update(IEnumerable<AnexoTicketMensagem> list)
        {
            return _repository.Update(list);
        }

        public bool Delete(int id)
        {
            return _repository.Delete(id);
        }

        public bool Delete(IEnumerable<AnexoTicketMensagem> list)
        {
            return _repository.Delete(list);
        }
    }
}
