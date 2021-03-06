﻿using System.ComponentModel.DataAnnotations;

namespace Atendimento.Entities.Requests
{
    public class AtendenteEmpresaUpdate
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int IdEmpresa { get; set; }

        [Required(ErrorMessage = "O campo 'Nome' é obrigatório.")]
        [StringLength(50)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo 'Username' é obrigatório.")]
        [StringLength(50)]
        public string Username { get; set; }

        [Required(ErrorMessage = "O campo 'Password' é obrigatório.")]
        [StringLength(10)]
        public string Password { get; set; }

        [Required(ErrorMessage = "O campo 'Email' é obrigatório.")]
        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(14)]
        public string TelefoneFixo { get; set; }

        [StringLength(15)]
        public string TelefoneCelular { get; set; }

        public bool Ativo { get; set; }

        public bool Copia { get; set; }

        public bool Provisorio { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsDefault { get; set; }
    }
}