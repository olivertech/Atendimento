using System.Collections.Generic;
using Atendimento.Business.Interfaces.Interfaces.Base;
using Atendimento.Entities.Entities;
using Atendimento.Entities.Requests;
using Atendimento.Entities.Responses;

namespace Atendimento.Business.Interfaces.Interfaces
{
    public interface ITicketBusiness : IBusinessBase<Ticket>
    {
        int GetCount(int idStatusTicket);
        int GetTotalTicketsUsuario(int idUsuario);
        int GetTotalTicketsCategoria(int idCategoria);
        CountsResponse GetCounts(int idCliente);
        TicketsResponse GetAllPaged(FilterRequest advancedFilter);
        TicketResponse GetByIdFilled(int id, bool withAnexos);
        bool UpdateStatusTicket(Ticket ticket);
        void EnviarEmailConfirmacao(string userTypeAgent,
                                    StatusTicket statusTicket,
                                    Classificacao classificacao,
                                    Ticket ticket,
                                    UsuarioCliente usuarioCliente,
                                    AtendenteEmpresa atendenteEmpresa,
                                    List<AtendenteEmpresa> listaAtendentes,
                                    string acao);
    }
}