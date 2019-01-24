using System.Collections.Generic;
using Atendimento.Entities.Entities;

namespace Atendimento.Entities.Responses
{
    /// <summary>
    /// Classe usada para response de empresas paginado
    /// </summary>
    public class AtendentesEmpresaResponse
    {
        public int TotalGeral { get; set; }
        public IList<AtendenteEmpresa> Atendentes { get; set; }
    }
}
