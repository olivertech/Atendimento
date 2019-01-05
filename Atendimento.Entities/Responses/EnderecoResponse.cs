namespace Atendimento.Entities.Responses
{
    public class EnderecoResponse
    {
        public string Logradouro { get; set; }
        public string NumeroLogradouro { get; set; }
        public string ComplementoLogradouro { get; set; }
        public string Estado { get; set; }
        public string Cidade { get; set; }
        public string Bairro { get; set; }
        public string Cep { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}