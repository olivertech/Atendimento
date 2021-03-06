﻿namespace Atendimento.Entities.Responses
{
    public class AtendenteEmpresaResponse
    {
        public int Id { get; set; }
        public int IdEmpresa { get; set; }
        public string Nome { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string TelefoneFixo { get; set; }
        public string TelefoneCelular { get; set; }
        public bool Ativo { get; set; }
        public bool Copia { get; set; }
        public bool Provisorio { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsDefault { get; set; }
    }
}