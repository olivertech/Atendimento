using System;
using System.ComponentModel.DataAnnotations;

namespace Atendimento.Entities.Requests
{
    public class TicketMensagemRequest
    {
        [Required(ErrorMessage = "Id do ticket original não informado.")]
        public int IdTicket { get; set; }

        [Required(ErrorMessage = "Informe o id do autor.")]
        public int IdAutor { get; set; }

        [Required(ErrorMessage = "Informe o tipo do autor.")]
        [StringLength(1, ErrorMessage = "Informe 'A' para Atendente ou 'C' para cliente.")]
        public string TipoUsuario { get; set; }

        [Required(ErrorMessage = "Informe o texto descritivo da mensagem.")]
        [StringLength(10000)]
        public string Descricao { get; set; }

        public DateTime DataHoraMensagem { get; set; } = DateTime.Now;

        public bool Interno { get; set; } = false;
    }
}
