using Atendimento.Entities.Entities;
using Dapper.FluentMap.Dommel.Mapping;

namespace Atendimento.Repository.Mapping
{
    public class AnexoMapping : DommelEntityMap<Anexo>
    {
        public AnexoMapping()
        {
            ToTable("Anexo");
            Map(m => m.Id).ToColumn("id").IsKey().IsIdentity();
            Map(m => m.IdTicket).ToColumn("id_ticket");
            Map(m => m.IdTicketMensagem).ToColumn("id_ticket_mensagem");
            Map(m => m.Nome).ToColumn("nome");
        }
    }
}
