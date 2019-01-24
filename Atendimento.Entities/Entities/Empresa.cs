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

        public string Email { get; set; }
        public string TelefoneFixo { get; set; }
        public string TelefoneCelular { get; set; }
        public bool IsDefault { get; set; }
        public string Descricao { get; set; }
    }
}
