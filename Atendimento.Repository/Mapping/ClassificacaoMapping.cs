using Atendimento.Entities.Entities;
using Dapper.FluentMap.Dommel.Mapping;

namespace Atendimento.Repository.Mapping
{
    public class ClassificacaoMapping : DommelEntityMap<Classificacao>
    {
        public ClassificacaoMapping()
        {
            ToTable("Classificacao");
            Map(m => m.Id).ToColumn("id_classificacao_entity").IsKey().IsIdentity();
            Map(m => m.Nome).ToColumn("nome");
        }
    }
}
