using System.ComponentModel.DataAnnotations;

namespace Atendimento.Entities.Requests
{
    public class ClassificacaoRequest
    {
        [Required(ErrorMessage = "O campo 'Nome' é obrigatório.")]
        [StringLength(50)]
        public string Nome { get; set; }
    }
}