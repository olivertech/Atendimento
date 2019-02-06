using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Atendimento.WebApi.Providers;

namespace Atendimento.WebApi.Controllers
{
    /// <summary>
    /// File Upload Controller
    /// </summary>
    [RoutePrefix("api/FileUpload")]
    [Authorize]
    public class FileUploadController : ApiController
    {
        /// <summary>
        /// Faz o upload de arquivos anexos
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        [Route("Upload")]
        [HttpPost]
        public async Task<HttpResponseMessage> PostAsync(int idUsuario)
        {
            var pathAnexosUsuario = HttpContext.Current.Server.MapPath("~/Anexos/" + idUsuario);

            //Caso a pasta com id do usuário não exista, criar pasta para receber os anexos
            if (!Directory.Exists(pathAnexosUsuario))
            {
                var di = Directory.CreateDirectory(pathAnexosUsuario);
            }

            var provider = new CustomMultipartFormDataStreamProvider(pathAnexosUsuario);

            return await Request.Content.ReadAsMultipartAsync(provider).ContinueWith<HttpResponseMessage>(t =>
            {
                if (t.IsFaulted || t.IsCanceled)
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, t.Exception);

                return Request.CreateResponse<string>(HttpStatusCode.OK, pathAnexosUsuario);
            });
        }
    }
}