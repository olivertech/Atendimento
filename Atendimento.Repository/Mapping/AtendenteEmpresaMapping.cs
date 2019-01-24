using Atendimento.Entities.Entities;
using Dapper.FluentMap.Dommel.Mapping;

namespace Atendimento.Repository.Mapping
{
    public class AtendenteEmpresaMapping : DommelEntityMap<AtendenteEmpresa>
    {
        public AtendenteEmpresaMapping()
        {
            ToTable("Atendente_Empresa");
            Map(m => m.Id).ToColumn("id").IsKey().IsIdentity();
            Map(m => m.IdEmpresa).ToColumn("id_empresa");
            Map(m => m.Nome).ToColumn("nome");
            Map(m => m.Username).ToColumn("username");
            Map(m => m.Password).ToColumn("password");
            Map(m => m.Email).ToColumn("email");
            Map(m => m.TelefoneFixo).ToColumn("telefone_fixo");
            Map(m => m.TelefoneCelular).ToColumn("telefone_celular");
            Map(m => m.Ativo).ToColumn("ativo");
            Map(m => m.Copia).ToColumn("copia");
            Map(m => m.Provisorio).ToColumn("provisorio");
            Map(m => m.IsAdmin).ToColumn("is_admin");
            Map(m => m.IsDefault).ToColumn("is_default");
            Map(m => m.Empresa).Ignore();
        }
    }
}
