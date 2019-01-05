using Atendimento.Entities.Entities;
using Dapper.FluentMap.Dommel.Mapping;

namespace Atendimento.Repository.Mapping
{
    public class TicketMensagemMapping : DommelEntityMap<TicketMensagem>
    {
        public TicketMensagemMapping()
        {
            ToTable("Ticket");
            Map(m => m.Id).ToColumn("id_ticket_mensagem").IsKey().IsIdentity();
            Map(m => m.IdTicket).ToColumn("id_ticket");
            Map(m => m.IdUsuarioCliente).ToColumn("id_usuario_cliente");
            Map(m => m.IdAtendenteEmpresa).ToColumn("id_atendente_empresa");
            Map(m => m.Descricao).ToColumn("descricao");
            Map(m => m.DataHoraMensagem).ToColumn("data_hora_mensagem");
            Map(m => m.Interno).ToColumn("interno");
            Map(m => m.Ticket).Ignore();
            Map(m => m.UsuarioCliente).Ignore();
            Map(m => m.AtendenteEmpresa).Ignore();
        }
    }
}
