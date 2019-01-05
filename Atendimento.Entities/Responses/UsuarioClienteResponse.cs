namespace Atendimento.Entities.Responses
{
    public class UsuarioClienteResponse
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public string Nome { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public bool Ativo { get; set; }
        public bool Copia { get; set; }
        public bool Provisorio { get; set; }
        public bool IsAdmin { get; set; }
    }
}