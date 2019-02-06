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

        [Required(ErrorMessage = "Informe o id do status.")]
        public int IdStatusTicket { get; set; }

        [Required(ErrorMessage = "O campo 'UserType' deve ser informado com a letra 'S' para Suporte ou 'C' para cliente")]
        public string UserType { get; set; } = "C";

        [Required(ErrorMessage = "Informe o texto descritivo da mensagem.")]
        [StringLength(10000)]
        public string Descricao { get; set; }

        public DateTime DataHoraMensagem { get; set; } = DateTime.Now;

        public bool Interno { get; set; } = false;

        //Propriedade usada pra receber temporariamente o nome da pasta temporaria onde foram gravados os anexos
        public string PathAnexos { get; set; }
    }
}
