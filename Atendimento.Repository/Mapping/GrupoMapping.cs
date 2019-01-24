using Atendimento.Entities.Entities;
using Dapper.FluentMap.Dommel.Mapping;

namespace Atendimento.Repository.Mapping
{
    public class GrupoMapping : DommelEntityMap<Grupo>
    {
        public GrupoMapping()
        {
            ToTable("Grupo");
            Map(m => m.Id).ToColumn("id").IsKey().IsIdentity();
            Map(m => m.Nome).ToColumn("nome");
            Map(m => m.Site).ToColumn("site");
            Map(m => m.Background).ToColumn("background");
            Map(m => m.LogomarcaGrande).ToColumn("logomarca_grande");
            Map(m => m.LogomarcaPequena).ToColumn("logomarca_pequena");
            Map(m => m.Favicon).ToColumn("favicon");
            Map(m => m.EmailHeader).ToColumn("email_header");
        }
    }
}
