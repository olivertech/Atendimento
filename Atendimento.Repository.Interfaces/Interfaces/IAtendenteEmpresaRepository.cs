using System.Collections.Generic;
using Atendimento.Entities.Entities;
using Atendimento.Entities.Requests;
using Atendimento.Entities.Responses;
using Atendimento.Repository.Interfaces.Interfaces.Base;

namespace Atendimento.Repository.Interfaces.Interfaces
{
    public interface IAtendenteEmpresaRepository : IRepositoryBase<AtendenteEmpresa>
    {
        IEnumerable<AtendenteEmpresa> GetAll(int idEmpresa);
        AtendentesEmpresaResponse GetAllPaged(FilterAtendenteEmpresaRequest advancedFilter);
        AtendenteEmpresa GetByUsernameAndPassword(string username, string password);
        bool UpdatePassword(AtendenteEmpresa atendente);
    }
}
