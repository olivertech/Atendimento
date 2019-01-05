namespace Atendimento.Entities.Entities
{
    public class Grupo : BaseEntity
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

        public string Logomarca { get; set; }
        public string Site { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
    }
}
