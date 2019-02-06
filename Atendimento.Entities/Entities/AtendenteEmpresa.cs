namespace Atendimento.Entities.Entities
{
    public class AtendenteEmpresa : BaseEntity
    {
        private string _nome;

        public string Nome
        {
            get
            {
                return _nome;
            }
            set
            {
                _nome = !string.IsNullOrEmpty(value) ? value.ToString().ToUpper() : null;
            }
        }

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

        /** COLUNAS DE CHAVE ESTRANGEIRA */
        public int IdEmpresa { get; set; }
        public Empresa Empresa { get; set; }
    }
}
