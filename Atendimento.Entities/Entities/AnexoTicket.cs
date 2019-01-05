using System;
using System.Collections.Generic;
using System.Text;

namespace Atendimento.Entities.Entities
{
    public class AnexoTicket : BaseEntity
    {
        public int IdAnexo { get; set; }
        public int IdTicket { get; set; }
    }
}
