using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Atendimento.Repository;
using Atendimento.WebApi.App_Start;
using AutoMapper;

namespace Atendimento.WebApi
{
    /// <summary>
    /// Global WebApi Application
    /// </summary>
    public class WebApiApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// Application Start
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            // Registrando o AutoMapper para configurar os profiles
            // de mapeamento durante a inicialização da aplicação.
            //IMapper mapper = AutoMapperConfig.RegisterMappings();
            AutoMapperConfig.RegisterMappings();

            // Registrando a injeção de dependência das classes
            // com suas respectivas interfaces
            UnityConfig.RegisterComponents();

            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Registrando o mapeamento das entidades do banco com as
            // propriedades das classes que mapeiam essas entidades
            DapperMappings.Register();
        }
    }
}
