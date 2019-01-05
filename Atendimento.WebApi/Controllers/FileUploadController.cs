using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using Atendimento.Business.Classes;
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

            //var httpRequest = HttpContext.Current.Request;
            var provider = new CustomMultipartFormDataStreamProvider(pathAnexosUsuario);

            return await Request.Content.ReadAsMultipartAsync(provider).ContinueWith<HttpResponseMessage>(t =>
            {
                if (t.IsFaulted || t.IsCanceled)
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, t.Exception);

                //FileUploadResponse fileUploadResponse = new FileUploadResponse();

                //if (httpRequest.Files.Count > 0)
                //{
                //    foreach (string file in httpRequest.Files)
                //    {
                //        //Salvo o arquivo anexo
                //        var postedFile = httpRequest.Files[file];
                //        string fileName = postedFile.FileName;

                //        //Comprimo o arquivo e salvo ele
                //        string zipName = Arquivo.Compress(pathAnexosZip, pathAnexosUsuario, idUsuario, fileName);

                //        Anexo anexo = new Anexo
                //        {
                //            Nome = zipName
                //        };

                //        //Guarda anexo no banco de dados
                //        _anexoBusiness.Insert(ref anexo);

                //        fileUploadResponse.AnexoResponse = Mapper.Map<Anexo, AnexoResponse>(anexo);
                //    }
                //}

                return Request.CreateResponse<string>(HttpStatusCode.OK, pathAnexosUsuario);
            });
        }
    }
}