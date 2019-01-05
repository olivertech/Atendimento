namespace Atendimento.Entities.Entities
{
    public class Anexo : BaseEntity
    {
        public int? IdTicket { get; set; }
        public int? IdTicketMensagem { get; set; }
        public string Nome { get; set; }
    }
}
