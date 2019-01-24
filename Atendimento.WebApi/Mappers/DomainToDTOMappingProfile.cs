using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Atendimento.Entities.Entities;
using Atendimento.WebApi.Dtos;
using AutoMapper;

namespace Atendimento.WebApi.Mappers
{
    /// <summary>
    /// Domain -> DTO Mapping
    /// </summary>
    public class DomainToDTOMappingProfile : Profile
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DomainToDTOMappingProfile()
        {
            CreateMap<Categoria, CategoriaDTO>();
            //CreateMap<Classificacao, ClassificacaoDTO>();
            //CreateMap<StatusTicket, StatusTicketDTO>();
        }
    }
}