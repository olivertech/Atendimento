using System.Collections.Generic;
using Atendimento.Entities.Entities;

namespace Atendimento.Entities.Responses
{
    /// <summary>
    /// Classe usada para response de clientes paginado
    /// </summary>
    public class ClientesResponse
    {
        public int TotalGeral { get; set; }
        public IList<Cliente> Clientes { get; set; }
    }
}
