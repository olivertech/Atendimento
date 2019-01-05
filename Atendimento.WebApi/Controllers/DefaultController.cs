using System.Web.Http.Description;
using System.Web.Mvc;

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
            return Redirect("/swagger/");
        }
    }
}
