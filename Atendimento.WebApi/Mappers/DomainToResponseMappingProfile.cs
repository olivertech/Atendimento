using Atendimento.Entities.Entities;
using Atendimento.Entities.Responses;
using AutoMapper;

namespace Atendimento.WebApi.Mappers
{
    /// <summary>
    /// Domain -> Responses Mapping
    /// </summary>
    public class DomainToResponseMappingProfile : Profile
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DomainToResponseMappingProfile()
        {
            CreateMap<Categoria, CategoriaResponse>();
            CreateMap<Classificacao, ClassificacaoResponse>();
            CreateMap<StatusTicket, StatusTicketResponse>();
            CreateMap<Empresa, EmpresaResponse>();
            CreateMap<AtendenteEmpresa, AtendenteEmpresaResponse>();
            CreateMap<Cliente, ClienteResponse>();
            CreateMap<UsuarioCliente, UsuarioClienteResponse>();
            CreateMap<Ticket, TicketResponse>();
            CreateMap<TicketMensagem, TicketMensagemResponse>();
            CreateMap<TemplateResposta, TemplateRespostaResponse>();
        }
    }
}