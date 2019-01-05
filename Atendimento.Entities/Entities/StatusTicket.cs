namespace Atendimento.Entities.Entities
{
    public class StatusTicket : BaseEntity
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
        
        private string _uso;
        public string Uso
        {
            get
            {
                return _uso;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    _uso = "E";

                _uso = value.ToString().ToUpper();
            }
        }

        public bool EmAberto { get; set; } = true;
        public int OrdemEmAberto { get; set; } = 0;
    }
}
