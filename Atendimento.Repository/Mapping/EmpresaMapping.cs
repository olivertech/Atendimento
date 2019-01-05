using Atendimento.Entities.Entities;
using Dapper.FluentMap.Dommel.Mapping;

namespace Atendimento.Repository.Mapping
{
    public class EmpresaMapping : DommelEntityMap<Empresa>
    {
        public EmpresaMapping()
        {
            ToTable("Empresa");
            Map(m => m.Id).ToColumn("id").IsKey().IsIdentity();
            Map(m => m.IdGrupo).ToColumn("id_grupo");
            Map(m => m.Nome).ToColumn("nome");
            Map(m => m.Cnpj).ToColumn("cnpj");
            Map(m => m.Email).ToColumn("email");
            Map(m => m.Telefone).ToColumn("telefone");
            Map(m => m.Endereco.Logradouro).ToColumn("logradouro");
            Map(m => m.Endereco.NumeroLogradouro).ToColumn("numero_logradouro");
            Map(m => m.Endereco.ComplementoLogradouro).ToColumn("complemento_logradouro");
            Map(m => m.Endereco.Estado).ToColumn("estado");
            Map(m => m.Endereco.Cidade).ToColumn("cidade");
            Map(m => m.Endereco.Bairro).ToColumn("bairro");
            Map(m => m.Endereco.Cep).ToColumn("cep");
            Map(m => m.Endereco.Latitude).ToColumn("latitude");
            Map(m => m.Endereco.Longitude).ToColumn("longitude");
            Map(m => m.Tipo).ToColumn("tipo");
            Map(m => m.Descricao).ToColumn("descricao");
            Map(m => m.Grupo).Ignore();
        }
    }
}
