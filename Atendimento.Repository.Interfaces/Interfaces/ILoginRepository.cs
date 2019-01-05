using Atendimento.Entities.Entities;

namespace Atendimento.Repository.Interfaces.Interfaces
{
    public interface ILoginRepository
    {
        AtendenteEmpresa LoginAtendente(UserLogin userLogin);
        UsuarioCliente LoginCliente(UserLogin userLogin);
        AtendenteEmpresa FindAtendenteEmpresaByEmail(UserLogin usuario);
        UsuarioCliente FindUsuarioClienteByEmail(UserLogin usuario);
    }
}
