using Atendimento.Entities.Entities;
using Dapper.FluentMap.Dommel.Mapping;

namespace Atendimento.Repository.Mapping
{
    public class ClienteMapping : DommelEntityMap<Cliente>
    {
        public ClienteMapping()
        {
            ToTable("Cliente");
            Map(m => m.Id).ToColumn("id_cliente_entity").IsKey().IsIdentity();
            Map(m => m.IdEmpresa).ToColumn("empresaid");
            Map(m => m.Nome).ToColumn("nome");
            Map(m => m.Cnpj).ToColumn("cnpj");
            Map(m => m.Email).ToColumn("email");
            Map(m => m.TelefoneFixo).ToColumn("telefone_fixo");
            Map(m => m.TelefoneCelular).ToColumn("telefone_celular");
            Map(m => m.Logradouro).ToColumn("logradouro");
            Map(m => m.NumeroLogradouro).ToColumn("numero_logradouro");
            Map(m => m.ComplementoLogradouro).ToColumn("complemento_logradouro");
            Map(m => m.Estado).ToColumn("estado");
            Map(m => m.Cidade).ToColumn("cidade");
            Map(m => m.Bairro).ToColumn("bairro");
            Map(m => m.Cep).ToColumn("cep");
            Map(m => m.Descricao).ToColumn("descricao");
            Map(m => m.Ativo).ToColumn("ativo");
            Map(m => m.Empresa).Ignore();
        }
    }
}
