using System.Web.Http.Description;
using System.Web.Mvc;
using Atendimento.Infra.Base;

namespace Atendimento.WebApi.Controllers
{
    /// <summary>
    /// Controller usado pra redirecionar para o Swagger UI
    /// </summary>
    public class DefaultController : Controller
    {
        /// <summary>
        /// Método que redireciona para o Swagger UI
        /// </summary>
        /// <returns></returns>
        [Route(""), HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        [AllowAnonymous]
        public RedirectResult RedirectToSwaggerUi()
        {
            var url = BaseUtil.RecuperarUrl();

            return Redirect(url + "/swagger/");
        }
    }
}
