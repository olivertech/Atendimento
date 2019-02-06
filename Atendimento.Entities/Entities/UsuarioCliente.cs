namespace Atendimento.Entities.Entities
{
    public class UsuarioCliente : BaseEntity
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
        public bool Copia { get; set; }
        public bool Provisorio { get; set; }
        public bool Ativo { get; set; }

        /** COLUNAS DE CHAVE ESTRANGEIRA */
        public int IdCliente { get; set; }
        public Cliente Cliente { get; set; }
    }
}
