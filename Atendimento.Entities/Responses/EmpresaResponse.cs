namespace Atendimento.Entities.Responses
{
    public class EmpresaResponse
    {
        public int Id { get; set; }
        public int IdGrupo { get; set; }
        public string Nome { get; set; }
        public string Cnpj { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public EnderecoResponse Endereco { get; set; }
        public string Tipo { get; set; }
        public string Descricao { get; set; }
    }
}