using System.Collections.Generic;
using Atendimento.Entities.Entities;
using Atendimento.Entities.Requests;
using Atendimento.Entities.Responses;
using Atendimento.Repository.Interfaces.Interfaces.Base;

namespace Atendimento.Repository.Interfaces.Interfaces
{
    public interface IUsuarioClienteRepository : IRepositoryBase<UsuarioCliente>
    {
        int GetCount(int idCliente);
        UsuarioCliente GetFullById(int idUsuario);
        UsuariosClienteResponse GetAllPaged(FilterUsuarioRequest advancedFilter);
        IEnumerable<UsuarioCliente> GetAllById(int idCliente);
        UsuarioCliente GetByUsernameAndPassword(string username, string password);
        bool UpdatePassword(UsuarioCliente usuarioCliente);
    }
}
