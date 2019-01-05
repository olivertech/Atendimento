using System.ComponentModel.DataAnnotations;

namespace Atendimento.Entities.Requests
{
    public class StatusTicketUpdate
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo 'Nome' é obrigatório.")]
        [StringLength(50)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo 'Uso' é obrigatório. Informe 'E' ou 'I'.")]
        [StringLength(1, ErrorMessage = "O campo Uso aceita apenas 1 dígito.")]
        public string Uso { get; set; }

        [Required(ErrorMessage = "O campo 'Em Aberto' é obrigatório.")]
        public bool EmAberto { get; set; }

        [Required(ErrorMessage = "O campo 'Ordem Em Aberto' é obrigatório.")]
        public int OrdemEmAberto { get; set; }
    }
}