using Atendimento.Entities.Entities;
using Dapper.FluentMap.Dommel.Mapping;

namespace Atendimento.Repository.Mapping
{
    public class AnexoTicketMapping : DommelEntityMap<AnexoTicket>
    {
        public AnexoTicketMapping()
        {
            ToTable("Anexo_Ticket");
            Map(m => m.Id).ToColumn("id").IsKey().IsIdentity();
            Map(m => m.IdAnexo).ToColumn("id_anexo");
            Map(m => m.IdTicket).ToColumn("id_ticket");
        }
    }
}
