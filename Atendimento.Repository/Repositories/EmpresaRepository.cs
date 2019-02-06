using System;
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
    public class EmpresaRepository : RepositoryBase<Empresa>, IEmpresaRepository
    {
        const string selectQuery = @"SELECT 
                                        id, 
                                        nome,
                                        email,
                                        telefone_fixo,
                                        telefone_celular,
                                        descricao,
                                        is_default
                                    FROM Empresa ";

        public EmpresasResponse GetAllPaged(FilterEmpresaRequest advancedFilter)
        {
            var result = new EmpresasResponse();
            var sql = string.Empty;
            var sqlCount = string.Empty;

            try
            {
                using (var conn = CreateConnection())
                {
                    result.TotalGeral = (int)conn.ExecuteScalar("SELECT COUNT(*) FROM Empresa");

                    sql = RecuperarQuery(advancedFilter);

                    result.Empresas = conn.Query<Empresa>(sql).Distinct().ToList();
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
        private string RecuperarQuery(FilterEmpresaRequest advancedFilter)
        {
            var sql = new StringBuilder();
            var selectClause = string.Empty;
            var orderByClause = string.Empty;
            var offSetNumRows = string.Empty;

            selectClause = selectQuery;
            orderByClause = BuildOrderBy();
            offSetNumRows = BuildOffSetNumRows(advancedFilter);

            sql.Append(selectClause);
            sql.Append(orderByClause);
            sql.Append(offSetNumRows);

            return sql.ToString();
        }

        private static string BuildOrderBy()
        {
            var orderByClause = new StringBuilder();

            orderByClause.Append("ORDER BY id ASC");

            return orderByClause.ToString();
        }

        private static string BuildOffSetNumRows(FilterEmpresaRequest advancedFilter)
        {
            if (advancedFilter.NumRows == 0) return string.Empty;

            var offSetNumRowsClauses = new StringBuilder();

            offSetNumRowsClauses.Append(" OFFSET " + advancedFilter.OffSet + " ROWS");
            offSetNumRowsClauses.Append(" FETCH NEXT " + advancedFilter.NumRows + " ROWS ONLY");

            return offSetNumRowsClauses.ToString();
        }

        /// <summary>
        /// Método que retorna o total de atendentes associados a empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public int GetTotalAtendentesEmpresa(int idEmpresa)
        {
            var result = 0;
            var sql = string.Empty;

            try
            {
                using (var conn = CreateConnection())
                {
                    result = conn.Select<AtendenteEmpresa>(q => q.IdEmpresa == idEmpresa).Count();
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
