namespace Atendimento.Entities.Entities
{
    public class Empresa : BaseEntity
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
                _nome = value.ToString().ToUpper();
            }
        }

        public string Cnpj { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public Endereco Endereco { get; set; }
        public string Tipo { get; set; }
        public string Descricao { get; set; }

        /** COLUNAS DE CHAVE ESTRANGEIRA */
        public int IdGrupo { get; set; }
        public Grupo Grupo { get; set; }
    }
}
