using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Atendimento.Entities.Requests
{
    public class TicketRequest
    {
        [Required(ErrorMessage = "Informe um status para o ticket.")]
        public int IdStatusTicket { get; set; }

        [Required(ErrorMessage = "Informe um usuário do cliente para o ticket.")]
        public int IdUsuarioCliente { get; set; }

        [Required(ErrorMessage = "Informe uma categoria para o ticket.")]
        public int IdCategoria { get; set; }

        [Required(ErrorMessage = "Informe uma classificação para o ticket.")]
        public int IdClassificacao { get; set; }

        [StringLength(150, ErrorMessage = "Assunto permite no máximo 150 caracteres.")]
        [Required(ErrorMessage = "O campo 'Título' é obrigatório.")]
        public string Titulo { get; set; }

        [StringLength(10000, ErrorMessage = "Descrição permite no máximo 10 mil caracteres.")]
        [Required(ErrorMessage = "O campo 'Descrição' é obrigatório.")]
        public string Descricao { get; set; }

        //public int[] IdsAnexos { get; set; } = new int[5];

        //Propriedade usada pra receber temporariamente o nome da pasta temporaria onde foram gravados os anexos
        public string PathAnexos { get; set; }
    }
}
