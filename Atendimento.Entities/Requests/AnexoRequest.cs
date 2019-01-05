using System.ComponentModel.DataAnnotations;

namespace Atendimento.Entities.Requests
{
    public class AnexoRequest
    {
        [Required]
        [StringLength(250, ErrorMessage = "Nome do anexo deve conter no máximo 250 caracteres.")]
        public string Nome { get; set; }
    }
}
