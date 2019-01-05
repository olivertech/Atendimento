using System.Collections.Generic;
using Atendimento.Business.Interfaces.Interfaces.Base;
using Atendimento.Entities.Entities;

namespace Atendimento.Business.Interfaces.Interfaces
{
    public interface IUsuarioClienteBusiness : IBusinessBase<UsuarioCliente>
    {
        IEnumerable<UsuarioCliente> GetAllById(int idCliente);
    }
}
