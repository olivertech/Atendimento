﻿using System.Runtime.Serialization;

namespace Atendimento.Entities.Requests
{
    /// <summary>
    /// Classe Request - Para classes que não tem mapeamento direto com entidades do banco de dados
    /// </summary>
    [DataContract(Name = "filter")]
    public class FilterAtendenteEmpresaRequest
    {
        /// <summary>
        /// OffSet - Número de registros que devem ser pulados, na paginação dos tickets
        /// </summary>
        [DataMember(Name = "offset")]
        public int OffSet { get; set; } = 0;

        /// <summary>
        /// NumRows - Número de registros que devem ser recuperados, na paginação dos tickets
        /// </summary>
        [DataMember(Name = "numrows")]
        public int NumRows { get; set; } = 0;
    }
}
