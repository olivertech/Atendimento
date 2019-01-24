using System.Collections.Generic;
using Atendimento.Entities.Entities;

namespace Atendimento.Entities.Responses
{
    /// <summary>
    /// Classe usada para response de usuarios paginado
    /// </summary>
    public class UsuariosClienteResponse
    {
        public int TotalGeral { get; set; }
        public IList<UsuarioCliente> Usuarios { get; set; }
    }
}
