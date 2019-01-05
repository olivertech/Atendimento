namespace Atendimento.Entities.Responses
{
    public class StatusTicketResponse
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Uso { get; set; }
        public bool EmAberto { get; set; }
        public int OrdemEmAberto { get; set; }
    }
}