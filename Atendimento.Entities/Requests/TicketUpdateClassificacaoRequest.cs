using System.ComponentModel.DataAnnotations;

namespace Atendimento.Entities.Requests
{
    public class TicketUpdateClassificacaoRequest
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int IdClassificacao { get; set; }
        [Required]
        public int IdUsuarioCliente { get; set; }
        [Required]
        public string UserTypeAgent { get; set; }

        public int IdAtendente { get; set; }
    }
}
