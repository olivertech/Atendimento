using System.ComponentModel.DataAnnotations;

namespace Atendimento.Entities.Requests
{
    public class EmpresaRequest
    {
        [Required(ErrorMessage = "O campo 'Nome' é obrigatório.")]
        [StringLength(255)]
        public string Nome { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(14)]
        public string TelefoneFixo { get; set; }

        [StringLength(15)]
        public string TelefoneCelular { get; set; }

        public bool IsDefault { get; set; }

        [StringLength(1000)]
        public string Descricao { get; set; }
    }
}