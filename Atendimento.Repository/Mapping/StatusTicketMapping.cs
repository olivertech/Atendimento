using Atendimento.Entities.Entities;
using Dapper.FluentMap.Dommel.Mapping;

namespace Atendimento.Repository.Mapping
{
    public class StatusTicketMapping : DommelEntityMap<StatusTicket>
    {
        public StatusTicketMapping()
        {
            ToTable("Status_Ticket");
            Map(m => m.Id).ToColumn("id").IsKey().IsIdentity();
            Map(m => m.Nome).ToColumn("nome");
            Map(m => m.Uso).ToColumn("uso");
            Map(m => m.EmAberto).ToColumn("em_aberto");
            Map(m => m.OrdemEmAberto).ToColumn("ordem_em_aberto");
        }
    }
}
