using System.Collections.Generic;
using Atendimento.Entities.Entities;

namespace Atendimento.Entities.Responses
{
    /// <summary>
    /// Classe usada para response de templates paginado
    /// </summary>
    public class TemplatesResponse
    {
        public int TotalGeral { get; set; }
        public IList<TemplateResposta> Templates { get; set; }
    }
}
