namespace Atendimento.Entities.Entities
{
    public class Categoria : BaseEntity
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
    }
}
