using Atendimento.Repository.Mapping;
using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;

namespace Atendimento.Repository
{
    /// <summary>
    /// Classe que registra todos os mapeamentos ORM
    /// </summary>
    public static class DapperMappings
    {
        public static void Register()
        {
            FluentMapper.Initialize(config =>
            {
                config.AddMap(new CategoriaMapping());
                config.AddMap(new ClassificacaoMapping());
                config.AddMap(new StatusTicketMapping());
                config.AddMap(new EmpresaMapping());
                config.AddMap(new AtendenteEmpresaMapping());
                config.AddMap(new ClienteMapping());
                config.AddMap(new UsuarioClienteMapping());
                config.AddMap(new TicketMapping());
                config.AddMap(new TicketMensagemMapping());
                config.AddMap(new AnexoMapping());
                config.AddMap(new TemplateRespostaMapping());
                config.ForDommel();
            });
        }
    }
}
