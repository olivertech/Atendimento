using Atendimento.Entities.Entities;
using Dapper.FluentMap.Dommel.Mapping;

namespace Atendimento.Repository.Mapping
{
    public class AnexoMapping : DommelEntityMap<Anexo>
    {
        public AnexoMapping()
        {
            ToTable("Anexo");
            Map(m => m.Id).ToColumn("id_anexo_entity").IsKey().IsIdentity();
            Map(m => m.IdTicket).ToColumn("ticketid");
            Map(m => m.IdTicketMensagem).ToColumn("ticketmensagemid");
            Map(m => m.Nome).ToColumn("nome");
        }
    }
}
