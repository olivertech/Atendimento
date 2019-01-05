using System.ComponentModel.DataAnnotations;

namespace Atendimento.Entities.Requests
{
    public class ClassificacaoUpdate
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo 'Nome' é obrigatório.")]
        [StringLength(50)]
        public string Nome { get; set; }
    }
}