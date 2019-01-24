using System;

namespace Atendimento.Entities.Entities
{
    public class TicketMensagem : BaseEntity
    {
        public string Descricao { get; set; }
        public DateTime DataHoraMensagem { get; set; }
        public bool Interno { get; set; }

        /** COLUNAS DE CHAVE ESTRANGEIRA */
        public int IdTicket { get; set; }
        public Ticket Ticket { get; set; }
        public int? IdUsuarioCliente { get; set; }
        public UsuarioCliente UsuarioCliente { get; set; }
        public int? IdAtendenteEmpresa { get; set; }
        public AtendenteEmpresa AtendenteEmpresa { get; set; }
    }
}
