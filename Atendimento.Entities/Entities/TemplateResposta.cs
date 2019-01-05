namespace Atendimento.Entities.Entities
{
    public class TemplateResposta : BaseEntity
    {
        private string _titulo;

        public string Titulo
        {
            get
            {
                return _titulo;
            }
            set
            {
                _titulo = value.ToString().ToUpper();
            }
        }

        public string Descricao { get; set; }
        public string Conteudo { get; set; }
    }
}
