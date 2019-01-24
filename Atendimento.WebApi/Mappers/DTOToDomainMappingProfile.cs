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
    /// DTO -> Domain Mapping
    /// </summary>
    public class DTOToDomainMappingProfile : Profile
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DTOToDomainMappingProfile()
        {
            CreateMap<CategoriaDTO, Categoria>();
            //CreateMap<ClassificacaoDTO, Classificacao>();
            //CreateMap<StatusTicketDTO, StatusTicket>();
        }
    }
}