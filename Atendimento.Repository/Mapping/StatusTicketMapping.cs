using Atendimento.Entities.Entities;
using Dapper.FluentMap.Dommel.Mapping;

namespace Atendimento.Repository.Mapping
{
    public class StatusTicketMapping : DommelEntityMap<StatusTicket>
    {
        public StatusTicketMapping()
        {
            ToTable("Status_Ticket");
            Map(m => m.Id).ToColumn("id_status_ticket").IsKey().IsIdentity(); //manter como id_status_ticket em função dos joins
            Map(m => m.Nome).ToColumn("nome");
            Map(m => m.Uso).ToColumn("uso");
            Map(m => m.EmAberto).ToColumn("em_aberto");
            Map(m => m.OrdemEmAberto).ToColumn("ordem_em_aberto");
        }
    }
}
