using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atendimento.Entities.Entities;
using Atendimento.Entities.Enums;
using Atendimento.Entities.Requests;
using Atendimento.Entities.Responses;
using Atendimento.Infra;
using Atendimento.Repository.Interfaces.Interfaces;
using Atendimento.Repository.Repositories.Base;
using Dapper;
using Dommel;

namespace Atendimento.Repository.Repositories
{
    public class TicketRepository : RepositoryBase<Ticket>, ITicketRepository
    {
        const string selectQuery = @"SELECT 
                                    t.id,
                                    t.id_status_ticket,
                                    t.id_usuario_cliente,
                                    t.id_categoria,
                                    t.id_classificacao,
                                    t.titulo,
                                    t.descricao,
                                    t.data_hora_inicial,
                                    t.data_hora_alteracao,
                                    CASE
                                        WHEN t.data_hora_ultima_mensagem IS NULL THEN t.data_hora_inicial
                                        WHEN t.data_hora_ultima_mensagem IS NOT NULL THEN t.data_hora_ultima_mensagem
                                    END AS data_hora_ultima_mensagem,
                                    t.data_hora_final,
                                    ct.id                       AS id_categoria, 
                                    ct.nome, 
                                    cl.id                       AS id_classificacao, 
                                    cl.nome,
                                    st.id                       AS id_status_ticket,
                                    st.nome,
                                    st.uso,
                                    st.em_aberto,
                                    st.ordem_em_aberto,
                                    uc.id                       AS id_usuario_cliente,
                                    uc.id_cliente,
                                    uc.nome,
                                    uc.email,
                                    uc.telefone,
                                    uc.copia,
                                    uc.provisorio,
                                    uc.ativo
                            FROM dbo.Ticket t
                            LEFT JOIN dbo.Categoria ct          ON t.id_categoria = ct.id
                            LEFT JOIN dbo.Classificacao cl      ON t.id_classificacao = cl.id
                            LEFT JOIN dbo.Status_Ticket st      ON t.id_status_ticket = st.id
                            LEFT JOIN dbo.Usuario_Cliente uc    ON t.id_usuario_cliente = uc.id 
                            WHERE 1 = 1 ";

        const string countQuery = @"SELECT COUNT(*)
                                FROM dbo.Ticket t
                                LEFT JOIN dbo.Categoria ct          ON t.id_categoria = ct.id
                                LEFT JOIN dbo.Classificacao cl      ON t.id_classificacao = cl.id
                                LEFT JOIN dbo.Status_Ticket st      ON t.id_status_ticket = st.id
                                LEFT JOIN dbo.Usuario_Cliente uc    ON t.id_usuario_cliente = uc.id 
                                WHERE 1 = 1 ";

        /// <summary>
        /// Método que sobreescreve o GetAll default do CRUD para atender as necessidades de JOIN
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<Ticket> GetAll()
        {
            IEnumerable<Ticket> result;

            try
            {
                using (var conn = CreateConnection())
                {
                    result = conn.Query<Ticket, Categoria, Classificacao, StatusTicket, UsuarioCliente, Ticket>(selectQuery,
                                map: (ticket, categoria, classificacao, status, usuario) =>
                                {
                                    ticket.Categoria = categoria;
                                    ticket.Classificacao = classificacao;
                                    ticket.StatusTicket = status;
                                    ticket.UsuarioCliente = usuario;

                                    return ticket;
                                },
                                splitOn: "id_categoria,id_classificacao,id_status_ticket,id_usuario_cliente").Distinct().ToList();

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método que sobreescreve o GetByID default do CRUD para atender as necessidades de JOIN
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TicketResponse GetByIdWithAnexos(int id)
        {
            TicketResponse response = new TicketResponse();
            Ticket result1;
            List<Anexo> result2;

            string sql1 = @"SELECT 
                                    t.id,
                                    t.id_status_ticket,
                                    t.id_usuario_cliente,
                                    t.id_categoria,
                                    t.id_classificacao,
                                    t.titulo,
                                    t.descricao,
                                    t.data_hora_inicial,
                                    t.data_hora_alteracao,
                                    CASE
                                        WHEN t.data_hora_ultima_mensagem IS NULL THEN t.data_hora_inicial
                                        WHEN t.data_hora_ultima_mensagem IS NOT NULL THEN t.data_hora_ultima_mensagem
                                    END AS data_hora_ultima_mensagem,
                                    t.data_hora_final,
                                    ct.id                       AS id_categoria, 
                                    ct.nome, 
                                    cl.id                       AS id_classificacao, 
                                    cl.nome,
                                    st.id                       AS id_status_ticket,
                                    st.nome,
                                    st.uso,
                                    st.em_aberto,
                                    st.ordem_em_aberto,
                                    uc.id                       AS id_usuario_cliente,
                                    uc.id_cliente,
                                    uc.nome,
                                    uc.email,
                                    uc.telefone,
                                    uc.copia,
                                    uc.provisorio,
                                    uc.ativo
                            FROM dbo.Ticket t
                            LEFT JOIN dbo.Categoria ct          ON t.id_categoria = ct.id
                            LEFT JOIN dbo.Classificacao cl      ON t.id_classificacao = cl.id
                            LEFT JOIN dbo.Status_Ticket st      ON t.id_status_ticket = st.id
                            LEFT JOIN dbo.Usuario_Cliente uc    ON t.id_usuario_cliente = uc.id 
                            WHERE t.id = " + id;


            string sql2 = @"SELECT
                                    a.id,
                                    a.nome
                        FROM		Anexo a
                        LEFT JOIN   Ticket t ON a.id_ticket = t.id
                        WHERE		a.id_ticket = " + id;

            try
            {
                using (var conn = CreateConnection())
                {
                    result1 = conn.Query<Ticket, Categoria, Classificacao, StatusTicket, UsuarioCliente, Ticket>(sql1,
                                map: (ticket, categoria, classificacao, status, usuario) =>
                                {
                                    ticket.Categoria = categoria;
                                    ticket.Classificacao = classificacao;
                                    ticket.StatusTicket = status;
                                    ticket.UsuarioCliente = usuario;

                                    return ticket;
                                },
                                splitOn: "id_categoria,id_classificacao,id_status_ticket,id_usuario_cliente").SingleOrDefault();

                    result2 = conn.Query<Anexo>(sql2).ToList();

                    response.Categoria = result1.Categoria;
                    response.Classificacao = result1.Classificacao;
                    response.DataHoraAlteracao = result1.DataHoraAlteracao;
                    response.DataHoraFinal = result1.DataHoraFinal;
                    response.DataHoraInicial = result1.DataHoraInicial;
                    response.DataHoraUltimaMensagem = result1.DataHoraUltimaMensagem;
                    response.Descricao = result1.Descricao;
                    response.Id = result1.Id;
                    response.StatusTicket = result1.StatusTicket;
                    response.Titulo = result1.Titulo;
                    response.UsuarioCliente = result1.UsuarioCliente;
                    response.Anexos = result2;

                    return response;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Recupera o total de tickets de um determinado status
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public int GetCount(int idStatusTicket)
        {
            int result = 0;
            string sql = string.Empty;

            try
            {
                using (var conn = CreateConnection())
                {
                    result = conn.Select<Ticket>(q => q.IdStatusTicket == idStatusTicket).Count();
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Recupera todos os totais de tickets por status
        /// </summary>
        /// <returns></returns>
        public CountsResponse GetCounts()
        {
            CountsResponse result = new CountsResponse();
            int total = 0;

            try
            {
                using (var conn = CreateConnection())
                {

                    var ids = conn.Query<int>(@"SELECT id FROM Status_Ticket").ToArray();

                    var totalAtendimentos = conn.GetAll<Ticket>().Count();

                    result.AddItem(0, totalAtendimentos);

                    for (int i = 0; i < ids.Length; i++)
                    {
                        if (ids[i] == 0)
                        {
                            total = conn.ExecuteScalar<int>(@"SELECT COUNT(*) FROM Ticket");
                        }
                        else
                        {
                            total = conn.ExecuteScalar<int>(@"SELECT COUNT(*) FROM Ticket WHERE id_status_ticket = @Id", new { Id = ids[i] });
                        }

                        result.AddItem(ids[i], total);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Recupera de forma paginada os tickets, com as referencias populadas
        /// </summary>
        /// <param name="offset">Número de linhas que deve ser pulado</param>
        /// <param name="numRows">Número de linhas que deve ser retornado</param>
        /// <param name="tipoConsulta">Se a consulta vai ou não retornar com ou sem paginação</param>
        /// <returns></returns>
        public TicketsResponse GetAllPaged(FilterRequest advancedFilter)
        {
            TicketsResponse result = new TicketsResponse();
            string sql = string.Empty;
            string sqlCount = string.Empty;

            try
            {
                using (var conn = CreateConnection())
                {
                    result.TotalGeral = (int)conn.ExecuteScalar("SELECT COUNT(*) FROM Ticket");

                    sql = RecuperarQuery(Tipos.TipoConsulta.Paged, advancedFilter);
                    sqlCount = RecuperarQuery(Tipos.TipoConsulta.Count, advancedFilter);

                    result.TotalFiltrado = (int)conn.ExecuteScalar(sqlCount);

                    result.Tickets = conn.Query<Ticket, Categoria, Classificacao, StatusTicket, UsuarioCliente, Ticket>(sql,
                                            map: (ticket, categoria, classificacao, status, usuario) =>
                                            {
                                                ticket.Categoria = categoria;
                                                ticket.Classificacao = classificacao;
                                                ticket.StatusTicket = status;
                                                ticket.UsuarioCliente = usuario;

                                                return ticket;
                                            },
                                            splitOn: "id_categoria,id_classificacao,id_status_ticket,id_usuario_cliente").Distinct().ToList();
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método que atualiza o status do ticket
        /// </summary>
        /// <param name="idTicket"></param>
        /// <param name="idStatus"></param>
        /// <returns></returns>
        public bool UpdateStatusTicket(Ticket ticket)
        {
            bool result;

            try
            {
                using (var conn = CreateConnection())
                {
                    if (ticket == null) { return false; }

                    ticket.DataHoraAlteracao = DateTime.Now;

                    if (ticket.IdStatusTicket == (int)EnumStatusTicket.CANCELADO || ticket.IdStatusTicket == (int)EnumStatusTicket.CONCLUIDO)
                    {
                        ticket.DataHoraFinal = DateTime.Now;
                    }
                    
                    result = conn.Update<Ticket>(ticket);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método que retorna a query de consulta aos tickets
        /// </summary>
        /// <param name="tipoConsulta"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        private string RecuperarQuery(Tipos.TipoConsulta tipoConsulta, FilterRequest advancedFilter)
        {
            StringBuilder sql = new StringBuilder();
            string selectClause = string.Empty;
            string whereClause = string.Empty;
            string orderByClause = string.Empty;
            string offSetNumRows = string.Empty;

            if (tipoConsulta == Tipos.TipoConsulta.Paged)
            {
                selectClause = selectQuery;
                whereClause = BuildWhere(advancedFilter);
                orderByClause = BuildOrderBy(advancedFilter);
                offSetNumRows = BuildOffSetNumRows(advancedFilter);

                sql.Append(selectClause);
                sql.Append(whereClause);
                sql.Append(orderByClause);
                sql.Append(offSetNumRows);

                return sql.ToString();
            }
            else
            {
                selectClause = countQuery;
                whereClause = BuildWhere(advancedFilter);

                sql.Append(selectClause);
                sql.Append(whereClause);

                return sql.ToString();
            }
        }

        /// <summary>
        /// Método que constroi a clausula WHERE da query de recuperação de tickets
        /// </summary>
        /// <param name="advancedFilter"></param>
        /// <returns></returns>
        private string BuildWhere(FilterRequest advancedFilter)
        {
            StringBuilder whereClause = new StringBuilder();

            //Monta os filtros
            if (advancedFilter.IdTicket > 0)
            {
                whereClause.Append(" AND t.id = " + advancedFilter.IdTicket);
            }

            if (advancedFilter.Titulo.Length > 0 && advancedFilter.Titulo.ToLower() != "string")
            {
                whereClause.Append(" AND t.titulo LIKE '%" + advancedFilter.Titulo + "%'");
            }

            if (advancedFilter.Descricao.Length > 0 && advancedFilter.Descricao.ToLower() != "string")
            {
                whereClause.Append(" AND t.descricao LIKE '%" + advancedFilter.Descricao + "%'");
            }

            if (advancedFilter.DataInicial.HasValue && !advancedFilter.DataFinal.HasValue)
            {
                whereClause.Append(" AND t.data_hora_inicial >= '" + advancedFilter.DataInicial + "'");
            }

            if (!advancedFilter.DataInicial.HasValue && advancedFilter.DataFinal.HasValue)
            {
                whereClause.Append(" AND t.data_hora_inicial <= '" + advancedFilter.DataFinal + "'");
            }

            if (advancedFilter.DataInicial.HasValue && advancedFilter.DataFinal.HasValue)
            {
                whereClause.Append(" AND t.data_hora_inicial BETWEEN '" + advancedFilter.DataInicial + "' AND '" + advancedFilter.DataFinal + "'");
            }

            if (advancedFilter.IdCliente > 0)
            {
                whereClause.Append(" AND uc.id_cliente = " + advancedFilter.IdCliente);
            }

            if (advancedFilter.IdCategoria > 0)
            {
                whereClause.Append(" AND ct.id = " + advancedFilter.IdCategoria);
            }

            switch (advancedFilter.IdsStatus)
            {
                case "0":
                    break;
                case "1": //Aguardando Atendimento
                case "4": //Pendente com Cliente
                case "5": //Em Análise
                    whereClause.Append(" AND t.id_status_ticket = " + advancedFilter.IdsStatus);
                    break;
                case "2": //Concluído
                case "3": //Cancelado
                    whereClause.Append(" AND t.id_status_ticket = " + advancedFilter.IdsStatus);
                    break;
                default:
                    whereClause.Append(" AND t.id_status_ticket IN (1,4,5)");
                    break;
            }

            return whereClause.ToString();
        }

        /// <summary>
        /// Método que constroi a clausula ORDER BY da query de recuperação de tickets
        /// </summary>
        /// <param name="advancedFilter"></param>
        /// <returns></returns>
        private string BuildOrderBy(FilterRequest advancedFilter)
        {
            StringBuilder orderByClause = new StringBuilder();

            switch (advancedFilter.IdsStatus)
            {
                case "0": //Todos
                    orderByClause.Append(" ORDER BY t.data_hora_inicial DESC, t.data_hora_ultima_mensagem DESC");
                    break;
                case "1": //Aguardando Atendimento
                case "4": //Pendente com Cliente
                case "5": //Em Análise
                    orderByClause.Append(" ORDER BY t.data_hora_inicial DESC, t.data_hora_ultima_mensagem DESC");
                    break;
                case "2": //Concluído
                case "3": //Cancelado
                    orderByClause.Append(" ORDER BY t.data_hora_final DESC");
                    break;
                default:
                    orderByClause.Append(" ORDER BY st.ordem_em_aberto ASC, t.data_hora_ultima_mensagem DESC");
                    break;
            }

            return orderByClause.ToString();
        }

        /// <summary>
        /// Método que constroi as clausulas OFFSET / FETCH NEXT da query de recuperação de tickets
        /// </summary>
        /// <param name="advancedFilter"></param>
        /// <returns></returns>
        private string BuildOffSetNumRows(FilterRequest advancedFilter)
        {
            if (advancedFilter.NumRows == 0) return string.Empty;

            StringBuilder offSetNumRowsClauses = new StringBuilder();

            offSetNumRowsClauses.Append(" OFFSET " + advancedFilter.OffSet + " ROWS");
            offSetNumRowsClauses.Append(" FETCH NEXT " + advancedFilter.NumRows + " ROWS ONLY");

            return offSetNumRowsClauses.ToString();
        }
    }
}
