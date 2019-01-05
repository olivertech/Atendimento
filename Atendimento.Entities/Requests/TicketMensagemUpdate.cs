using System;
using System.ComponentModel.DataAnnotations;

namespace Atendimento.Entities.Requests
{
    public class TicketMensagemUpdate
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Id do ticket original não informado.")]
        public int IdTicket { get; set; }

        [Required]
        public int IdUsuarioCliente { get; set; }

        [Required]
        public int IdAtendenteEmpresa { get; set; }

        [Required]
        [StringLength(10000)]
        public string Descricao { get; set; }

        public DateTime DataHoraMensagem { get; set; }

        public bool Interno { get; set; }
    }
}
