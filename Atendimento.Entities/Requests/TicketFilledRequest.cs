using System.ComponentModel.DataAnnotations;

namespace Atendimento.Entities.Requests
{
    /// <summary>
    /// Esse request é usado pra recuperar um ticket completamente preenchido,
    /// com todas as subclasses carregadas, podendo ou não trazer também os anexos
    /// associados
    /// </summary>
    public class TicketFilledRequest
    {
        [Required(ErrorMessage = "Informe o id do ticket.")]
        public int Id { get; set; }

        public bool WithAnexos { get; set; } = false;
    }
}
