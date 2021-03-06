﻿using System.Collections.Generic;
using Atendimento.Entities.Entities;

namespace Atendimento.Entities.Responses
{
    /// <summary>
    /// Classe usada para response de empresas paginado
    /// </summary>
    public class EmpresasResponse
    {
        public int TotalGeral { get; set; }
        public IList<Empresa> Empresas { get; set; }
    }
}
