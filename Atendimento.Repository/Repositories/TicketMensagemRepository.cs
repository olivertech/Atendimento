using System;
using System.Collections.Generic;
using System.Linq;
using Atendimento.Entities.Entities;
using Atendimento.Entities.Requests;
using Atendimento.Entities.Responses;
using Atendimento.Repository.Interfaces.Interfaces;
using Atendimento.Repository.Repositories.Base;
using Dapper;

namespace Atendimento.Repository.Repositories
{
    public class TicketMensagemRepository : RepositoryBase<TicketMensagem>, ITicketMensagemRepository
    {
        public IEnumerable<TicketMensagemResponse> GetAllByTicketId(TicketMensagensRequest request)
        {
            List<TicketMensagemResponse> result;
            var sql = string.Empty;

            if (request.IdCliente == 0)
            {
                sql = @"SELECT
                                        tm.id,
                                        tm.descricao AS Descricao, 
                                        tm.data_hora_mensagem AS Data, 
                                        tm.interno AS Interno,
                                        CASE
                                            WHEN uc.nome IS NOT NULL THEN uc.nome
                                            WHEN ae.nome IS NOT NULL THEN ae.nome
                                        END AS Autor,
                                        STUFF((SELECT '|' + nome FROM anexo WHERE ticketmensagemid = tm.id for XML PATH('')), 1, 1, '') AS Anexos
                            FROM        Ticket_Mensagem tm
                            LEFT JOIN   Usuario_Cliente uc on tm.usuarioclienteid = uc.id
                            LEFT JOIN	Atendente_Empresa ae on tm.atendenteempresaid = ae.id
                            WHERE tm.ticketid = " + request.IdTicket + " ORDER BY tm.data_hora_mensagem DESC";
            }
            else
            {
                sql = @"SELECT
                                        tm.id,
                                        tm.descricao AS Descricao, 
                                        tm.data_hora_mensagem AS Data, 
                                        tm.interno AS Interno,
                                        CASE
                                            WHEN uc.nome IS NOT NULL THEN uc.nome
                                            WHEN ae.nome IS NOT NULL THEN ae.nome
                                        END AS Autor,
                                        STUFF((SELECT '|' + nome FROM anexo WHERE ticketmensagemid = tm.id for XML PATH('')), 1, 1, '') AS Anexos
                            FROM        Ticket_Mensagem tm
                            LEFT JOIN   Usuario_Cliente uc on tm.usuarioclienteid = uc.id
                            LEFT JOIN	Atendente_Empresa ae on tm.atendenteempresaid = ae.id
                            WHERE tm.ticketid = " + request.IdTicket + " AND tm.interno = 0 ORDER BY tm.data_hora_mensagem DESC";
            }

            try
            {
                using (var conn = CreateConnection())
                {
                    result = conn.Query<TicketMensagemResponse>(sql).ToList();
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
