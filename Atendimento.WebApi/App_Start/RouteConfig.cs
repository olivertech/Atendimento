using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Atendimento.WebApi
{
    /// <summary>
    /// Route Configuration
    /// </summary>
    public class RouteConfig
    {
        /// <summary>
        /// Route Register
        /// </summary>
        /// <param name="routes"></param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //Esse comando determina que as rotas sejam feitas por atributos nas actions
            routes.MapMvcAttributeRoutes();
        }
    }
}
