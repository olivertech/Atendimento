using System.Collections.Generic;
using Atendimento.Entities.Entities;

namespace Atendimento.Entities.Responses
{
    /// <summary>
    /// Classe usada para mapear o resultado da busca paginada de tickets
    /// que irá montar a lista de ticket do dashboard do sistema
    /// </summary>
    public class TicketsResponse
    {
        public int TotalGeral { get; set; }
        public int TotalFiltrado { get; set; }
        public IList<Ticket> Tickets { get; set; }
    }
}
