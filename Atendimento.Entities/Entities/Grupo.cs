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

        public string Site { get; set; }
        public string Background { get; set; }
        public string LogomarcaGrande { get; set; }
        public string LogomarcaPequena { get; set; }
        public string Favicon { get; set; }
        public string EmailHeader { get; set; }
    }
}
