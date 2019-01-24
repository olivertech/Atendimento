using System;
using Atendimento.Business.Interfaces.Interfaces;
using Atendimento.Entities.Entities;
using Atendimento.Entities.Enums;
using Atendimento.Entities.Responses;
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
            LoginResponse response = null;

            var atendenteEmpresa = (AtendenteEmpresa)null;
            var usuarioCliente = (UsuarioCliente)null;

            switch (userLogin.UserType)
            {
                case Tipos.Login.Atendimento:
                    atendenteEmpresa = LoginAtendente(userLogin);

                    if (atendenteEmpresa != null)
                    {
                        response = new LoginResponse
                        {
                            Usuario = atendenteEmpresa,
                            TipoUsuario = nameof(Atendimento)
                        };
                    }

                    break;
                case Tipos.Login.Cliente:
                    usuarioCliente = LoginCliente(userLogin);

                    if (usuarioCliente != null)
                    {
                        response = new LoginResponse
                        {
                            Usuario = usuarioCliente,
                            TipoUsuario = nameof(Cliente)
                        };
                    }

                    break;
                default:
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
            var email = userLogin.Email.ToLower().Trim();
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

                            response = new RecoverResponse
                            {
                                NomeUsuario = atendenteEmpresa.Nome
                            };
                        }


                        break;
                    case Tipos.Login.Cliente:

                        usuarioCliente = FindUsuarioCliente(userLogin);

                        if (usuarioCliente != null)
                        {
                            userLogin.Nome = usuarioCliente.Nome;
                            userLogin.Password = usuarioCliente.Password;
                            EnviarEmailRecover(userLogin);

                            response = new RecoverResponse
                            {
                                NomeUsuario = usuarioCliente.Nome
                            };
                        }

                        break;
                    default:
                        break;
                }

                return response;
            }
            catch (Exception)
            {
                throw;
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
            Emailer.EnviarEmailRecover(userLogin);
        }

        #endregion
    }
}
