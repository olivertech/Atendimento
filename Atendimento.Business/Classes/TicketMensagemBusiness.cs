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

        public IEnumerable<TicketMensagemResponse> GetAllByTicketId(TicketMensagensRequest request)
        {
            return _repository.GetAllByTicketId(request);
        }

        public void EnviarEmailConfirmacao(TicketMensagemRequest request, 
                                           TicketMensagem ticketMensagem,
                                           TicketResponse ticket, 
                                           AtendenteEmpresa atendenteEmpresa, 
                                           UsuarioCliente usuarioCliente, 
                                           List<AtendenteEmpresa> listaAtendentes)
        {
            //Se for mensagem enviada pelo atendimento (suporte) e interno
            if (request.UserType == "S" && request.Interno)
            {
                //Se for nova mensagem interna criada pelo suporte
                Emailer.EnviarEmailInterno(ticket, ticketMensagem, atendenteEmpresa, listaAtendentes);
            }
            else
            {
                if(request.UserType == "S" && !request.Interno)
                {
                    //Se for nova mensagem (não interna) criada pelo suporte
                    Emailer.EnviarEmailNovaMensagemCliente(ticket, ticketMensagem);
                }
                else
                {
                    if (request.UserType == "C")
                    {
                        //Se for nova mensagem criada pelo cliente
                        Emailer.EnviarEmailNovaMensagemSuporte(ticket, ticketMensagem, usuarioCliente, listaAtendentes);
                    }
                }
            }
        }
    }
}

