using Atendimento.Business.Interfaces.Interfaces.Base;
using Atendimento.Entities.Entities;
using Atendimento.Entities.Requests;
using Atendimento.Entities.Responses;

namespace Atendimento.Business.Interfaces.Interfaces
{
    public interface ITemplateRespostaBusiness : IBusinessBase<TemplateResposta>
    {
        TemplatesResponse GetAllPaged(FilterTemplateRequest advancedFilter);
    }
}
