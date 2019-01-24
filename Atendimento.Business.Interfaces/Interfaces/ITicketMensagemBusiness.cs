using System.Collections.Generic;
using Atendimento.Business.Interfaces.Interfaces.Base;
using Atendimento.Entities.Entities;
using Atendimento.Entities.Requests;
using Atendimento.Entities.Responses;

namespace Atendimento.Business.Interfaces.Interfaces
{
    public interface ITicketMensagemBusiness : IBusinessBase<TicketMensagem>
    {
        IEnumerable<TicketMensagemResponse> GetAllByTicketId(TicketMensagensRequest request);
        void EnviarEmailConfirmacao(TicketMensagemRequest request, 
                                    TicketMensagem ticketMensagem, 
                                    Ticket ticket, 
                                    AtendenteEmpresa atendenteEmpresa, 
                                    UsuarioCliente usuarioCliente,
                                    List<AtendenteEmpresa> listaAtendentes);
    }
}
