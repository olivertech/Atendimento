using System;
using System.Linq;
using Atendimento.Entities.Entities;
using Atendimento.Repository.Interfaces.Interfaces;
using Atendimento.Repository.Repositories.Base;
using Dapper;
using Dommel;

namespace Atendimento.Repository.Repositories
{
    public class LoginRepository : ConnectionBase, ILoginRepository
    {
        /// <summary>
        /// Valida os dados de login do atendente, buscando no banco
        /// um atendente que tenha login e senha válidos, e que esteja ativo
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public AtendenteEmpresa LoginAtendente(UserLogin login)
        {
            AtendenteEmpresa result;

            try
            {
                using (var conn = CreateConnection())
                {
                    result = conn.Select<AtendenteEmpresa>(q => q.Username == login.UserName && q.Password == login.Password && q.Ativo).FirstOrDefault();

                    if (result != null) { result.Password = "***"; }
                }

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Valida os dados de login do cliente, buscando no banco
        /// um atendente que tenha login e senha válidos, e que esteja ativo
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public UsuarioCliente LoginCliente(UserLogin login)
        {
            UsuarioCliente result;

            try
            {
                using (var conn = CreateConnection())
                {
                    var query = @"SELECT 
                                    uc.id,
                                    uc.clienteid,
                                    uc.nome,
                                    uc.username,
                                    uc.email,
                                    uc.telefone_fixo,
                                    uc.telefone_celular,
                                    uc.copia,
                                    c.id            AS id_cliente_entity,
                                    c.nome
                            FROM Usuario_Cliente uc
                            INNER JOIN Cliente c ON uc.clienteid = c.id
                            WHERE uc.username = '" + login.UserName + "' and uc.password = '" + login.Password + "' AND uc.ativo = 1";

                    result = conn.Query<UsuarioCliente, Cliente, UsuarioCliente>(query,
                                map: (usuario, cliente) =>
                                {
                                    usuario.Cliente = cliente;

                                    return usuario;
                                },
                                splitOn: "id_cliente_entity").SingleOrDefault();
                }

                return result;
            }
            catch (Exception)
            {
                throw;
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
                    result = conn.Select<AtendenteEmpresa>(q => q.Email == usuario.Email && q.Ativo).FirstOrDefault();
                }

                return result;
            }
            catch (Exception)
            {
                throw;
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
                    result = conn.Select<UsuarioCliente>(q => q.Email == usuario.Email && q.Ativo).FirstOrDefault();
                }

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
