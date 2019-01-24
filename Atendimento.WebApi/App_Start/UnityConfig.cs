using System.Web.Http;
using System.Web.Mvc;
using Atendimento.Business.Classes;
using Atendimento.Business.Interfaces.Interfaces;
using Atendimento.Repository.Interfaces.Interfaces;
using Atendimento.Repository.Repositories;
using Unity;

namespace Atendimento.WebApi
{
    /// <summary>
    /// Unity IoC Configuration
    /// </summary>
    public static class UnityConfig
    {
        /// <summary>
        /// Unity Register
        /// </summary>
        //public static void RegisterComponents(IMapper mapper)
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            //GENÉRICO - TODO : TENTAR MIGRAR TODAS AS CLASSES BUSINESS E REPOSITORY PARA CLASSES GENERICAS
            //container.RegisterType(typeof(IBusiness<>), typeof(Business<>));
            //container.RegisterType(typeof(IRepository<>), typeof(RepositoryBase<>));

            //LOGIN
            container.RegisterType<ILoginRepository, LoginRepository>();
            container.RegisterType<ILoginBusiness, LoginBusiness>();

            // CATEGORIA
            container.RegisterType<ICategoriaRepository, CategoriaRepository>();
            container.RegisterType<ICategoriaBusiness, CategoriaBusiness>();

            // CLASSIFICACAO
            container.RegisterType<IClassificacaoRepository, ClassificacaoRepository>();
            container.RegisterType<IClassificacaoBusiness, ClassificacaoBusiness>();

            // STATUS TICKET
            container.RegisterType<IStatusTicketRepository, StatusTicketRepository>();
            container.RegisterType<IStatusTicketBusiness, StatusTicketBusiness>();

            // EMPRESA
            container.RegisterType<IEmpresaRepository, EmpresaRepository>();
            container.RegisterType<IEmpresaBusiness, EmpresaBusiness>();

            // ANTENDENTE EMPRESA
            container.RegisterType<IAtendenteEmpresaRepository, AtendenteEmpresaRepository>();
            container.RegisterType<IAtendenteEmpresaBusiness, AtendenteEmpresaBusiness>();

            // CLIENTE
            container.RegisterType<IClienteRepository, ClienteRepository>();
            container.RegisterType<IClienteBusiness, ClienteBusiness>();

            // USUARIO CLIENTE
            container.RegisterType<IUsuarioClienteRepository, UsuarioClienteRepository>();
            container.RegisterType<IUsuarioClienteBusiness, UsuarioClienteBusiness>();

            // TICKET
            container.RegisterType<ITicketRepository, TicketRepository>();
            container.RegisterType<ITicketBusiness, TicketBusiness>();

            // TICKET MENSAGEM
            container.RegisterType<ITicketMensagemRepository, TicketMensagemRepository>();
            container.RegisterType<ITicketMensagemBusiness, TicketMensagemBusiness>();

            // ANEXO
            container.RegisterType<IAnexoRepository, AnexoRepository>();
            container.RegisterType<IAnexoBusiness, AnexoBusiness>();

            // TEMPLATE RESPOSTA
            container.RegisterType<ITemplateRespostaRepository, TemplateRespostaRepository>();
            container.RegisterType<ITemplateRespostaBusiness, TemplateRespostaBusiness>();

            //container.RegisterInstance(mapper);

            DependencyResolver.SetResolver(new Unity.Mvc5.UnityDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
        }
    }
}