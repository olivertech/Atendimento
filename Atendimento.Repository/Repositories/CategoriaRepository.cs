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
    public class CategoriaRepository : RepositoryBase<Categoria>, ICategoriaRepository
    {
        const string selectQuery = @"SELECT 
                                        id, 
                                        nome
                                    FROM Categoria ";

        /// <summary>
        /// Recupera de forma paginada as categorias
        /// </summary>
        /// <param name="advancedFilter"></param>
        /// <returns></returns>
        public CategoriasResponse GetAllPaged(FilterCategoriaRequest advancedFilter)
        {
            var result = new CategoriasResponse();
            var sql = string.Empty;
            var sqlCount = string.Empty;

            try
            {
                using (var conn = CreateConnection())
                {
                    result.TotalGeral = (int)conn.ExecuteScalar("SELECT COUNT(*) FROM Categoria");

                    sql = RecuperarQuery(advancedFilter);

                    result.Categorias = conn.Query<Categoria>(sql).Distinct().ToList();
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
        private string RecuperarQuery(FilterCategoriaRequest advancedFilter)
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
        private string BuildOrderBy(FilterCategoriaRequest advancedFilter)
        {
            var orderByClause = new StringBuilder();

            orderByClause.Append("ORDER BY " + advancedFilter.OrderBy + " " + advancedFilter.Direction);

            return orderByClause.ToString();
        }

        /// <summary>
        /// Monta as clausulas Offset e Fetch Next
        /// </summary>
        /// <param name="advancedFilter"></param>
        /// <returns></returns>
        private static string BuildOffSetNumRows(FilterCategoriaRequest advancedFilter)
        {
            if (advancedFilter.NumRows == 0) return string.Empty;

            var offSetNumRowsClauses = new StringBuilder();

            offSetNumRowsClauses.Append(" OFFSET " + advancedFilter.OffSet + " ROWS");
            offSetNumRowsClauses.Append(" FETCH NEXT " + advancedFilter.NumRows + " ROWS ONLY");

            return offSetNumRowsClauses.ToString();
        }
    }
}
