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
    public class TemplateRespostaRepository : RepositoryBase<TemplateResposta>, ITemplateRespostaRepository
    {
        const string selectQuery = @"SELECT 
                                        id, 
                                        titulo, 
                                        descricao, 
                                        conteudo 
                                    FROM Template_Resposta ";

        /// <summary>
        /// Recupera de forma paginada os templates
        /// </summary>
        /// <param name="advancedFilter"></param>
        /// <returns></returns>
        public TemplatesResponse GetAllPaged(FilterTemplateRequest advancedFilter)
        {
            var result = new TemplatesResponse();
            var sql = string.Empty;
            var sqlCount = string.Empty;

            try
            {
                using (var conn = CreateConnection())
                {
                    result.TotalGeral = (int)conn.ExecuteScalar("SELECT COUNT(*) FROM Template_Resposta");

                    sql = RecuperarQuery(advancedFilter);

                    result.Templates = conn.Query<TemplateResposta>(sql).Distinct().ToList();
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
        private string RecuperarQuery(FilterTemplateRequest advancedFilter)
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
        private string BuildOrderBy(FilterTemplateRequest advancedFilter)
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
        private static string BuildOffSetNumRows(FilterTemplateRequest advancedFilter)
        {
            if (advancedFilter.NumRows == 0) return string.Empty;

            var offSetNumRowsClauses = new StringBuilder();

            offSetNumRowsClauses.Append(" OFFSET " + advancedFilter.OffSet + " ROWS");
            offSetNumRowsClauses.Append(" FETCH NEXT " + advancedFilter.NumRows + " ROWS ONLY");

            return offSetNumRowsClauses.ToString();
        }
    }
}
