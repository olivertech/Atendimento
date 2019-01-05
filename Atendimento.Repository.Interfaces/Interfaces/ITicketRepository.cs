using Atendimento.Entities.Entities;
using Atendimento.Entities.Requests;
using Atendimento.Entities.Responses;
using Atendimento.Repository.Interfaces.Interfaces.Base;

namespace Atendimento.Repository.Interfaces.Interfaces
{
    public interface ITicketRepository : IRepositoryBase<Ticket>
    {
        int GetCount(int idStatusTicket);
        CountsResponse GetCounts();
        TicketsResponse GetAllPaged(FilterRequest advancedFilter);
        TicketResponse GetByIdWithAnexos(int id);
        bool UpdateStatusTicket(Ticket ticket);
    }
}
