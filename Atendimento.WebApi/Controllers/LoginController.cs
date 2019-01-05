using System;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Cors;
using Atendimento.Business.Interfaces.Interfaces;
using Atendimento.Entities.Entities;
using Atendimento.Entities.Enums;
using Atendimento.Entities.Requests;
using Atendimento.Entities.Responses;
using Microsoft.IdentityModel.Tokens;

namespace Atendimento.WebApi.Controllers
{
    /// <summary>
    /// Controller para autenticação
    /// </summary>
    [RoutePrefix("api/Login")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class LoginController : ApiController
    {
        private readonly ILoginBusiness _business;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="business"></param>
        public LoginController(ILoginBusiness business)
        {
            _business = business;
        }

        /// <summary>
        /// Action de autenticação chamada no processo de login,
        /// que cria e devolve uma token de autenticação que
        /// deverá ser usada em todas as requisições dos serviços
        /// </summary>
        /// <param name="loginRequest">Dados do usuário para autenticar o login</param>
        /// <returns></returns>
        [Route("Authenticate")]
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult Authenticate([FromBody] LoginRequest loginRequest)
        {
            HttpResponseMessage responseMsg = new HttpResponseMessage();
            IHttpActionResult result;

            try
            {
                //Se os dados do login estão null, retornar BadRequest
                if (loginRequest == null)
                    return BadRequest("Dados inválidos");

                UserLogin userLogin = new UserLogin { UserName = loginRequest.Username, Password = loginRequest.Password, UserType = loginRequest.UserType };

                //Recupera o usuário de atendimento
                var loginResponse = _business.DoLogin(userLogin);

                //Se o usuário não foi encontrado com base nos dados do login, retornar Unauthorized
                if (loginResponse == null)
                {
                    var msg = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "Login não autorizado." };
                    throw new HttpResponseException(msg);
                }

                //Cria a token de autenticação e autorização
                if (loginRequest.UserType == Tipos.Login.Atendimento)
                    loginResponse.Token = CreateToken(loginRequest.Username.ToLower(), ((AtendenteEmpresa)loginResponse.Usuario).Email);
                else
                    loginResponse.Token = CreateToken(loginRequest.Username.ToLower(), ((UsuarioCliente)loginResponse.Usuario).Email);

                //Monta response
                result = Ok(Retorno<LoginResponse>.Criar(true, "Acesso Autorizado", loginResponse));

                //Retorna o response com o token
                return result;

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Action de recuperação de senha
        /// </summary>
        /// <param name="recoverRequest">Dados do usuário para recuperar a senha</param>
        /// <returns></returns>
        [Route("Recover")]
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult DoRecover([FromBody] RecoverRequest recoverRequest)
        {
            HttpResponseMessage responseMsg = new HttpResponseMessage();
            IHttpActionResult result;

            try
            {
                //Se o email enviado é null, retornar BadRequest
                if (recoverRequest == null)
                    return BadRequest("Email inválido");

                UserLogin userLogin = new UserLogin { Email = recoverRequest.Email, UserType = recoverRequest.UserType };

                //Recupera o usuário de atendimento
                RecoverResponse recoverResponse = _business.DoRecover(userLogin);
                
                //Monta response
                result = Ok(Retorno<RecoverResponse>.Criar(true, "Recuperação de Senha Realizada Com Sucesso", recoverResponse));

                //Retorna o response com o token
                return result;

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        #region Métodos Privados

        /// <summary>
        /// Metodo privado que cria o token de autenticação que deverá ser 
        /// enviado em todas as requisições feitas aos serviços
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        private string CreateToken(string userName, string email)
        {
            //==========================
            // PREPARE TOKEN PARAMETERS
            //==========================
            //Set issued at date
            DateTime issuedAt = DateTime.UtcNow;
            DateTime expires = DateTime.UtcNow;

            try
            {
                double result = double.Parse(ConfigurationManager.AppSettings["TokenExpirationTimeInDays"].ToString());

                //set the time when it expires
                expires = DateTime.UtcNow.AddDays(result);
            }
            catch (Exception)
            {
                expires = DateTime.UtcNow.AddDays(1);
            }

            //create a identity and add claims to the user which we want to log in
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Email, email)
            });

            // * Código usado pra gerar a chave abaixo:
            // *  var hmac = new HMACSHA256();
            // *  var key1 = Convert.ToBase64String(hmac.Key);
            string key = ConfigurationManager.AppSettings.Get("SecretKey");

            var securityKey = new SymmetricSecurityKey(Convert.FromBase64String(key));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            //======================================================================
            //CREATE JSON WEB TOKEN - ATTENTION >>> CHANGE UrlScope to Production
            //======================================================================
            var tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = tokenHandler.CreateJwtSecurityToken(issuer: ConfigurationManager.AppSettings.Get("UrlScope"),
                                                                         audience: ConfigurationManager.AppSettings.Get("UrlScope"),
                                                                         subject: claimsIdentity,
                                                                         notBefore: issuedAt,
                                                                         expires: expires,
                                                                         signingCredentials: signingCredentials);

            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        #endregion
    }
}
