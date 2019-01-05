using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Atendimento.Entities.Enums;

namespace Atendimento.Entities.Requests
{
    /// <summary>
    /// Classe Request - Para classes que não tem mapeamento direto com entidades do banco de dados
    /// </summary>
    [DataContract(Name = "recover")]
    public class RecoverRequest
    {
        /// <summary>
        /// Email
        /// </summary>
        [Required(ErrorMessage = "O campo 'Email' é obrigatório.")]
        [DataMember(Name = "email")]
        [StringLength(50, ErrorMessage ="O campo 'Email' deve possuir até 50 caracteres.")]
        public string Email { get; set; }

        /// <summary>
        /// Tipo de usuário (A) = Atendente | (C) = Cliente
        /// </summary>
        [Required(ErrorMessage = "O campo 'Tipo' é obrigatório. Informe 'Atendimento' para usuário de atendimento ou 'Cliente' para usuário de empresa cliente.")]
        [DataMember(Name = "userType")]
        [EnumDataType(typeof(Tipos.Login))]
        public Tipos.Login UserType { get; set; }
    }
}
