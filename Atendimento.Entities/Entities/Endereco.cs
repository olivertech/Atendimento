namespace Atendimento.Entities.Entities
{
    /// <summary>
    /// Classe de endereço herdada pelas entidades Cliente e Empresa
    /// </summary>
    public class Endereco
    {
        private string _estado;
        private string _bairro;
        private string _cidade;

        public string Logradouro { get; set; }
        public string NumeroLogradouro { get; set; }
        public string ComplementoLogradouro { get; set; }

        public string Estado
        {
            get
            {
                return _estado;
            }
            set
            {
                _estado = value.ToString().ToUpper();
            }
        }

        public string Cidade
        {
            get
            {
                return _cidade;
            }
            set
            {
                _cidade = value.ToString().ToUpper();
            }
        }

        public string Bairro
        {
            get
            {
                return _bairro;
            }
            set
            {
                _bairro = value.ToString().ToUpper();
            }
        }

        public string Cep { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}
