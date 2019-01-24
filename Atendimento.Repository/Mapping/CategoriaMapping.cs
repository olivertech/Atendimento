using Atendimento.Entities.Entities;
using Dapper.FluentMap.Dommel.Mapping;

namespace Atendimento.Repository.Mapping
{
    public class CategoriaMapping : DommelEntityMap<Categoria>
    {
        public CategoriaMapping()
        {
            ToTable("Categoria");
            Map(m => m.Id).ToColumn("id_categoria").IsKey().IsIdentity(); //manter id_categoria em função dos joins
            Map(m => m.Nome).ToColumn("nome");
        }
    }
}
