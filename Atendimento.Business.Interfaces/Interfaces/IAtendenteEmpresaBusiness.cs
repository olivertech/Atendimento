using System.Collections.Generic;
using Atendimento.Business.Interfaces.Interfaces.Base;
using Atendimento.Entities.Entities;
using Atendimento.Entities.Requests;
using Atendimento.Entities.Responses;

namespace Atendimento.Business.Interfaces.Interfaces
{
    public interface IAtendenteEmpresaBusiness : IBusinessBase<AtendenteEmpresa>
    {
        IEnumerable<AtendenteEmpresa> GetAll(int idEmpresa);
        AtendentesEmpresaResponse GetAllPaged(FilterAtendenteEmpresaRequest advancedFilter);
        AtendenteEmpresa GetByUsernameAndPassword(string username, string password);
        bool UpdatePassword(AtendenteEmpresa atendente);
    }
}
