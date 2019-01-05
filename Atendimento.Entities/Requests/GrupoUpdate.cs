using System.ComponentModel.DataAnnotations;

namespace Atendimento.Entities.Requests
{
    public class GrupoUpdate
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo 'Nome' é obrigatório.")]
        [StringLength(255)]
        public string Nome { get; set; }

        [StringLength(50)]
        public string Logomarca { get; set; }

        [StringLength(50)]
        public string Site { get; set; }

        [StringLength(50)]
        public string Facebook { get; set; }

        [StringLength(50)]
        public string Twitter { get; set; }

        [StringLength(50)]
        public string Instagram { get; set; }
    }
}