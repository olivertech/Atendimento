using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atendimento.Entities.Entities;
using Atendimento.Entities.Enums;
using Atendimento.Entities.Requests;
using Atendimento.Entities.Responses;
using Atendimento.Repository.Interfaces.Interfaces;
using Dapper;
using Dommel;

namespace Atendimento.Repository.Repositories
{
    //public class TicketRepositoryOLD : RepositoryBaseOLD<Ticket>, ITicketRepository
    //{
    //    /// <summary>
    //    /// Recupera o total de tickets de um determinado status
    //    /// </summary>
    //    /// <param name="filter"></param>
    //    /// <returns></returns>
    //    public int GetCount(int idStatusTicket)
    //    {
    //        int result = 0;
    //        string sql = string.Empty;

    //        try
    //        {
    //            using (var conn = CreateConnection())
    //            {
    //                result = conn.Select<Ticket>(q => q.StatusTicket.IdStatusTicket == idStatusTicket).Count();
    //            }

    //            return result;
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }

    //    /// <summary>
    //    /// Recupera todos os totais de tickets por status
    //    /// </summary>
    //    /// <returns></returns>
    //    public CountsResponse GetCounts()
    //    {
    //        CountsResponse result = new CountsResponse();
    //        int total = 0;

    //        try
    //        {
    //            using (var conn = CreateConnection())
    //            {

    //                var ids = conn.Query<int>(@"SELECT id_status_ticket FROM Status_Ticket").ToArray();

    //                var totalAtendimentos = conn.GetAll<Ticket>().Count();

    //                result.AddItem(0, totalAtendimentos);

    //                for (int i = 0; i < ids.Length; i++)
    //                {
    //                    if (ids[i] == 0)
    //                    {
    //                        total = conn.ExecuteScalar<int>(@"SELECT COUNT(*) FROM Ticket");
    //                    }
    //                    else
    //                    {
    //                        total = conn.ExecuteScalar<int>(@"SELECT COUNT(*) FROM Ticket WHERE id_status_ticket = @Id", new { Id = ids[i] });
    //                    }
                        
    //                    result.AddItem(ids[i], total);
    //                }
    //            }

    //            return result;
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }

    //    /// <summary>
    //    /// Recupera de forma paginada os tickets, com as referencias populadas
    //    /// </summary>
    //    /// <param name="offset">Número de linhas que deve ser pulado</param>
    //    /// <param name="numRows">Número de linhas que deve ser retornado</param>
    //    /// <param name="tipoConsulta">Se a consulta vai ou não retornar com ou sem paginação</param>
    //    /// <returns></returns>
    //    public TicketsResponse GetAllPaged(FilterRequest advancedFilter)
    //    {
    //        TicketsResponse result = new TicketsResponse();
    //        string sql = string.Empty;

    //        try
    //        {
    //            using (var conn = CreateConnection())
    //            {
    //                result.TotalGeral = (int)conn.ExecuteScalar("SELECT COUNT(*) FROM Ticket");

    //                if (advancedFilter.IdsStatus == "0")
    //                    result.TotalPorStatus = result.TotalGeral;
    //                else
    //                    result.TotalPorStatus = (int)conn.ExecuteScalar("SELECT COUNT(*) FROM Ticket WHERE id_status_ticket IN (" + advancedFilter.IdsStatus + ")");

    //                sql = RecuperarQuery(Tipos.TipoConsulta.Paged, advancedFilter);

    //                result.Tickets = conn.Query<Ticket, UsuarioCliente, StatusTicket, Categoria, Classificacao, Ticket>(sql,
    //                                    map: (tres, uc, st, ct, cl) =>
    //                                    {
    //                                        tres.UsuarioCliente = uc;
    //                                        tres.StatusTicket = st;
    //                                        tres.Categoria = ct;
    //                                        tres.Classificacao = cl;

    //                                        return tres;
    //                                    },
    //                                    splitOn: "id_usuario_cliente, id_status_ticket, id_categoria, id_classificacao" //,param: new { @OffSet = advancedFilter.OffSet, @NumRows = advancedFilter.NumRows }
    //                                    ).Distinct().ToList();

    //                result.TotalGeral = result.Tickets.Count();
    //            }

    //            return result;
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }

    //    /// <summary>
    //    /// Método que retorna a query de consulta aos tickets
    //    /// </summary>
    //    /// <param name="tipoConsulta"></param>
    //    /// <param name="filter"></param>
    //    /// <returns></returns>
    //    private string RecuperarQuery(Tipos.TipoConsulta tipoConsulta, FilterRequest advancedFilter)
    //    {
    //        StringBuilder sql = new StringBuilder();
    //        string selectClause = string.Empty;
    //        string whereClause = string.Empty;
    //        string orderByClause = string.Empty;
    //        string offSetNumRows = string.Empty;

    //        selectClause = BuildSelect(tipoConsulta);
    //        whereClause = BuildWhere(advancedFilter);
    //        orderByClause = BuildOrderBy(advancedFilter);
    //        offSetNumRows = BuildOffSetNumRows(advancedFilter);

    //        sql.Append(selectClause);
    //        sql.Append(whereClause);
    //        sql.Append(orderByClause);
    //        sql.Append(offSetNumRows);

    //        return sql.ToString();
    //    }

    //    private string BuildSelect(Tipos.TipoConsulta tipoConsulta)
    //    {
    //        StringBuilder body = new StringBuilder();

    //        switch (tipoConsulta)
    //        {
    //            case Tipos.TipoConsulta.All:

    //                body.Append(@"SELECT 
    //                                        t.id_ticket             AS id_ticket,
    //                                        t.titulo,
    //                                        t.descricao,
    //                                        CASE
    //                                            WHEN t.data_hora_ultima_mensagem IS NULL THEN t.data_hora_inicial
    //                                            WHEN t.data_hora_ultima_mensagem IS NOT NULL THEN t.data_hora_ultima_mensagem
    //                                        END AS data_hora_ultima_mensagem,


    //                                        uc.id_usuario_cliente   AS id_usuario_cliente,
    //                                        uc.id_cliente,
    //                                        uc.nome,
    //                                        uc.email,
    //                                        uc.telefone,
    //                                        uc.copia,
    //                                        uc.provisorio,
    //                                        uc.ativo,

    //                                        st.id_status_ticket     AS id_status_ticket,
    //                                        st.nome,
    //                                        st.uso,
    //                                        st.em_aberto,
    //                                        st.ordem_em_aberto,

    //                                        c.id_categoria          AS id_categoria,
    //                                        c.nome,

    //                                        cl.id_classificacao     AS id_classificacao,
    //                                        cl.nome
    //                                FROM Ticket t
    //                                LEFT JOIN Status_Ticket st      ON t.id_status_ticket = st.id_status_ticket
    //                                LEFT JOIN Usuario_Cliente uc    ON t.id_usuario_cliente = uc.id_usuario_cliente
    //                                LEFT JOIN Categoria c           ON t.id_categoria = c.id_categoria
    //                                LEFT JOIN Classificacao cl      ON t.id_classificacao = cl.id_classificacao
    //                                WHERE 1 = 1");

    //                break;
    //            case Tipos.TipoConsulta.Paged:

    //                body.Append(@"SELECT 
    //                                        t.id_ticket             AS id_ticket,
    //                                        t.titulo,
    //                                        t.descricao,
    //                                        CASE
    //                                            WHEN t.data_hora_ultima_mensagem IS NULL THEN t.data_hora_inicial
    //                                            WHEN t.data_hora_ultima_mensagem IS NOT NULL THEN t.data_hora_ultima_mensagem
    //                                        END AS data_hora_ultima_mensagem,

    //                                        uc.id_usuario_cliente   AS id_usuario_cliente,
    //                                        uc.id_cliente,
    //                                        uc.nome,
    //                                        uc.email,
    //                                        uc.telefone,
    //                                        uc.copia,
    //                                        uc.provisorio,
    //                                        uc.ativo,

    //                                        st.id_status_ticket     AS id_status_ticket,
    //                                        st.nome,
    //                                        st.uso,
    //                                        st.em_aberto,
    //                                        st.ordem_em_aberto,

    //                                        c.id_categoria          AS id_categoria,
    //                                        c.nome,

    //                                        cl.id_classificacao     AS id_classificacao,
    //                                        cl.nome
    //                                FROM Ticket t
    //                                LEFT JOIN Status_Ticket st      ON t.id_status_ticket = st.id_status_ticket
    //                                LEFT JOIN Usuario_Cliente uc    ON t.id_usuario_cliente = uc.id_usuario_cliente
    //                                LEFT JOIN Categoria c           ON t.id_categoria = c.id_categoria
    //                                LEFT JOIN Classificacao cl      ON t.id_classificacao = cl.id_classificacao
    //                                WHERE 1 = 1");

    //                break;
    //        }

    //        return body.ToString();
    //    }

    //    private string BuildWhere(FilterRequest advancedFilter)
    //    {
    //        StringBuilder whereClause = new StringBuilder();

    //        //Monta os filtros
    //        if (advancedFilter.IdTicket > 0)
    //        {
    //            whereClause.Append(" AND t.id_ticket LIKE '%" + advancedFilter.IdTicket + "%'");
    //        }

    //        if (advancedFilter.Titulo.Length > 0 && advancedFilter.Titulo.ToLower() != "string")
    //        {
    //            whereClause.Append(" AND t.titulo LIKE '%" + advancedFilter.Titulo + "%'");
    //        }

    //        if (advancedFilter.Descricao.Length > 0 && advancedFilter.Descricao.ToLower() != "string")
    //        {
    //            whereClause.Append(" AND t.descricao LIKE '%" + advancedFilter.Descricao + "%'");
    //        }

    //        if (advancedFilter.DataInicial.HasValue && !advancedFilter.DataFinal.HasValue)
    //        {
    //            whereClause.Append(" AND t.data_hora_inicial >= '" + advancedFilter.DataInicial + "'");
    //        }

    //        if (!advancedFilter.DataInicial.HasValue && advancedFilter.DataFinal.HasValue)
    //        {
    //            whereClause.Append(" AND t.data_hora_inicial <= '" + advancedFilter.DataFinal + "'");
    //        }

    //        if (advancedFilter.DataInicial.HasValue && advancedFilter.DataFinal.HasValue)
    //        {
    //            whereClause.Append(" AND t.data_hora_inicial BETWEEN '" + advancedFilter.DataInicial + "' AND '" + advancedFilter.DataFinal + "'");
    //        }

    //        if (advancedFilter.IdEmpresa > 0)
    //        {
    //            whereClause.Append(" AND uc.id_cliente = " + advancedFilter.IdEmpresa);
    //        }

    //        if (advancedFilter.IdCategoria > 0)
    //        {
    //            whereClause.Append(" AND c.id_categoria = " + advancedFilter.IdCategoria);
    //        }

    //        return whereClause.ToString();
    //    }

    //    private string BuildOrderBy(FilterRequest advancedFilter)
    //    {
    //        StringBuilder orderByClause = new StringBuilder();

    //        switch (advancedFilter.IdsStatus)
    //        {
    //            case "0": //Todos
    //                orderByClause.Append(" ORDER BY t.data_hora_inicial DESC, t.data_hora_ultima_mensagem DESC");
    //                break;
    //            case "1": //Aguardando Atendimento
    //            case "4": //Pendente com Cliente
    //            case "5": //Em Análise
    //                orderByClause.Append(" AND t.id_status_ticket = " + advancedFilter.IdsStatus);
    //                orderByClause.Append(" ORDER BY t.data_hora_inicial DESC, t.data_hora_ultima_mensagem DESC");
    //                break;
    //            case "2": //Concluído
    //            case "3": //Cancelado
    //                orderByClause.Append(" AND t.id_status_ticket = " + advancedFilter.IdsStatus);
    //                orderByClause.Append(" ORDER BY t.data_hora_final DESC");
    //                break;
    //            default:
    //                orderByClause.Append(" AND t.id_status_ticket IN (1,4,5)");
    //                orderByClause.Append(" ORDER BY st.ordem_em_aberto ASC, t.data_hora_ultima_mensagem DESC");
    //                break;
    //        }

    //        return orderByClause.ToString();
    //    }

    //    private string BuildOffSetNumRows(FilterRequest advancedFilter)
    //    {
    //        if (advancedFilter.NumRows == 0) return string.Empty;

    //        StringBuilder offSetNumRowsClauses = new StringBuilder();

    //        offSetNumRowsClauses.Append(" OFFSET " + advancedFilter.OffSet + " ROWS");
    //        offSetNumRowsClauses.Append(" FETCH NEXT " + advancedFilter.NumRows + " ROWS ONLY");

    //        return offSetNumRowsClauses.ToString();
    //    }
    //}
}
