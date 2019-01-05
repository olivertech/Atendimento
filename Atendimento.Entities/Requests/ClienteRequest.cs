﻿using System.ComponentModel.DataAnnotations;

namespace Atendimento.Entities.Requests
{
    public class ClienteRequest
    {
        [Required(ErrorMessage = "O campo 'Nome' é obrigatório.")]
        [StringLength(255)]
        public string Nome { get; set; }

        [StringLength(19)]
        public string Cnpj { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(15)]
        public string Telefone { get; set; }

        public EnderecoRequest Endereco { get; set; }

        [StringLength(1000)]
        public string Descricao { get; set; }

        public bool Ativo { get; set; }
    }
}
