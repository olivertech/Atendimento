using System;

namespace Atendimento.Entities.Responses
{
    public class TicketMensagemResponse
    {
        public int Id { get; set; }
        public string Autor { get; set; }
        public string Descricao { get; set; }
        public DateTime Data { get; set; }
        public bool Interno { get; set; }
        public string Anexos { get; set; }
    }
}
