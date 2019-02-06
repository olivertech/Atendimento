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
                _nome = !string.IsNullOrEmpty(value) ? value.ToString().ToUpper() : null;
            }
        }

        public string Email { get; set; }
        public string TelefoneFixo { get; set; }
        public string TelefoneCelular { get; set; }
        public bool IsDefault { get; set; }
        public string Descricao { get; set; }
    }
}
