using Atendimento.Entities.Entities;
using Dapper.FluentMap.Dommel.Mapping;

namespace Atendimento.Repository.Mapping
{
    public class UsuarioClienteMapping : DommelEntityMap<UsuarioCliente>
    {
        public UsuarioClienteMapping()
        {
            ToTable("Usuario_Cliente");
            Map(m => m.Id).ToColumn("id").IsKey().IsIdentity();
            Map(m => m.IdCliente).ToColumn("id_cliente");
            Map(m => m.Nome).ToColumn("nome");
            Map(m => m.Username).ToColumn("username");
            Map(m => m.Password).ToColumn("password");
            Map(m => m.Email).ToColumn("email");
            Map(m => m.Telefone).ToColumn("telefone");
            Map(m => m.Copia).ToColumn("copia");
            Map(m => m.Provisorio).ToColumn("provisorio");
            Map(m => m.Ativo).ToColumn("ativo");
            Map(m => m.Cliente).Ignore();
        }
    }
}
