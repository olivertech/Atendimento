using Atendimento.Entities.Entities;
using Dapper.FluentMap.Dommel.Mapping;

namespace Atendimento.Repository.Mapping
{
    public class UsuarioClienteMapping : DommelEntityMap<UsuarioCliente>
    {
        public UsuarioClienteMapping()
        {
            ToTable("Usuario_Cliente");
            Map(m => m.Id).ToColumn("id_usuario_cliente_entity").IsKey().IsIdentity(); //manter id_usuario_cliente em função dos joins
            Map(m => m.IdCliente).ToColumn("clienteid");
            Map(m => m.Nome).ToColumn("nome");
            Map(m => m.Username).ToColumn("username");
            Map(m => m.Password).ToColumn("password");
            Map(m => m.Email).ToColumn("email");
            Map(m => m.TelefoneFixo).ToColumn("telefone_fixo");
            Map(m => m.TelefoneCelular).ToColumn("telefone_celular");
            Map(m => m.Copia).ToColumn("copia");
            Map(m => m.Provisorio).ToColumn("provisorio");
            Map(m => m.Ativo).ToColumn("ativo");
            Map(m => m.Cliente).Ignore();
        }
    }
}
