using Atendimento.Entities.Entities;
using Atendimento.Entities.Responses;

namespace Atendimento.Business.Interfaces.Interfaces
{
    public interface ILoginBusiness
    {
        LoginResponse DoLogin(UserLogin userLogin);
        RecoverResponse DoRecover(UserLogin userLogin);
    }
}
