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
                                    uc.telefone_fixo,
                                    uc.telefone_celular,
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
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Método que recupera determinado tickek, com todas as subclasses preenchidas,
        /// podendo carregar ou não os anexos
        /// </summary>
        /// <param name="id"></param>
        /// <param name="withAnexos"></param>
        /// <returns></returns>
        public TicketResponse GetByIdFilled(int id, bool withAnexos)
        {
            var response = new TicketResponse();
            Ticket result1;
            List<Anexo> result2;

            var sql1 = @"SELECT 
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
                                    uc.telefone_fixo,
                                    uc.telefone_celular,
                                    uc.copia,
                                    uc.provisorio,
                                    uc.ativo,
                                    c.id                        AS id_cliente,
                                    c.nome
                            FROM dbo.Ticket t
                            LEFT JOIN dbo.Categoria ct          ON t.id_categoria = ct.id
                            LEFT JOIN dbo.Classificacao cl      ON t.id_classificacao = cl.id
                            LEFT JOIN dbo.Status_Ticket st      ON t.id_status_ticket = st.id
                            LEFT JOIN dbo.Usuario_Cliente uc    ON t.id_usuario_cliente = uc.id 
                            LEFT JOIN dbo.Cliente c             ON uc.id_cliente = c.id
                            WHERE t.id = " + id;


            var sql2 = @"SELECT
                                    a.id,
                                    a.nome
                        FROM		Anexo a
                        LEFT JOIN   Ticket t ON a.id_ticket = t.id
                        WHERE		a.id_ticket = " + id;

            try
            {
                using (var conn = CreateConnection())
                {
                    result1 = conn.Query<Ticket, Categoria, Classificacao, StatusTicket, UsuarioCliente, Cliente, Ticket>(sql1,
                                map: (ticket, categoria, classificacao, status, usuario, cliente) =>
                                {
                                    ticket.Categoria = categoria;
                                    ticket.Classificacao = classificacao;
                                    ticket.StatusTicket = status;
                                    ticket.UsuarioCliente = usuario;
                                    ticket.UsuarioCliente.Cliente = cliente;

                                    return ticket;
                                },
                                splitOn: "id_categoria,id_classificacao,id_status_ticket,id_usuario_cliente,id_cliente").SingleOrDefault();

                    if (withAnexos)
                    {
                        result2 = conn.Query<Anexo>(sql2).ToList();
                        response.Anexos = result2;
                    }

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

                    return response;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Recupera o total de tickets de um determinado status
        /// </summary>
        /// <param name="idStatusTicket"></param>
        /// <returns></returns>
        public int GetCount(int idStatusTicket)
        {
            var result = 0;
            var sql = string.Empty;

            try
            {
                using (var conn = CreateConnection())
                {
                    result = conn.Select<Ticket>(q => q.IdStatusTicket == idStatusTicket).Count();
                }

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Recupera o total de tickets associados a um usuario
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public int GetTotalTicketsUsuario(int idUsuario)
        {
            var result = 0;
            var sql = string.Empty;

            try
            {
                using (var conn = CreateConnection())
                {
                    result = conn.Select<Ticket>(q => q.IdUsuarioCliente == idUsuario).Count();
                }

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Recupera todos os totais de tickets por status
        /// </summary>
        /// <param name="idCliente"></param>
        /// <returns></returns>
        public CountsResponse GetCounts(int idCliente)
        {
            var result = new CountsResponse();
            var total = 0;

            try
            {
                using (var conn = CreateConnection())
                {
                    //Recupera todos os ids dos status cadastrados
                    var statusTickets = conn.Query<int>(@"SELECT id FROM Status_Ticket").ToArray();

                    if (idCliente > 0)
                    {
                        var sql = @"SELECT COUNT(*)
                                    FROM Ticket t
                                    INNER JOIN Usuario_Cliente uc ON t.id_usuario_cliente = uc.id
                                    INNER JOIN Cliente c ON uc.id_cliente = c.id ";

                        var builder = new StringBuilder();

                        builder.Append(sql);
                        builder.Append("WHERE c.id = " + idCliente);

                        total = conn.ExecuteScalar<int>(builder.ToString());
                        result.AddItem(0, total);

                        for (int i = 0; i < statusTickets.Length; i++)
                        {
                            builder.Clear();
                            builder.Append(sql);
                            builder.Append("WHERE c.id = " + idCliente + " AND id_status_ticket = " + statusTickets[i]);

                            total = conn.ExecuteScalar<int>(builder.ToString());
                            result.AddItem(statusTickets[i], total);
                        }
                    }
                    else
                    {
                        total = conn.ExecuteScalar<int>(@"SELECT COUNT(*) FROM Ticket");
                        result.AddItem(0, total);

                        for (int i = 0; i < statusTickets.Length; i++)
                        {
                            total = conn.ExecuteScalar<int>(@"SELECT COUNT(*) FROM Ticket WHERE id_status_ticket = @Id", new { Id = statusTickets[i] });
                            result.AddItem(statusTickets[i], total);
                        }
                    }
                }

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Recupera de forma paginada os tickets, com as referencias populadas
        /// </summary>
        /// <param name="advancedFilter"></param>
        /// <returns></returns>
        public TicketsResponse GetAllPaged(FilterRequest advancedFilter)
        {
            var result = new TicketsResponse();
            var sql = string.Empty;
            var sqlCount = string.Empty;

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
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Método que atualiza o status do ticket
        /// </summary>
        /// <param name="ticket"></param>
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
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Método que retorna a query de consulta aos tickets
        /// </summary>
        /// <param name="tipoConsulta"></param>
        /// <param name="advancedFilter"></param>
        /// <returns></returns>
        private static string RecuperarQuery(Tipos.TipoConsulta tipoConsulta, FilterRequest advancedFilter)
        {
            var sql = new StringBuilder();
            var selectClause = string.Empty;
            var whereClause = string.Empty;
            var orderByClause = string.Empty;
            var offSetNumRows = string.Empty;

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
        private static string BuildWhere(FilterRequest advancedFilter)
        {
            var whereClause = new StringBuilder();

            //Monta os filtros
            if (advancedFilter.IdTicketFiltro > 0)
            {
                whereClause.Append(" AND t.id = " + advancedFilter.IdTicketFiltro);
            }

            if (advancedFilter.TituloFiltro.Length > 0 && advancedFilter.TituloFiltro.ToLower() != "string")
            {
                whereClause.Append(" AND t.titulo LIKE '%" + advancedFilter.TituloFiltro + "%'");
            }

            if (advancedFilter.DescricaoFiltro.Length > 0 && advancedFilter.DescricaoFiltro.ToLower() != "string")
            {
                whereClause.Append(" AND t.descricao LIKE '%" + advancedFilter.DescricaoFiltro + "%'");
            }

            if (advancedFilter.DataInicialFiltro.HasValue && !advancedFilter.DataFinalFiltro.HasValue)
            {
                whereClause.Append(" AND t.data_hora_inicial >= '" + advancedFilter.DataInicialFiltro + "'");
            }

            if (!advancedFilter.DataInicialFiltro.HasValue && advancedFilter.DataFinalFiltro.HasValue)
            {
                whereClause.Append(" AND t.data_hora_inicial <= '" + advancedFilter.DataFinalFiltro + "'");
            }

            if (advancedFilter.DataInicialFiltro.HasValue && advancedFilter.DataFinalFiltro.HasValue)
            {
                whereClause.Append(" AND t.data_hora_inicial BETWEEN '" + advancedFilter.DataInicialFiltro + "' AND '" + advancedFilter.DataFinalFiltro + "'");
            }

            if (advancedFilter.IdClienteFiltro > 0)
            {
                whereClause.Append(" AND uc.id_cliente = " + advancedFilter.IdClienteFiltro);
            }

            if (advancedFilter.IdCategoriaFiltro > 0)
            {
                whereClause.Append(" AND ct.id = " + advancedFilter.IdCategoriaFiltro);
            }

            if (advancedFilter.IdClienteSession > 0)
            {
                whereClause.Append(" AND uc.id_cliente = " + advancedFilter.IdClienteSession);
            }

            switch (advancedFilter.IdsStatusFiltro)
            {
                case "0":
                    break;
                case "1": //Aguardando Atendimento
                case "4": //Pendente com Cliente
                case "5": //Em Análise
                    whereClause.Append(" AND t.id_status_ticket = " + advancedFilter.IdsStatusFiltro);
                    break;
                case "2": //Concluído
                case "3": //Cancelado
                    whereClause.Append(" AND t.id_status_ticket = " + advancedFilter.IdsStatusFiltro);
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
        private static string BuildOrderBy(FilterRequest advancedFilter)
        {
            var orderByClause = new StringBuilder();

            if (string.IsNullOrEmpty(advancedFilter.OrderBy) && string.IsNullOrEmpty(advancedFilter.Direction))
            {
                switch (advancedFilter.IdsStatusFiltro)
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
            }
            else
            {
                orderByClause.Append(" ORDER BY t." + advancedFilter.OrderBy + " " + advancedFilter.Direction);
            }

            return orderByClause.ToString();
        }

        /// <summary>
        /// Método que constroi as clausulas OFFSET / FETCH NEXT da query de recuperação de tickets
        /// </summary>
        /// <param name="advancedFilter"></param>
        /// <returns></returns>
        private static string BuildOffSetNumRows(FilterRequest advancedFilter)
        {
            if (advancedFilter.NumRows == 0) return string.Empty;

            var offSetNumRowsClauses = new StringBuilder();

            offSetNumRowsClauses.Append(" OFFSET " + advancedFilter.OffSet + " ROWS");
            offSetNumRowsClauses.Append(" FETCH NEXT " + advancedFilter.NumRows + " ROWS ONLY");

            return offSetNumRowsClauses.ToString();
        }
    }
}
