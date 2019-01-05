using System;

namespace Atendimento.Entities.Entities
{
    public class Ticket : BaseEntity
    {
        private string _titulo;

        public string Titulo
        {
            get
            {
                return _titulo;
            }
            set
            {
                _titulo = value.ToString().ToUpper();
            }
        }

        public string Descricao { get; set; }
        public DateTime? DataHoraInicial { get; set; }
        public DateTime? DataHoraAlteracao{ get; set; }
        public DateTime? DataHoraUltimaMensagem { get; set; }
        public DateTime? DataHoraFinal { get; set; }

        /** COLUNAS DE CHAVE ESTRANGEIRA */
        public int IdStatusTicket { get; set; }
        public StatusTicket StatusTicket { get; set; }

        public int IdUsuarioCliente { get; set; }
        public UsuarioCliente UsuarioCliente { get; set; }

        public int IdCategoria { get; set; }
        public Categoria Categoria { get; set; }

        public int IdClassificacao { get; set; }
        public Classificacao Classificacao { get; set; }
    }
}
