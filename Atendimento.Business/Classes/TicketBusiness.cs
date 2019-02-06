using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Atendimento.Business.Interfaces.Interfaces;
using Atendimento.Entities.Entities;
using Atendimento.Entities.Requests;
using Atendimento.Entities.Responses;
using Atendimento.Infra;
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
        public CountsResponse GetCounts(int idCliente)
        {
            return _repository.GetCounts(idCliente);
        }

        public int GetCount(int idStatusTicket)
        {
            return _repository.GetCount(idStatusTicket);
        }

        public int GetTotalTicketsUsuario(int idUsuario)
        {
            return _repository.GetTotalTicketsUsuario(idUsuario);
        }

        public int GetTotalTicketsCategoria(int idCategoria)
        {
            return _repository.GetTotalTicketsCategoria(idCategoria);
        }

        public TicketsResponse GetAllPaged(FilterRequest advancedFilter)
        {
            return _repository.GetAllPaged(advancedFilter);
        }

        public TicketResponse GetByIdFilled(int id, bool withAnexos)
        {
            return _repository.GetByIdFilled(id, withAnexos);
        }

        public bool UpdateStatusTicket(Ticket ticket)
        {
            return _repository.UpdateStatusTicket(ticket);
        }

        public void EnviarEmailConfirmacao(string userTypeAgent, 
                                           StatusTicket statusTicket, 
                                           Classificacao classificacao,
                                           Ticket ticket, 
                                           UsuarioCliente usuarioCliente, 
                                           AtendenteEmpresa atendenteEmpresa, 
                                           List<AtendenteEmpresa> listaAtendentes, 
                                           string acao)
        {
            //Verifica se o novo atendimento foi criado pelo cliente ou pelo suporte
            if (userTypeAgent == "S") //suporte
            {
                if (acao == "insert")
                {
                    //Envia email ao cliente, confirmando que um novo ticket de atendimento foi cadastrado com sucesso pelo suporte
                    Emailer.EnviarEmailNovoTicketCliente(ticket, usuarioCliente);

                    //Envia email ao suporte, confirmando que um novo ticket de atendimento foi cadastrado com sucesso pelo suporte
                    //permitindo que todos os atendentes envolvidos, fiquem cientes do novo atendimento
                    Emailer.EnviarEmailNovoTicketSuporte(ticket, usuarioCliente, atendenteEmpresa, listaAtendentes);
                }
                else
                {
                    if (acao == "update")
                    {
                        //Envia email ao cliente, confirmando atualização do ticket
                        Emailer.EnviarEmailAlteracaoStatusTicketCliente(ticket, statusTicket, usuarioCliente);
                    }
                    else
                    {
                        //Envia email ao cliente, confirmando atualização da classificacao do atendimento
                        Emailer.EnviarEmailAlteracaoClassificacaoCliente(ticket, classificacao, usuarioCliente);
                    }
                }
            }
            else //cliente
            {
                if (acao == "insert")
                {
                    //Envia email ao suporte, confirmando que um novo ticket de atendimento foi cadastrado com sucesso pelo cliente
                    Emailer.EnviarEmailNovoTicketSuporte(ticket, usuarioCliente, listaAtendentes);
                }
                else
                {
                    //Envia email ao suporte, confirmando atualização do ticket
                    //Emailer.EnviarEmailMensagemSuportee(ticket.Id, usuarioCliente);
                }
            }
        }
    }
}
