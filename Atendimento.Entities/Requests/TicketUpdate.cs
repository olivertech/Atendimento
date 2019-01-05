using System;
using System.ComponentModel.DataAnnotations;

namespace Atendimento.Entities.Requests
{
    public class TicketUpdate
    {
        public int Id { get; set; }

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

        public DateTime? DataHoraAlteracao { get; set; }

        public DateTime? DataHoraUltimaMensagem { get; set; }

        public DateTime? DataHoraFinal { get; set; }
    }
}
