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

        [StringLength(255)]
        public string Logradouro { get; set; }

        [StringLength(25)]
        public string NumeroLogradouro { get; set; }

        [StringLength(50)]
        public string ComplementoLogradouro { get; set; }

        [StringLength(2)]
        public string Estado { get; set; }

        [StringLength(50)]
        public string Cidade { get; set; }

        [StringLength(50)]
        public string Bairro { get; set; }

        [StringLength(9)]
        public string Cep { get; set; }

        [StringLength(1000)]
        public string Descricao { get; set; }

        public bool Ativo { get; set; }
    }
}
