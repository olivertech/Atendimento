using System.ComponentModel.DataAnnotations;

namespace Atendimento.Entities.Requests
{
    public class TicketMensagensRequest
    {
        [Required(ErrorMessage = "Id do ticket original não informado.")]
        public int IdTicket { get; set; }
        public int IdCliente { get; set; } = 0;
    }
}
