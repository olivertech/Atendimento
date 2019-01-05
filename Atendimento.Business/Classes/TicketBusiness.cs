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
    public class TicketBusiness : ITicketBusiness
    {
        ITicketRepository _repository;

        public TicketBusiness(ITicketRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Ticket> GetAll()
        {
            return _repository.GetAll();
        }

        public Ticket GetById(int id)
        {
            return _repository.GetById(id);
        }

        public IEnumerable<Ticket> GetList(Expression<Func<Ticket, bool>> predicate)
        {
            return _repository.GetList(predicate);
        }

        public void Insert(ref Ticket entity)
        {
            _repository.Insert(ref entity);
        }

        public int Insert(IEnumerable<Ticket> list)
        {
            return _repository.Insert(list);
        }

        public bool Update(Ticket entity)
        {
            return _repository.Update(entity);
        }

        public bool Update(IEnumerable<Ticket> list)
        {
            return _repository.Update(list);
        }

        public bool Delete(int id)
        {
            return _repository.Delete(id);
        }

        public bool Delete(IEnumerable<Ticket> list)
        {
            return _repository.Delete(list);
        }

        /** MÉTODOS EXTRAS */
        public CountsResponse GetCounts()
        {
            return _repository.GetCounts();
        }

        public int GetCount(int idStatusTicket)
        {
            return _repository.GetCount(idStatusTicket);
        }

        public TicketsResponse GetAllPaged(FilterRequest advancedFilter)
        {
            return _repository.GetAllPaged(advancedFilter);
        }

        public TicketResponse GetByIdWithAnexos(int id)
        {
            return _repository.GetByIdWithAnexos(id);
        }

        public bool UpdateStatusTicket(Ticket ticket)
        {
            return _repository.UpdateStatusTicket(ticket);
        }
    }
}
