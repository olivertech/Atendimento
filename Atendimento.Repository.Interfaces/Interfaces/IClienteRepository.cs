using Atendimento.Entities.Entities;
using Atendimento.Entities.Requests;
using Atendimento.Entities.Responses;
using Atendimento.Repository.Interfaces.Interfaces.Base;

namespace Atendimento.Repository.Interfaces.Interfaces
{
    public interface IClienteRepository : IRepositoryBase<Cliente>
    {
        ClientesResponse GetAllPaged(FilterClienteRequest advancedFilter);
    }
}
