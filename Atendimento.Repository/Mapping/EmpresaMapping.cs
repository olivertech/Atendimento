using Atendimento.Entities.Entities;
using Dapper.FluentMap.Dommel.Mapping;

namespace Atendimento.Repository.Mapping
{
    public class EmpresaMapping : DommelEntityMap<Empresa>
    {
        public EmpresaMapping()
        {
            ToTable("Empresa");
            Map(m => m.Id).ToColumn("id_empresa_entity").IsKey().IsIdentity();
            Map(m => m.Nome).ToColumn("nome");
            Map(m => m.Email).ToColumn("email");
            Map(m => m.TelefoneFixo).ToColumn("telefone_fixo");
            Map(m => m.TelefoneCelular).ToColumn("telefone_celular");
            Map(m => m.IsDefault).ToColumn("is_default");
            Map(m => m.Descricao).ToColumn("descricao");
        }
    }
}
