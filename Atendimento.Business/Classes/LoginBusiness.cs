using System;
using System.Configuration;
using Atendimento.Business.Interfaces.Interfaces;
using Atendimento.Entities.Entities;
using Atendimento.Entities.Enums;
using Atendimento.Entities.Responses;
using Atendimento.Infra;
using Atendimento.Repository.Interfaces.Interfaces;

namespace Atendimento.Business.Classes
{
    public class LoginBusiness : ILoginBusiness
    {
        private ILoginRepository _repository;

        public LoginBusiness(ILoginRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Método que realiza o login e retorna o token de autenticação
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        public LoginResponse DoLogin(UserLogin userLogin)
        {
            LoginResponse response = new LoginResponse();

            var atendenteEmpresa = (AtendenteEmpresa)null;
            var usuarioCliente = (UsuarioCliente)null;

            switch (userLogin.UserType)
            {
                case Tipos.Login.Atendimento:
                    atendenteEmpresa = LoginAtendente(userLogin);

                    if (atendenteEmpresa != null)
                    {
                        response.Usuario = atendenteEmpresa;
                        response.TipoUsuario = "Atendimento";
                    }

                    break;
                case Tipos.Login.Cliente:
                    usuarioCliente = LoginCliente(userLogin);

                    if (usuarioCliente != null)
                    {
                        response.Usuario = usuarioCliente;
                        response.TipoUsuario = "Cliente";
                    }

                    break;
            }

            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userLogin"></param>
        public RecoverResponse DoRecover(UserLogin userLogin)
        {
            string email = userLogin.Email.ToLower().Trim();

            var atendenteEmpresa = (AtendenteEmpresa)null;
            var usuarioCliente = (UsuarioCliente)null;

            RecoverResponse response = null;

            try
            {
                switch (userLogin.UserType)
                {
                    case Tipos.Login.Atendimento:

                        atendenteEmpresa = FindAtendenteEmpresa(userLogin);

                        if (atendenteEmpresa != null)
                        {
                            userLogin.Nome = atendenteEmpresa.Nome;
                            userLogin.Password = atendenteEmpresa.Password;
                            EnviarEmailRecover(userLogin);

                            response = new RecoverResponse();
                            response.NomeUsuario = atendenteEmpresa.Nome;
                        }


                        break;
                    case Tipos.Login.Cliente:

                        usuarioCliente = FindUsuarioCliente(userLogin);

                        if (usuarioCliente != null)
                        {
                            userLogin.Nome = usuarioCliente.Nome;
                            userLogin.Password = usuarioCliente.Password;
                            EnviarEmailRecover(userLogin);

                            response = new RecoverResponse();
                            response.NomeUsuario = usuarioCliente.Nome;
                        }

                        break;
                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Métodos Privados

        private AtendenteEmpresa LoginAtendente(UserLogin userLogin)
        {
            return _repository.LoginAtendente(userLogin);
        }

        private UsuarioCliente LoginCliente(UserLogin userLogin)
        {
            return _repository.LoginCliente(userLogin);
        }

        private AtendenteEmpresa FindAtendenteEmpresa(UserLogin userLogin)
        {
            return _repository.FindAtendenteEmpresaByEmail(userLogin);
        }

        private UsuarioCliente FindUsuarioCliente(UserLogin userLogin)
        {
            return _repository.FindUsuarioClienteByEmail(userLogin);
        }

        private void EnviarEmailRecover(UserLogin userLogin)
        {
            string subject = string.Empty;
            string emailFrom = string.Empty;
            string emailTo = string.Empty;
            string emailReply = string.Empty;

            try
            {
                subject = "Senha de acesso ao sistema de atendimento Algorix";
                emailFrom = ConfigurationManager.AppSettings["EmailSuporte"].ToString();
                emailTo = userLogin.Email;

                Email.SendNetEmail(emailFrom, emailTo, emailReply, Email.FormatarCorpoEmailEsqueciSenha(userLogin), subject, false, System.Net.Mail.MailPriority.High);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
