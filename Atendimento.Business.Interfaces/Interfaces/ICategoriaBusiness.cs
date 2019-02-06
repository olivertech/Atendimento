using Atendimento.Business.Interfaces.Interfaces.Base;
using Atendimento.Entities.Entities;
using Atendimento.Entities.Requests;
using Atendimento.Entities.Responses;

namespace Atendimento.Business.Interfaces.Interfaces
{
    public interface ICategoriaBusiness : IBusinessBase<Categoria>
    {
        CategoriasResponse GetAllPaged(FilterCategoriaRequest advancedFilter);
    }
}
