using System.ComponentModel.DataAnnotations;

namespace Atendimento.Entities.Requests
{
    public class TemplateRespostaUpdate
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo 'Título' é obrigatório.")]
        [StringLength(150)]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "O campo 'Descrição' é obrigatório.")]
        [StringLength(1000)]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "O campo 'Conteúdo' é obrigatório.")]
        [StringLength(10000)]
        public string Conteudo { get; set; }
    }
}