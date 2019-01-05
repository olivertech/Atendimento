using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Atendimento.Business.Interfaces.Interfaces;
using Atendimento.Entities.Entities;
using Atendimento.Repository.Interfaces.Interfaces;

namespace Atendimento.Business.Classes
{
    public class AnexoTicketBusiness : IAnexoTicketBusiness
    {
        IAnexoTicketRepository _repository;

        public AnexoTicketBusiness(IAnexoTicketRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<AnexoTicket> GetAll()
        {
            return _repository.GetAll();
        }

        public AnexoTicket GetById(int id)
        {
            return _repository.GetById(id);
        }

        public IEnumerable<AnexoTicket> GetList(Expression<Func<AnexoTicket, bool>> predicate)
        {
            return _repository.GetList(predicate);
        }

        public void Insert(ref AnexoTicket entity)
        {
            _repository.Insert(ref entity);
        }

        public int Insert(IEnumerable<AnexoTicket> list)
        {
            return _repository.Insert(list);
        }

        public bool Update(AnexoTicket entity)
        {
            return _repository.Update(entity);
        }

        public bool Update(IEnumerable<AnexoTicket> list)
        {
            return _repository.Update(list);
        }

        public bool Delete(int id)
        {
            return _repository.Delete(id);
        }

        public bool Delete(IEnumerable<AnexoTicket> list)
        {
            return _repository.Delete(list);
        }
    }
}
