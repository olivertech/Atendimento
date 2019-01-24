using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Atendimento.Entities.Enums;

namespace Atendimento.Entities.Requests
{
    /// <summary>
    /// Classe Request - Para classes que não tem mapeamento direto com entidades do banco de dados
    /// </summary>
    [DataContract(Name = "changePassword")]
    public class ChangePasswordRequest
    {
        /// <summary>
        /// Login
        /// </summary>
        [Required(ErrorMessage = "O campo 'Username' é obrigatório.")]
        [DataMember(Name = "username")]
        [StringLength(50)]
        public string Username { get; set; }

        /// <summary>
        /// Senha
        /// </summary>
        [Required(ErrorMessage = "O campo 'OldPassword' é obrigatório.")]
        [DataMember(Name = "oldPassword")]
        [StringLength(10)]
        public string OldPassword { get; set; }

        /// <summary>
        /// Senha
        /// </summary>
        [Required(ErrorMessage = "O campo 'NewPassword' é obrigatório.")]
        [DataMember(Name = "newPassword")]
        [StringLength(10)]
        public string NewPassword { get; set; }

        /// <summary>
        /// Tipo de usuário (A) = Atendente | (C) = Cliente
        /// </summary>
        [Required(ErrorMessage = "O campo 'Tipo' é obrigatório. Informe 'Atendimento' para usuário de atendimento ou 'Cliente' para usuário de empresa cliente.")]
        [DataMember(Name = "userType")]
        [EnumDataType(typeof(Tipos.Login))]
        public Tipos.Login UserType { get; set; }
    }
}