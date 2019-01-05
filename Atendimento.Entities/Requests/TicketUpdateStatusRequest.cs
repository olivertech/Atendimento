using System.ComponentModel.DataAnnotations;

namespace Atendimento.Entities.Requests
{
    public class TicketUpdateStatusRequest
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int IdStatusTicket { get; set; }
    }
}
