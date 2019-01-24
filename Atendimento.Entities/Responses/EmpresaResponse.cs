namespace Atendimento.Entities.Responses
{
    public class EmpresaResponse
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string TelefoneFixo { get; set; }
        public string TelefoneCelular { get; set; }
        public bool IsDefault { get; set; }
        public string Descricao { get; set; }
    }
}