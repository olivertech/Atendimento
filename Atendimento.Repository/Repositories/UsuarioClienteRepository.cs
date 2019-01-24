using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atendimento.Entities.Entities;
using Atendimento.Entities.Requests;
using Atendimento.Entities.Responses;
using Atendimento.Repository.Interfaces.Interfaces;
using Atendimento.Repository.Repositories.Base;
using Dapper;
using Dommel;

namespace Atendimento.Repository.Repositories
{
    public class UsuarioClienteRepository : RepositoryBase<UsuarioCliente>, IUsuarioClienteRepository
    {
        const string selectQuery = @"SELECT 
                                        uc.id,
                                        uc.id_cliente,
                                        uc.nome,
                                        uc.username,
                                        uc.password,
                                        uc.email,
                                        uc.telefone_fixo,
                                        uc.telefone_celular,
                                        uc.copia,
                                        uc.provisorio,
                                        uc.ativo,
                                        c.id                AS id_cliente,
                                        c.nome,
                                        c.email
                                    FROM Usuario_Cliente uc 
                                    INNER JOIN Cliente c ON c.id = uc.id_cliente ";


        /// <summary>
        /// Método que sobrescreve o base, por conta no alias dado a coluna Id da tabela,
        /// em função dos joins do Dapper, que exigem campos chaves com nomes diferentes
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public override UsuarioCliente GetById(Int32 idUsuario)
        {
            UsuarioCliente result = null;

            try
            {
                using (var conn = CreateConnection())
                {
                    result = conn.Query<UsuarioCliente>(@"SELECT
                                                                id AS id_usuario_cliente,
                                                                id_cliente,
                                                                nome,
                                                                username,
                                                                password,
                                                                email,
                                                                telefone_fixo,
                                                                telefone_celular,
                                                                copia,
                                                                provisorio,
                                                                ativo
                                                          FROM Usuario_Cliente WHERE id = " + idUsuario).SingleOrDefault();
                }

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Método que sobreescreve o base, por conta da necessidade de mudar o nome do id da tabela no
        /// mapeamento pra atender aos joins do Dapper. Por conta disso, é necessário fazer as consultas
        /// de forma direta, usando o nome original do atribuito chave
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override bool Delete(Int32 id)
        {
            var rows = 0;

            try
            {
                using (var conn = CreateConnection())
                {
                    var entity = GetById(id);

                    if (entity == null) throw new Exception("Registro não encontrado");

                    rows = conn.Execute("DELETE FROM Usuario_Cliente WHERE id = @Id", new { Id = id });
                }

                return rows > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Método que recupera todos os usuários associados a um cliente
        /// </summary>
        /// <param name="idCliente"></param>
        /// <returns></returns>
        public IEnumerable<UsuarioCliente> GetAllById(int idCliente)
        {
            List<UsuarioCliente> result = null;
            var sql = string.Empty;

            try
            {
                using (var conn = CreateConnection())
                {
                    result = conn.Select<UsuarioCliente>(q => q.IdCliente == idCliente).ToList();
                }

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Método que recupera o usuario através do seu username e senha.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public UsuarioCliente GetByUsernameAndPassword(string username, string password)
        {
            UsuarioCliente result;

            try
            {
                using (var conn = CreateConnection())
                {
                    result = conn.Select<UsuarioCliente>(q => q.Username == username && q.Password == password && q.Ativo).SingleOrDefault();
                }

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Método que atualiza apenas a senha do usuario
        /// </summary>
        /// <param name="usuarioCliente"></param>
        /// <returns></returns>
        public bool UpdatePassword(UsuarioCliente usuarioCliente)
        {
            try
            {
                using (var conn = CreateConnection())
                {
                    return conn.Update(usuarioCliente);
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Método que retorna o total de registros de usuários associados a um cliente
        /// </summary>
        /// <param name="idCliente"></param>
        /// <returns></returns>
        public int GetCount(int idCliente)
        {
            var result = 0;
            var sql = string.Empty;

            try
            {
                using (var conn = CreateConnection())
                {
                    result = conn.Select<UsuarioCliente>(q => q.IdCliente == idCliente).Count();
                }

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Recupera os usuarios de forma paginada
        /// </summary>
        /// <param name="advancedFilter"></param>
        /// <returns></returns>
        public UsuariosClienteResponse GetAllPaged(FilterUsuarioRequest advancedFilter)
        {
            var result = new UsuariosClienteResponse();
            var sql = string.Empty;
            var sqlCount = string.Empty;

            try
            {
                using (var conn = CreateConnection())
                {
                    result.TotalGeral = (int)conn.ExecuteScalar("SELECT COUNT(*) FROM Usuario_Cliente");

                    sql = RecuperarQuery(advancedFilter);

                    result.Usuarios = conn.Query<UsuarioCliente, Cliente, UsuarioCliente>(sql,
                                            map: (usuario, cliente) =>
                                            {
                                                usuario.Cliente = cliente;

                                                return usuario;
                                            },
                                            splitOn: "id_cliente").Distinct().ToList();
                }

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Monta a query
        /// </summary>
        /// <param name="advancedFilter"></param>
        /// <returns></returns>
        private static string RecuperarQuery(FilterUsuarioRequest advancedFilter)
        {
            var sql = new StringBuilder();
            var selectClause = string.Empty;
            var orderByClause = string.Empty;
            var offSetNumRows = string.Empty;

            selectClause = selectQuery;
            orderByClause = BuildOrderBy(advancedFilter);
            offSetNumRows = BuildOffSetNumRows(advancedFilter);

            sql.Append(selectClause);
            sql.Append(orderByClause);
            sql.Append(offSetNumRows);

            return sql.ToString();
        }

        /// <summary>
        /// Monta a clausula Order By
        /// </summary>
        /// <param name="advancedFilter"></param>
        /// <returns></returns>
        private static string BuildOrderBy(FilterUsuarioRequest advancedFilter)
        {
            var orderByClause = new StringBuilder();

            orderByClause.Append("ORDER BY uc." + advancedFilter.OrderBy + " " + advancedFilter.Direction);

            return orderByClause.ToString();
        }

        /// <summary>
        /// Monta as clausulas Offset e Fetch Next
        /// </summary>
        /// <param name="advancedFilter"></param>
        /// <returns></returns>
        private static string BuildOffSetNumRows(FilterUsuarioRequest advancedFilter)
        {
            if (advancedFilter.NumRows == 0) return string.Empty;

            var offSetNumRowsClauses = new StringBuilder();

            offSetNumRowsClauses.Append(" OFFSET " + advancedFilter.OffSet + " ROWS");
            offSetNumRowsClauses.Append(" FETCH NEXT " + advancedFilter.NumRows + " ROWS ONLY");

            return offSetNumRowsClauses.ToString();
        }
    }
}
