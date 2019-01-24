using System.ComponentModel.DataAnnotations;

namespace Atendimento.Entities.Requests
{
    public class ClienteUpdate
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo IdEmpresa é obrigatório")]
        public int IdEmpresa { get; set; }

        [Required(ErrorMessage = "O campo 'Nome' é obrigatório.")]
        [StringLength(255)]
        public string Nome { get; set; }

        [StringLength(19)]
        public string Cnpj { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(14)]
        public string TelefoneFixo { get; set; }

        [StringLength(15)]
        public string TelefoneCelular { get; set; }

        public EnderecoRequest Endereco { get; set; }

        [StringLength(1000)]
        public string Descricao { get; set; }

        public bool Ativo { get; set; }
    }
}
