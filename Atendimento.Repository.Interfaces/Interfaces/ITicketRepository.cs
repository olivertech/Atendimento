using Atendimento.Entities.Entities;
using Atendimento.Entities.Requests;
using Atendimento.Entities.Responses;
using Atendimento.Repository.Interfaces.Interfaces.Base;

namespace Atendimento.Repository.Interfaces.Interfaces
{
    public interface ITicketRepository : IRepositoryBase<Ticket>
    {
        int GetCount(int idStatusTicket);
        int GetTotalTicketsUsuario(int idUsuario);
        CountsResponse GetCounts(int idCliente);
        TicketsResponse GetAllPaged(FilterRequest advancedFilter);
        TicketResponse GetByIdFilled(int id, bool withAnexos);
        bool UpdateStatusTicket(Ticket ticket);
    }
}
