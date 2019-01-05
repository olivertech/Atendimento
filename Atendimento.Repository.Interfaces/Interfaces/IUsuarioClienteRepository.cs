using System.Collections.Generic;
using Atendimento.Entities.Entities;
using Atendimento.Repository.Interfaces.Interfaces.Base;

namespace Atendimento.Repository.Interfaces.Interfaces
{
    public interface IUsuarioClienteRepository : IRepositoryBase<UsuarioCliente>
    {
        IEnumerable<UsuarioCliente> GetAllById(int idCliente);
    }
}
