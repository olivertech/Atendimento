using Atendimento.Business.Interfaces.Interfaces.Base;
using Atendimento.Entities.Entities;
using Atendimento.Entities.Requests;
using Atendimento.Entities.Responses;

namespace Atendimento.Business.Interfaces.Interfaces
{
    public interface ITicketBusiness : IBusinessBase<Ticket>
    {
        int GetCount(int idStatusTicket);
        CountsResponse GetCounts();
        TicketsResponse GetAllPaged(FilterRequest advancedFilter);
        TicketResponse GetByIdWithAnexos(int id);
        bool UpdateStatusTicket(Ticket ticket);
    }
}
