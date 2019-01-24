using System.Collections.Generic;

namespace Atendimento.Entities.Responses
{
    public class CountsResponse
    {
        public List<Total> Totais { get; set; } = new List<Total>();

        public void AddItem(int idStatusTicket, int totalStatusTicket)
        {
            Totais.Add(new Total { IdStatusTicket = idStatusTicket, TotalRegistros = totalStatusTicket });
        }
    }

    public class Total
    {
        public int IdStatusTicket { get; set; }
        public int TotalRegistros { get; set; }
    }
}
