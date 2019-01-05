using Atendimento.Entities.Entities;
using Dapper.FluentMap.Dommel.Mapping;

namespace Atendimento.Repository.Mapping
{
    public class AnexoTicketMensagemMapping : DommelEntityMap<AnexoTicketMensagem>
    {
        public AnexoTicketMensagemMapping()
        {
            ToTable("AnexoTicket");
            Map(m => m.Id).ToColumn("id").IsKey().IsIdentity();
            Map(m => m.IdAnexo).ToColumn("id_anexo");
            Map(m => m.IdTicketMensagem).ToColumn("id_ticket_mensagem");
        }
    }
}
