using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using Atendimento.WebApi.Providers;

namespace Atendimento.WebApi.Controllers
{
    /// <summary>
    /// File Upload Controller
    /// </summary>
    [RoutePrefix("api/FileUpload")]
    [Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class FileUploadController : ApiController
    {
        /// POST: api/Ticket
        [Route("Upload")]
        [HttpPost]
        public async Task<HttpResponseMessage> PostAsync(int idUsuario)
        {
            //string pathAnexosZip = HttpContext.Current.Server.MapPath("~/Anexos");
            string pathAnexosUsuario = HttpContext.Current.Server.MapPath("~/Anexos/" + idUsuario);

            //Caso a pasta com id do usuário não exista, criar pasta para receber os anexos
            if (!Directory.Exists(pathAnexosUsuario))
            {
                DirectoryInfo di = Directory.CreateDirectory(pathAnexosUsuario);
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