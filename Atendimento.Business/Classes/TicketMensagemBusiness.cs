using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Atendimento.Business.Interfaces.Interfaces;
using Atendimento.Entities.Entities;
using Atendimento.Entities.Responses;
using Atendimento.Repository.Interfaces.Interfaces;

namespace Atendimento.Business.Classes
{
    public class TicketMensagemBusiness : ITicketMensagemBusiness
    {
        ITicketMensagemRepository _repository;

        public TicketMensagemBusiness(ITicketMensagemRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<TicketMensagem> GetAll()
        {
            return _repository.GetAll();
        }

        public TicketMensagem GetById(int id)
        {
            return _repository.GetById(id);
        }

        public IEnumerable<TicketMensagem> GetList(Expression<Func<TicketMensagem, bool>> predicate)
        {
            return _repository.GetList(predicate);
        }

        public void Insert(ref TicketMensagem entity)
        {
            _repository.Insert(ref entity);
        }

        public int Insert(IEnumerable<TicketMensagem> list)
        {
            return _repository.Insert(list);
        }

        public bool Update(TicketMensagem entity)
        {
            return _repository.Update(entity);
        }

        public bool Update(IEnumerable<TicketMensagem> list)
        {
            return _repository.Update(list);
        }

        public bool Delete(int id)
        {
            return _repository.Delete(id);
        }

        public bool Delete(IEnumerable<TicketMensagem> list)
        {
            return _repository.Delete(list);
        }

        public IEnumerable<TicketMensagemResponse> GetAllByTicketId(int idTicket)
        {
            return _repository.GetAllByTicketId(idTicket);
        }
    }
}

