namespace Atendimento.Entities.Entities
{
    public class Cliente : BaseEntity
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

        public string Cnpj { get; set; }
        public string Email { get; set; }
        public string TelefoneFixo { get; set; }
        public string TelefoneCelular { get; set; }
        public string Logradouro { get; set; }
        public string NumeroLogradouro { get; set; }
        public string ComplementoLogradouro { get; set; }

        private string _estado;
        public string Estado
        {
            get
            {
                return _estado;
            }
            set
            {
                _estado = !string.IsNullOrEmpty(value) ? value.ToString().ToUpper() : null;
            }
        }

        private string _cidade;
        public string Cidade
        {
            get
            {
                return _cidade;
            }
            set
            {
                _cidade = !string.IsNullOrEmpty(value) ? value.ToString().ToUpper() : null;
            }
        }

        private string _bairro;
        public string Bairro
        {
            get
            {
                return _bairro;
            }
            set
            {
                _bairro = !string.IsNullOrEmpty(value) ? value.ToString().ToUpper() : null;
            }
        }

        public string Cep { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }

        /** COLUNAS DE CHAVE ESTRANGEIRA */
        public int IdEmpresa { get; set; }
        public Empresa Empresa { get; set; }
    }
}
