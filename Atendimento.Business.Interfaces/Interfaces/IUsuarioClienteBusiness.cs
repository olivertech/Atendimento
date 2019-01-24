using System.Collections.Generic;
using Atendimento.Business.Interfaces.Interfaces.Base;
using Atendimento.Entities.Entities;
using Atendimento.Entities.Requests;
using Atendimento.Entities.Responses;

namespace Atendimento.Business.Interfaces.Interfaces
{
    public interface IUsuarioClienteBusiness : IBusinessBase<UsuarioCliente>
    {
        int GetCount(int idCliente);
        UsuariosClienteResponse GetAllPaged(FilterUsuarioRequest advancedFilter);
        IEnumerable<UsuarioCliente> GetAllById(int idCliente);
        UsuarioCliente GetByUsernameAndPassword(string username, string password);
        bool UpdatePassword(UsuarioCliente usuarioCliente);
    }
}
