namespace Atendimento.Entities.Responses
{
    public class ClienteResponse
    {
        public int Id { get; set; }
        public int IdEmpresa { get; set; }
        public string Nome { get; set; }
        public string Cnpj { get; set; }
        public string Email { get; set; }
        public string TelefoneFixo { get; set; }
        public string TelefoneCelular { get; set; }
        public string Logradouro { get; set; }
        public string NumeroLogradouro { get; set; }
        public string ComplementoLogradouro { get; set; }
        public string Estado { get; set; }
        public string Cidade { get; set; }
        public string Bairro { get; set; }
        public string Cep { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
    }
}
