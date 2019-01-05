using Atendimento.Entities.Entities;
using Atendimento.Entities.Requests;
using AutoMapper;

namespace Atendimento.WebApi.Mappers
{
    /// <summary>
    /// Requests -> Domain Mapping
    /// </summary>
    public class RequestToDomainMappingProfile : Profile
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public RequestToDomainMappingProfile()
        {
            CreateMap<CategoriaRequest, Categoria>();
            CreateMap<CategoriaUpdate, Categoria>();
            CreateMap<ClassificacaoRequest, Classificacao>();
            CreateMap<ClassificacaoUpdate, Classificacao>();
            CreateMap<StatusTicketRequest, StatusTicket>();
            CreateMap<StatusTicketUpdate, StatusTicket>();
            CreateMap<GrupoRequest, Grupo>();
            CreateMap<GrupoUpdate, Grupo>();
            CreateMap<EmpresaRequest, Empresa>();
            CreateMap<EmpresaUpdate, Empresa>();
            CreateMap<AtendenteEmpresaRequest, AtendenteEmpresa>();
            CreateMap<AtendenteEmpresaUpdate, AtendenteEmpresa>();
            CreateMap<ClienteRequest, Cliente>();
            CreateMap<ClienteUpdate, Cliente>();
            CreateMap<UsuarioClienteRequest, UsuarioCliente>();
            CreateMap<UsuarioClienteUpdate, UsuarioCliente>();
            CreateMap<TicketRequest, Ticket>();
            CreateMap<TicketUpdate, Ticket>();
            CreateMap<TicketMensagemRequest, TicketMensagem>();
            CreateMap<TicketMensagemUpdate, TicketMensagem>();
            CreateMap<TemplateRespostaRequest, TemplateResposta>();
            CreateMap<TemplateRespostaUpdate, TemplateResposta>();
        }
    }
}