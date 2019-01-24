using System.ComponentModel.DataAnnotations;

namespace Atendimento.Entities.Requests
{
    public class UsuarioClienteRequest
    {
        [Required]
        public int IdCliente { get; set; }

        [Required(ErrorMessage = "O campo 'Nome' é obrigatório.")]
        [StringLength(50)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo 'Username' é obrigatório.")]
        [StringLength(50)]
        public string Username { get; set; }

        [Required(ErrorMessage = "O campo 'Password' é obrigatório.")]
        [StringLength(10)]
        public string Password { get; set; }

        [Required(ErrorMessage = "O campo 'Email' é obrigatório.")]
        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(14)]
        public string TelefoneFixo { get; set; }

        [StringLength(15)]
        public string TelefoneCelular { get; set; }

        public bool Ativo { get; set; }

        public bool Copia { get; set; }

        public bool Provisorio { get; set; }
    }
}