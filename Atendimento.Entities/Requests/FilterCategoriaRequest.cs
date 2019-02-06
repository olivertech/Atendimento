using System.Runtime.Serialization;

namespace Atendimento.Entities.Requests
{
    /// <summary>
    /// Classe Request - Para classes que não tem mapeamento direto com entidades do banco de dados
    /// </summary>
    [DataContract(Name = "filter")]
    public class FilterCategoriaRequest
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

        /// <summary>
        /// OrderBY - A maneira como deve ser ordenada a lista de templates
        /// </summary>
        [DataMember(Name = "orderby")]
        public string OrderBy { get; set; } = "id";

        /// <summary>
        /// Direction - A direção como deve ser ordenada a lista de templates
        /// </summary>
        [DataMember(Name = "direction")]
        public string Direction { get; set; } = "ASC";

    }
}
