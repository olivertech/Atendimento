using System;
using System.Linq;
using Atendimento.Entities.Entities;
using Atendimento.Repository.Interfaces.Interfaces;
using Atendimento.Repository.Repositories.Base;
using Dommel;

namespace Atendimento.Repository.Repositories
{
    public class LoginRepository : ConnectionBase, ILoginRepository
    {
        /// <summary>
        /// Valida os dados de login do atendente, buscando no banco
        /// um atendente que tenha login e senha válidos, e que esteja ativo
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public AtendenteEmpresa LoginAtendente(UserLogin login)
        {
            AtendenteEmpresa result;

            try
            {
                using (var conn = CreateConnection())
                {
                    result = conn.Select<AtendenteEmpresa>(q => q.Username == login.UserName && q.Password == login.Password && q.Ativo == true).FirstOrDefault();

                    if (result != null) { result.Password = "***"; }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Valida os dados de login do cliente, buscando no banco
        /// um atendente que tenha login e senha válidos, e que esteja ativo
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public UsuarioCliente LoginCliente(UserLogin login)
        {
            UsuarioCliente result;

            try
            {
                using (var conn = CreateConnection())
                {
                    result = conn.Select<UsuarioCliente>(q => q.Username == login.UserName && q.Password == login.Password && q.Ativo == true).FirstOrDefault();

                    if (result != null) { result.Password = "***"; }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Recupera usuario atendente por email
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public AtendenteEmpresa FindAtendenteEmpresaByEmail(UserLogin usuario)
        {
            AtendenteEmpresa result;

            try
            {
                using (var conn = CreateConnection())
                {
                    result = conn.Select<AtendenteEmpresa>(q => q.Email == usuario.Email && q.Ativo == true).FirstOrDefault();
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Recupera usuario de cliente por email
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public UsuarioCliente FindUsuarioClienteByEmail(UserLogin usuario)
        {
            UsuarioCliente result;

            try
            {
                using (var conn = CreateConnection())
                {
                    result = conn.Select<UsuarioCliente>(q => q.Email == usuario.Email && q.Ativo == true).FirstOrDefault();
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
