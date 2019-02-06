using Atendimento.Entities.Entities;
using Atendimento.Entities.Requests;
using Atendimento.Entities.Responses;
using Atendimento.Repository.Interfaces.Interfaces.Base;

namespace Atendimento.Repository.Interfaces.Interfaces
{
    public interface ICategoriaRepository : IRepositoryBase<Categoria>
    {
        CategoriasResponse GetAllPaged(FilterCategoriaRequest advancedFilter);
    }
}
