using Atendimento.Entities.Entities;
using Atendimento.Repository.Interfaces.Interfaces;
using Atendimento.Repository.Repositories.Base;

namespace Atendimento.Repository.Repositories
{
    public class ClienteRepository : RepositoryBase<Cliente>, IClienteRepository
    {
    }
}
