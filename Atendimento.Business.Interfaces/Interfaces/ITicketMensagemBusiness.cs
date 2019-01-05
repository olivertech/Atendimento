using System.Collections.Generic;
using Atendimento.Business.Interfaces.Interfaces.Base;
using Atendimento.Entities.Entities;
using Atendimento.Entities.Responses;

namespace Atendimento.Business.Interfaces.Interfaces
{
    public interface ITicketMensagemBusiness : IBusinessBase<TicketMensagem>
    {
        IEnumerable<TicketMensagemResponse> GetAllByTicketId(int idTicket);
    }
}
