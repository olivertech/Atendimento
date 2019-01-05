using System;
using System.Collections.Generic;
using Atendimento.Entities.Entities;

namespace Atendimento.Entities.Responses
{
    public class TicketResponse
    {
        public int Id { get; set; }
        public StatusTicket StatusTicket { get; set; }
        public UsuarioCliente UsuarioCliente { get; set; }
        public Categoria Categoria { get; set; }
        public Classificacao Classificacao { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime? DataHoraInicial { get; set; }
        public DateTime? DataHoraAlteracao { get; set; }
        public DateTime? DataHoraUltimaMensagem { get; set; }
        public DateTime? DataHoraFinal { get; set; }
        public List<Anexo> Anexos { get; set; }
    }
}
