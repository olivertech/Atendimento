using System.ComponentModel.DataAnnotations;

namespace Atendimento.Entities.Requests
{
    public class TicketUpdateStatusRequest
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int IdStatusTicket { get; set; }
        [Required]
        public int IdUsuarioCliente { get; set; }
        [Required]
        public string UserTypeAgent { get; set; }

        public int IdAtendente { get; set; }
    }
}
