using Atendimento.Entities.Entities;
using Dapper.FluentMap.Dommel.Mapping;

namespace Atendimento.Repository.Mapping
{
    public class TicketMensagemMapping : DommelEntityMap<TicketMensagem>
    {
        public TicketMensagemMapping()
        {
            ToTable("Ticket_Mensagem");
            Map(m => m.Id).ToColumn("id").IsKey().IsIdentity();
            Map(m => m.IdTicket).ToColumn("ticketid");
            Map(m => m.IdUsuarioCliente).ToColumn("usuarioclienteid");
            Map(m => m.IdAtendenteEmpresa).ToColumn("atendenteempresaid");
            Map(m => m.Descricao).ToColumn("descricao");
            Map(m => m.DataHoraMensagem).ToColumn("data_hora_mensagem");
            Map(m => m.Interno).ToColumn("interno");
            Map(m => m.Ticket).Ignore();
            Map(m => m.UsuarioCliente).Ignore();
            Map(m => m.AtendenteEmpresa).Ignore();
        }
    }
}
