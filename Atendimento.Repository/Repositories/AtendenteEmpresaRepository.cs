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
    public class AtendenteEmpresaRepository : RepositoryBase<AtendenteEmpresa>, IAtendenteEmpresaRepository
    {
        const string selectQuery = @"SELECT 
                                        ae.id,
                                        ae.id_empresa,
                                        ae.nome,
                                        ae.username,
                                        ae.password,
                                        ae.email,
                                        ae.telefone_fixo,
                                        ae.telefone_celular,
                                        ae.copia,
                                        ae.provisorio,
                                        ae.ativo,
                                        ae.is_admin,
                                        ae.is_default,
                                        e.id                AS id_empresa,
                                        e.nome,
                                        e.email,
                                        e.telefone_fixo,
                                        e.telefone_celular,
                                        e.descricao,
                                        e.is_default
                                    FROM Atendente_Empresa ae 
                                    INNER JOIN Empresa e ON e.id = ae.id_empresa ";

        /// <summary>
        /// Método que retorna todos os atendentes de uma determinada empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public IEnumerable<AtendenteEmpresa> GetAll(int idEmpresa)
        {
            IEnumerable<AtendenteEmpresa> result;
            var sql = string.Empty;

            try
            {
                using (var conn = CreateConnection())
                {
                    result = conn.Select<AtendenteEmpresa>(q => q.IdEmpresa == idEmpresa);
                }

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Método que subescreve o GetAll para retornar os atendentes com a subclasse Empresa populada
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<AtendenteEmpresa> GetAll()
        {
            IEnumerable<AtendenteEmpresa> result;

            try
            {
                using (var conn = CreateConnection())
                {
                    result = conn.Query<AtendenteEmpresa, Empresa, AtendenteEmpresa>(selectQuery,
                                map: (atendente, empresa) =>
                                {
                                    atendente.Empresa = empresa;

                                    return atendente;
                                },
                                splitOn: "id_empresa").Distinct().ToList();

                    return result;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Recupera os atendentes de forma paginada
        /// </summary>
        /// <param name="advancedFilter"></param>
        /// <returns></returns>
        public AtendentesEmpresaResponse GetAllPaged(FilterAtendenteEmpresaRequest advancedFilter)
        {
            var result = new AtendentesEmpresaResponse();
            var sql = string.Empty;
            var sqlCount = string.Empty;

            try
            {
                using (var conn = CreateConnection())
                {
                    result.TotalGeral = (int)conn.ExecuteScalar("SELECT COUNT(*) FROM Atendente_Empresa");

                    sql = RecuperarQuery(advancedFilter);

                    result.Atendentes = conn.Query<AtendenteEmpresa, Empresa, AtendenteEmpresa>(selectQuery,
                                            map: (atendente, empresa) =>
                                            {
                                                atendente.Empresa = empresa;

                                                return atendente;
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
        private static string RecuperarQuery(FilterAtendenteEmpresaRequest advancedFilter)
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

        /// <summary>
        /// Monta a clausula Order By
        /// </summary>
        /// <returns></returns>
        private static string BuildOrderBy()
        {
            var orderByClause = new StringBuilder();

            orderByClause.Append("ORDER BY ID ASC");

            return orderByClause.ToString();
        }

        /// <summary>
        /// Monta as clausulas Offset e Fetch Next
        /// </summary>
        /// <param name="advancedFilter"></param>
        /// <returns></returns>
        private static string BuildOffSetNumRows(FilterAtendenteEmpresaRequest advancedFilter)
        {
            if (advancedFilter.NumRows == 0) return string.Empty;

            var offSetNumRowsClauses = new StringBuilder();

            offSetNumRowsClauses.Append(" OFFSET " + advancedFilter.OffSet + " ROWS");
            offSetNumRowsClauses.Append(" FETCH NEXT " + advancedFilter.NumRows + " ROWS ONLY");

            return offSetNumRowsClauses.ToString();
        }

        /// <summary>
        /// Método que recupera o atendente através do seu username e senha.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public AtendenteEmpresa GetByUsernameAndPassword(string username, string password)
        {
            AtendenteEmpresa result;

            try
            {
                using (var conn = CreateConnection())
                {
                    result = conn.Select<AtendenteEmpresa>(q => q.Username == username && q.Password == password && q.Ativo).SingleOrDefault();
                }

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Método que atualiza apenas a password do atendente
        /// </summary>
        /// <param name="atendente"></param>
        /// <returns></returns>
        public bool UpdatePassword(AtendenteEmpresa atendente)
        {
            try
            {
                using (var conn = CreateConnection())
                {
                    return conn.Update(atendente);
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
