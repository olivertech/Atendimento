using System;
using System.Collections.Generic;
using System.Text;
using Atendimento.Entities.Requests;
using Dapper.FluentMap.Dommel.Mapping;

namespace Atendimento.Repository.Mapping.InsertsUpdates
{
    public class InsertUpdateTicketMapping : DommelEntityMap<TicketRequest>
    {
        public InsertUpdateTicketMapping()
        {
            ToTable("Ticket");
            Map(m => m.Id).ToColumn("id_ticket").IsKey().IsIdentity();
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
        }
    }
}
