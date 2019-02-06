using System.Collections.Generic;
using Atendimento.Entities.Entities;

namespace Atendimento.Entities.Responses
{
    /// <summary>
    /// Classe usada para response de categorias paginado
    /// </summary>
    public class CategoriasResponse
    {
        public int TotalGeral { get; set; }
        public IList<Categoria> Categorias { get; set; }
    }
}
