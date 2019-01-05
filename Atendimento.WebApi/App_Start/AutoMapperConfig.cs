using System.Web.Mvc;
using Atendimento.Business.Interfaces.Interfaces;
using Atendimento.WebApi.Mappers;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Atendimento.WebApi.App_Start
{
    /// <summary>
    /// AutoMapper Configuration
    /// </summary>
    public static class AutoMapperConfig
    {
        /// <summary>
        /// AutoMapper Register
        /// </summary>
        public static void RegisterMappings()
        {
            Mapper.Initialize(map =>
            {
                map.AddProfile<DomainToResponseMappingProfile>();
                map.AddProfile<RequestToDomainMappingProfile>();
            });
        }
    }
}