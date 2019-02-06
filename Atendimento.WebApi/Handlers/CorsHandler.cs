using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Atendimento.WebApi.Handlers
{
    /// <summary>
    /// Classe que trata o problema de CORS abaixo:
    /// 
    /// Access to XMLHttpRequest at 'http://localhost:8080/Login/Authenticate?timestamp=1549069169032' from origin 'http://localhost:8081' has been blocked by CORS policy: Response to preflight request doesn't pass access control check: It does not have HTTP ok status.
    /// 
    /// Essa dica foi recuparada no site https://forums.asp.net/t/2107098.aspx?Response+to+preflight+request+doesn+t+pass+access+control+check
    /// 
    /// </summary>
    public class CorsHandler : DelegatingHandler
    {
        const string Origin = "Origin";
        const string AccessControlRequestMethod = "Access-Control-Request-Method";
        const string AccessControlRequestHeaders = "Access-Control-Request-Headers";
        const string AccessControlAllowOrigin = "Access-Control-Allow-Origin";
        const string AccessControlAllowMethods = "Access-Control-Allow-Methods";
        const string AccessControlAllowHeaders = "Access-Control-Allow-Headers";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var isCorsRequest = request.Headers.Contains(Origin);
            var isPreflightRequest = request.Method == HttpMethod.Options;

            if (isCorsRequest)
            {
                if (isPreflightRequest)
                {
                    return Task.Factory.StartNew<HttpResponseMessage>(() =>
                    {
                        var response = new HttpResponseMessage(HttpStatusCode.OK);

                        response.Headers.Add(AccessControlAllowOrigin, request.Headers.GetValues(Origin).First());

                        var accessControlRequestMethod = request.Headers.GetValues(AccessControlRequestMethod).FirstOrDefault();

                        if (accessControlRequestMethod != null)
                        {
                            response.Headers.Add(AccessControlAllowMethods, accessControlRequestMethod);
                        }

                        var requestedHeaders = string.Join(", ", request.Headers.GetValues(AccessControlRequestHeaders));

                        if (!string.IsNullOrEmpty(requestedHeaders))
                        {
                            response.Headers.Add(AccessControlAllowHeaders, requestedHeaders);
                        }

                        return response;
                    }, cancellationToken);
                }
                else
                {
                    return base.SendAsync(request, cancellationToken).ContinueWith<HttpResponseMessage>(t =>
                    {
                        var resp = t.Result;
                        resp.Headers.Add(AccessControlAllowOrigin, request.Headers.GetValues(Origin).First());
                        return resp;
                    });
                }
            }
            else
            {
                return base.SendAsync(request, cancellationToken);
            }
        }
    }
}