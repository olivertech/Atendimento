using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.IdentityModel.Tokens;

namespace Atendimento.WebApi.Handlers
{
    /// <summary>
    /// Classe instanciada no WebApiConfig para interceptar os requests
    /// feitos aos serviços e validar se as requisições estão
    /// sendo enviadas com o token de autenticação e se o mesmo
    /// é um token válido
    /// </summary>
    public class TokenValidationHandler : DelegatingHandler
    {
        /// <summary>
        /// SendAsync
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpStatusCode statusCode;
            string token;

            //determine whether a jwt exists or not
            if (!TryRetrieveToken(request, out token))
            {
                statusCode = HttpStatusCode.Unauthorized;
                //allow requests with no token - whether a action method needs an authentication can be set with the claimsauthorization attribute
                return base.SendAsync(request, cancellationToken);
            }

            try
            {
                string key = ConfigurationManager.AppSettings.Get("SecretKey");
                //var now = DateTime.UtcNow;
                var securityKey = new SymmetricSecurityKey(Convert.FromBase64String(key));

                SecurityToken securityToken;
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

                //Validation parameters
                TokenValidationParameters validationParameters = new TokenValidationParameters()
                {
                    ValidAudience = ConfigurationManager.AppSettings.Get("UrlScope"),
                    ValidIssuer = ConfigurationManager.AppSettings.Get("UrlScope"),
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    LifetimeValidator = LifetimeValidator,
                    IssuerSigningKey = securityKey
                };

                //extract and assign the user of the jwt
                Thread.CurrentPrincipal = handler.ValidateToken(token, validationParameters, out securityToken);
                HttpContext.Current.User = handler.ValidateToken(token, validationParameters, out securityToken);

                return base.SendAsync(request, cancellationToken);
            }
            catch (SecurityTokenValidationException)
            {
                statusCode = HttpStatusCode.Unauthorized;
            }
            catch (Exception)
            {
                statusCode = HttpStatusCode.InternalServerError;
            }

            return Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(statusCode) { });
        }

        /// <summary>
        /// LifetimeValidator
        /// </summary>
        /// <param name="notBefore"></param>
        /// <param name="expires"></param>
        /// <param name="securityToken"></param>
        /// <param name="validationParameters"></param>
        /// <returns></returns>
        public bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            if (expires != null)
            {
                if (DateTime.UtcNow < expires) return true;
            }

            return false;
        }

        private static bool TryRetrieveToken(HttpRequestMessage request, out string token)
        {
            token = null;
            IEnumerable<string> authzHeaders;

            if (!request.Headers.TryGetValues("Authorization", out authzHeaders) || authzHeaders.Count() > 1)
            {
                return false;
            }

            var bearerToken = authzHeaders.ElementAt(0);
            token = bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;

            return true;
        }
    }
}