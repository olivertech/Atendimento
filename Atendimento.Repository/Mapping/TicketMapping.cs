using Atendimento.Entities.Entities;
using Dapper.FluentMap.Dommel.Mapping;

namespace Atendimento.Repository.Mapping
{
    public class TicketMapping : DommelEntityMap<Ticket>
    {
        public TicketMapping()
        {
            ToTable("Ticket");
            Map(m => m.Id).ToColumn("id").IsKey().IsIdentity();
            Map(m => m.IdStatusTicket).ToColumn("id_status_ticket");
            Map(m => m.IdUsuarioCliente).ToColumn("id_usuario_cliente");
            Map(m => m.IdCategoria).ToColumn("id_categoria");
            Map(m => m.IdClassificacao).ToColumn("id_classificacao");
            Map(m => m.Titulo).ToColumn("titulo");
            Map(m => m.Descricao).ToColumn("descricao");
            Map(m => m.DataHoraInicial).ToColumn("data_hora_inicial");
            Map(m => m.DataHoraAlteracao).ToColumn("data_hora_alteracao");
            Map(m => m.DataHoraUltimaMensagem).ToColumn("data_hora_ultima_mensagem");
            Map(m => m.DataHoraFinal).ToColumn("data_hora_final");
            Map(m => m.Categoria).Ignore();
            Map(m => m.Classificacao).Ignore();
            Map(m => m.StatusTicket).Ignore();
            Map(m => m.UsuarioCliente).Ignore();
        }
    }
}
