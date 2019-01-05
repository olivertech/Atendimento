using System;
using System.Runtime.Serialization;

namespace Atendimento.Entities.Requests
{
    /// <summary>
    /// Classe Request - Para classes que não tem mapeamento direto com entidades do banco de dados
    /// </summary>
    [DataContract(Name = "filter")]
    public class FilterRequest
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
        /// Id fo Ticket
        /// </summary>
        [DataMember(Name = "idTicket")]
        public int IdTicket { get; set; } = 0;

        /// <summary>
        /// Texto que representa parte ou todo o título de um ticket
        /// </summary>
        [DataMember(Name = "titulo")]
        public string Titulo { get; set; } = "";

        /// <summary>
        /// Texto que representa parte ou toda a descrição de um ticket
        /// </summary>
        [DataMember(Name = "descricao")]
        public string Descricao { get; set; } = "";

        /// <summary>
        /// Data inicial do range de tickets - informar vazio se não quiser filtrar por data inicial
        /// </summary>
        [DataMember(Name = "dataInicial")]
        public DateTime? DataInicial { get; set; }

        /// <summary>
        /// Data final do range de tickets - informar vazio se não quiser filtrar por data final
        /// </summary>
        [DataMember(Name = "dataFinal")]
        public DateTime? DataFinal { get; set; }

        /// <summary>
        /// Id do cliente associado ao ticket
        /// </summary>
        [DataMember(Name = "idCliente")]
        public int IdCliente { get; set; } = 0;

        /// <summary>
        /// Id da categoria associada ao ticket
        /// </summary>
        [DataMember(Name = "idCategoria")]
        public int IdCategoria { get; set; } = 0;

        /// <summary>
        /// Ids dos status do ticket no formato texto. No caso de mais de um id, informar todos separado por ","
        /// </summary>
        [DataMember(Name = "idsStatus")]
        public string IdsStatus { get; set; } = "0";
    }
}
