using Atendimento.Entities.Entities;
using Dapper.FluentMap.Dommel.Mapping;

namespace Atendimento.Repository.Mapping
{
    public class ClienteMapping : DommelEntityMap<Cliente>
    {
        public ClienteMapping()
        {
            ToTable("Cliente");
            Map(m => m.Id).ToColumn("id").IsKey().IsIdentity();
            Map(m => m.IdEmpresa).ToColumn("id_empresa");
            Map(m => m.Nome).ToColumn("nome");
            Map(m => m.Cnpj).ToColumn("cnpj");
            Map(m => m.Email).ToColumn("email");
            Map(m => m.TelefoneFixo).ToColumn("telefone_fixo");
            Map(m => m.TelefoneCelular).ToColumn("telefone_celular");
            Map(m => m.Endereco.Logradouro).ToColumn("logradouro");
            Map(m => m.Endereco.NumeroLogradouro).ToColumn("numero_logradouro");
            Map(m => m.Endereco.ComplementoLogradouro).ToColumn("complemento_logradouro");
            Map(m => m.Endereco.Estado).ToColumn("estado");
            Map(m => m.Endereco.Cidade).ToColumn("cidade");
            Map(m => m.Endereco.Bairro).ToColumn("bairro");
            Map(m => m.Endereco.Cep).ToColumn("cep");
            Map(m => m.Descricao).ToColumn("descricao");
            Map(m => m.Ativo).ToColumn("ativo");
            Map(m => m.Empresa).Ignore();
        }
    }
}
