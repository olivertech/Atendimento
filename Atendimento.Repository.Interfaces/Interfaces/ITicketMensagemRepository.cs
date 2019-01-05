using System.Collections.Generic;
using Atendimento.Entities.Entities;
using Atendimento.Entities.Responses;
using Atendimento.Repository.Interfaces.Interfaces.Base;

namespace Atendimento.Repository.Interfaces.Interfaces
{
    public interface ITicketMensagemRepository : IRepositoryBase<TicketMensagem>
    {
        IEnumerable<TicketMensagemResponse> GetAllByTicketId(int idTicket);
    }
}
