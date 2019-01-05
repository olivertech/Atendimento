namespace Atendimento.Entities.Responses
{
    public class ClienteResponse
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cnpj { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public EnderecoResponse Endereco { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
    }
}
