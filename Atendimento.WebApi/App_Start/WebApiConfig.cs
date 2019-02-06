using System.Web.Http;
using System.Web.Http.Cors;
using Atendimento.WebApi.Handlers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Atendimento.WebApi
{
    /// <summary>
    /// WebApi Config
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// Register the webapi configurations
        /// </summary>
        /// <param name="config"></param>
        public static void Register(HttpConfiguration config)
        {
            //==================================================================================
            //Configuração que habilita o acesso de outros domínios aos serviços, evitando
            //o erro de CORS
            config.MessageHandlers.Add(new CorsHandler());
            //==================================================================================

            //===================================================================================
            //Configurações necessárias para retornar o resultado dos requests dos serviços na 
            //notação CamelCase, de acordo com o padrão dos serviços webapi.
            var settings = config.Formatters.JsonFormatter.SerializerSettings;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            settings.Formatting = Formatting.Indented;
            //===================================================================================

            //===================================================================================
            //Essa configuração habilita o uso de rotas através de atributos data annotation
            config.MapHttpAttributeRoutes();
            //===================================================================================

            //==================================================================================
            //Esse handler intercepta todas as requisições feitas aos serviços
            //e permite validar se são requisições autenticadas, mediante o envio
            //de um token gerado no momento do login
            config.MessageHandlers.Add(new TokenValidationHandler());
            //==================================================================================

            //==================================================================================
            //Confiração do Swagger
            SwaggerConfig.Register();
            //==================================================================================

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
