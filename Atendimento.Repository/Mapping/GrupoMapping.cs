using Atendimento.Entities.Entities;
using Dapper.FluentMap.Dommel.Mapping;

namespace Atendimento.Repository.Mapping
{
    public class GrupoMapping : DommelEntityMap<Grupo>
    {
        public GrupoMapping()
        {
            ToTable("Grupo");
            Map(m => m.Id).ToColumn("id_grupo").IsKey().IsIdentity();
            Map(m => m.Nome).ToColumn("nome");
            Map(m => m.Logomarca).ToColumn("logomarca");
            Map(m => m.Site).ToColumn("site");
            Map(m => m.Facebook).ToColumn("facebook");
            Map(m => m.Twitter).ToColumn("twitter");
            Map(m => m.Instagram).ToColumn("instagram");
        }
    }
}
