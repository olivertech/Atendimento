using System;
using System.Collections.Generic;
using System.Linq;
using Atendimento.Entities.Entities;
using Atendimento.Entities.Responses;
using Atendimento.Repository.Interfaces.Interfaces;
using Atendimento.Repository.Repositories.Base;
using Dapper;

namespace Atendimento.Repository.Repositories
{
    public class TicketMensagemRepository : RepositoryBase<TicketMensagem>, ITicketMensagemRepository
    {
        public IEnumerable<TicketMensagemResponse> GetAllByTicketId(int idTicket)
        {
            List<TicketMensagemResponse> result;

            string sql = @"SELECT
                                        tm.id,
                                        tm.descricao AS Descricao, 
                                        tm.data_hora_mensagem AS Data, 
                                        tm.interno AS Interno,
                                        CASE
                                            WHEN uc.nome IS NOT NULL THEN uc.nome
                                            WHEN ae.nome IS NOT NULL THEN ae.nome
                                        END AS Autor,
                                        STUFF((SELECT '|' + nome FROM anexo WHERE id_ticket_mensagem = tm.id for XML PATH('')), 1, 1, '') AS Anexos
                            FROM        Ticket_Mensagem tm
                            LEFT JOIN   Usuario_Cliente uc on tm.id_usuario_cliente = uc.id
                            LEFT JOIN	Atendente_Empresa ae on tm.id_atendente_empresa = ae.id
                            WHERE tm.id_ticket = " + idTicket + "ORDER BY tm.data_hora_mensagem DESC"; //5148

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
