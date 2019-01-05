using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Atendimento.Business.Interfaces.Interfaces;
using Atendimento.Entities.Entities;
using Atendimento.Repository.Interfaces.Interfaces;

namespace Atendimento.Business.Classes
{
    public class StatusTicketBusiness : IStatusTicketBusiness
    {
        IStatusTicketRepository _repository;

        public StatusTicketBusiness(IStatusTicketRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<StatusTicket> GetAll()
        {
            return _repository.GetAll();
        }

        public StatusTicket GetById(int id)
        {
            return _repository.GetById(id);
        }

        public IEnumerable<StatusTicket> GetList(Expression<Func<StatusTicket, bool>> predicate)
        {
            return _repository.GetList(predicate);
        }

        public void Insert(ref StatusTicket entity)
        {
            _repository.Insert(ref entity);
        }

        public int Insert(IEnumerable<StatusTicket> list)
        {
            return _repository.Insert(list);
        }

        public bool Update(StatusTicket entity)
        {
            return _repository.Update(entity);
        }

        public bool Update(IEnumerable<StatusTicket> list)
        {
            return _repository.Update(list);
        }

        public bool Delete(int id)
        {
            return _repository.Delete(id);
        }

        public bool Delete(IEnumerable<StatusTicket> list)
        {
            return _repository.Delete(list);
        }
    }
}
