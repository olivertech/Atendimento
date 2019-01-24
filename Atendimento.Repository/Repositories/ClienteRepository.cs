using System;
using System.Linq;
using System.Text;
using Atendimento.Entities.Entities;
using Atendimento.Entities.Requests;
using Atendimento.Entities.Responses;
using Atendimento.Repository.Interfaces.Interfaces;
using Atendimento.Repository.Repositories.Base;
using Dapper;

namespace Atendimento.Repository.Repositories
{
    public class ClienteRepository : RepositoryBase<Cliente>, IClienteRepository
    {
        const string selectQuery = @"SELECT 
                                        c.id, 
                                        c.id_empresa,
                                        c.nome,
                                        c.cnpj,
                                        c.email,
                                        c.telefone_fixo,
                                        c.telefone_celular,
                                        c.logradouro,
                                        c.numero_logradouro,
                                        c.complemento_logradouro,
                                        c.estado,
                                        c.cidade,
                                        c.bairro,
                                        c.cep,
                                        c.descricao,
                                        c.ativo,
                                        e.id                AS id_empresa,
                                        e.nome,
                                        e.email,
                                        e.telefone_fixo,
                                        e.telefone_celular,
                                        e.descricao,
                                        e.is_default
                                    FROM Cliente c
                                    INNER JOIN Empresa e ON e.id = c.id_empresa ";

        public ClientesResponse GetAllPaged(FilterClienteRequest advancedFilter)
        {
            var result = new ClientesResponse();
            var sql = string.Empty;
            var sqlCount = string.Empty;

            try
            {
                using (var conn = CreateConnection())
                {
                    result.TotalGeral = (int)conn.ExecuteScalar("SELECT COUNT(*) FROM Cliente");

                    sql = RecuperarQuery(advancedFilter);

                    result.Clientes = conn.Query<Cliente, Empresa, Cliente>(sql,
                                            map: (cliente, empresa) =>
                                            {
                                                cliente.Empresa = empresa;

                                                return cliente;
                                            },
                                            splitOn: "id_empresa").Distinct().ToList();
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
        private string RecuperarQuery(FilterClienteRequest advancedFilter)
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
        private static string BuildOrderBy(FilterClienteRequest advancedFilter)
        {
            var orderByClause = new StringBuilder();

            orderByClause.Append("ORDER BY c." + advancedFilter.OrderBy + " " + advancedFilter.Direction);

            return orderByClause.ToString();
        }

        /// <summary>
        /// Monta as clausulas Offset e Fetch Next
        /// </summary>
        /// <param name="advancedFilter"></param>
        /// <returns></returns>
        private static string BuildOffSetNumRows(FilterClienteRequest advancedFilter)
        {
            if (advancedFilter.NumRows == 0) return string.Empty;

            var offSetNumRowsClauses = new StringBuilder();

            offSetNumRowsClauses.Append(" OFFSET " + advancedFilter.OffSet + " ROWS");
            offSetNumRowsClauses.Append(" FETCH NEXT " + advancedFilter.NumRows + " ROWS ONLY");

            return offSetNumRowsClauses.ToString();
        }
    }
}
