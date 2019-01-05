using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using Atendimento.Business.Interfaces.Interfaces;
using Atendimento.Entities.Entities;
using Atendimento.Entities.Requests;
using Atendimento.Entities.Responses;
using Atendimento.Infra;
using AutoMapper;

namespace Atendimento.WebApi.Controllers
{
    /// <summary>
    /// Ticket Controller
    /// </summary>
    [RoutePrefix("api/Ticket")]
    [Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TicketController : ApiController
    {
        private readonly ITicketBusiness _business;
        private readonly IAnexoBusiness _anexoBusiness;
        //private readonly IAnexoTicketBusiness _anexoTicketbusiness;
        private IHttpActionResult _result;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="business"></param>
        /// <param name="anexoBusiness"></param>
        public TicketController(ITicketBusiness business, 
                                //IAnexoTicketBusiness anexoTicketbusiness, 
                                IAnexoBusiness anexoBusiness)
        {
            _business = business;
            //_anexoTicketbusiness = anexoTicketbusiness;
            _anexoBusiness = anexoBusiness;
        }

        /// GET: api/Ticket
        [Route("GetAll")]
        [HttpGet]
        //[AllowAnonymous]
        public IHttpActionResult GetAll()
        {
            try
            {
                //Mapeia os dados da fonte (source class) para o destino (destiny class)
                IEnumerable<TicketResponse> lista = _business.GetAll().ToList().Select(Mapper.Map<Ticket, TicketResponse>);

                int totalRegistros = lista.Count();

                //Monta response
                _result = Ok(Retorno<IEnumerable<TicketResponse>>.Criar(true, "Consulta Realizada Com Sucesso", lista, totalRegistros, totalRegistros));

                //Retorna o response
                return _result;
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        /// GET: api/Ticket
        [Route("GetById")]
        [HttpGet]
        //[AllowAnonymous]
        public IHttpActionResult GetById(int id)
        {
            try
            {
                var entity = _business.GetByIdWithAnexos(id);

                if (entity == null)
                    return NotFound();

                //Monta response
                _result = Ok(Retorno<TicketResponse>.Criar(true, "Consulta Realizada Com Sucesso", entity));

                //Retorna o response
                return _result;
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        /// GET: api/Ticket
        [Route("GetCount")]
        [HttpGet]
        //[AllowAnonymous]
        public IHttpActionResult GetCount(int idStatusTicket)
        {
            try
            {
                //Recupera o total de registros de tickets de acordo com o filtro informado
                int total = _business.GetCount(idStatusTicket);

                //Monta response
                _result = Ok(Retorno<int>.Criar(true, "Consulta Realizada Com Sucesso", total));

                //Retorna o response
                return _result;
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        /// GET: api/Ticket
        [Route("GetCounts")]
        [HttpGet]
        //[Authorize]
        public IHttpActionResult GetCounts()
        {
            try
            {
                //Recupera todos os totais de registros de tickets
                CountsResponse response = _business.GetCounts();

                //Monta response
                _result = Ok(Retorno<CountsResponse>.Criar(true, "Consulta Realizada Com Sucesso", response, response.Totais.Count()));

                //Retorna o response
                return _result;
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        /// GET: api/Ticket
        [Route("GetAllPaged")]
        [HttpPost]
        //[AllowAnonymous]
        public IHttpActionResult GetAllPaged(FilterRequest advancedFilter)
        {
            try
            {
                //Mapeia os dados da fonte (source class) para o destino (destiny class)
                //IEnumerable<TicketDTO> lista = _business.GetAllPaged(advancedFilter).ToList().Select(Mapper.Map<Ticket, TicketDTO>);

                TicketsResponse result = _business.GetAllPaged(advancedFilter);

                IEnumerable<TicketResponse> lista = result.Tickets.ToList().Select(Mapper.Map<Ticket, TicketResponse>);
                int totalGeral = result.TotalGeral;
                int totalFiltrado = result.TotalFiltrado;
                int totalLinhas = lista.Count();

                //Monta response
                _result = Ok(Retorno<IEnumerable<TicketResponse>>.Criar(true, "Consulta Realizada Com Sucesso", lista, totalGeral, totalFiltrado));

                //Retorna o response
                return _result;
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        /// POST: api/Ticket
        [Route("Insert")]
        [HttpPost]
        //[AllowAnonymous]
        public IHttpActionResult Insert([FromBody]TicketRequest request)
        {
            try
            {
                //Valida objeto
                if (!ModelState.IsValid)
                    return BadRequest("Dados inválidos.");

                var entity = Mapper.Map<TicketRequest, Ticket>(request);
                var pathAnexosUsuario = request.PathAnexos;

                entity.DataHoraInicial = DateTime.Now;

                _business.Insert(ref entity);

                if (entity.Id > 0)
                {
                    //Monta response
                    _result = Ok(Retorno<TicketResponse>.Criar(true, "Inclusão Realizada Com Sucesso", Mapper.Map<Ticket, TicketResponse>(entity)));

                    //Zipa todos os anexos
                    string zipName = Arquivo.Compress(ConfigurationManager.AppSettings["CaminhoFisicoAnexo"], pathAnexosUsuario, entity.Id);

                    //======================================
                    //Guarda anexo (zip) no banco de dados
                    //======================================
                    Anexo anexo = new Anexo
                    {
                        IdTicket = entity.Id,
                        Nome = zipName
                    };
                                        
                    _anexoBusiness.Insert(ref anexo);
                }

                //Retorna o response
                return _result;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        /// POST: api/Ticket
        [Route("InsertList")]
        [HttpPost]
        //[AllowAnonymous]
        public IHttpActionResult InsertList(IEnumerable<TicketRequest> list)
        {
            IList<Ticket> entityList = new List<Ticket>();

            try
            {
                //Valida objeto
                if (!ModelState.IsValid)
                    return BadRequest("Dados inválidos.");

                foreach (var item in list)
                {
                    var entity = Mapper.Map<TicketRequest, Ticket>(item);

                    entity.DataHoraInicial = DateTime.Now;
                    entityList.Add(entity);
                }

                int rows = _business.Insert(entityList);

                if (rows > 0)
                {
                    //Monta response
                    _result = Ok(Retorno<int>.Criar(true, "Inclusão de Lista Realizada Com Sucesso", rows));

                    //Retorna o response
                    return _result;
                }
                else
                    return BadRequest("Nenhum registro inserido. Verifique os dados enviados.");
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        /// PUT: api/Ticket
        [Route("Update")]
        [HttpPut]
        //[AllowAnonymous]
        public IHttpActionResult Update(int id, [FromBody]TicketRequest request)
        {
            try
            {
                //Valida objeto
                if (!ModelState.IsValid)
                    return BadRequest("Dados inválidos.");

                Ticket entityInDb = _business.GetById(id);

                //Verifica se objeto existe
                if (entityInDb == null)
                    return NotFound();

                //Mapeio os dados do dto para o objeto recuperado do banco, atualizando os dados do objeto do banco
                Mapper.Map(request, entityInDb);

                entityInDb.DataHoraAlteracao = DateTime.Now;

                if (_business.Update(entityInDb))
                {
                    //Monta response
                    _result = Ok(Retorno<Ticket>.Criar(true, "Atualização Realizada Com Sucesso", entityInDb));

                    //Retorna o response
                    return _result;
                }
                else
                    return BadRequest("Nenhum registro atualizado. Verifique os dados enviados.");
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        /// PUT: api/Ticket
        [Route("UpdateStatusTicket")]
        [HttpPut]
        //[AllowAnonymous]
        public IHttpActionResult UpdateStatusTicket([FromBody]TicketUpdateStatusRequest request)
        {
            try
            {
                //Valida objeto
                if (!ModelState.IsValid)
                    return BadRequest("Dados inválidos.");

                Ticket entityInDb = _business.GetById(request.Id);

                //Verifica se objeto existe
                if (entityInDb == null)
                    return NotFound();

                entityInDb.IdStatusTicket = request.IdStatusTicket;

                if (_business.UpdateStatusTicket(entityInDb))
                {
                    //Recupera o ticket atualizado
                    entityInDb = _business.GetById(request.Id);

                    //Monta response
                    _result = Ok(Retorno<Ticket>.Criar(true, "Atualização Realizada Com Sucesso", entityInDb));

                    //Retorna o response
                    return _result;
                }
                else
                    return BadRequest("Nenhum registro atualizado. Verifique os dados enviados.");
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        /// PUT: api/Ticket
        [Route("UpdateList")]
        [HttpPut]
        //[AllowAnonymous]
        public IHttpActionResult UpdateList(IEnumerable<TicketUpdate> list)
        {
            IList<Ticket> entityList = new List<Ticket>();

            try
            {
                //Valida objeto
                if (!ModelState.IsValid)
                    return BadRequest("Dados inválidos.");

                foreach (var item in list)
                {
                    Ticket entityInDb = _business.GetById(item.Id);

                    //Verifica se objeto existe
                    if (entityInDb == null)
                        return BadRequest("Nenhum registro atualizado. Verifique os dados enviados.");
                    else
                    {
                        Mapper.Map(item, entityInDb);
                        entityInDb.DataHoraAlteracao = DateTime.Now;
                        entityList.Add(entityInDb);
                    }
                }

                if (_business.Update(entityList))
                {
                    //Monta response
                    _result = Ok(Retorno<bool>.Criar(true, "Atualização de Lista Realizada Com Sucesso", true));

                    //Retorna o response
                    return _result;
                }
                else
                    return BadRequest("Nenhum registro atualizado. Verifique os dados enviados.");
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        /// DELETE: api/Ticket/5
        [Route("Delete")]
        [HttpDelete]
        //[AllowAnonymous]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                Ticket entityInDb = _business.GetById(id);

                //Verifica se objeto existe
                if (entityInDb == null)
                    return NotFound();

                if (_business.Delete(id))
                {
                    //Monta response
                    _result = Ok(Retorno<bool>.Criar(true, "Deleção Realizada Com Sucesso", true));

                    //Retorna o response
                    return _result;
                }
                else
                    return BadRequest("Nenhum registro deletado. Verifique os dados enviados.");
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        /// DELETE: api/Ticket
        [Route("DeleteList")]
        [HttpDelete]
        //[AllowAnonymous]
        public IHttpActionResult DeleteList([FromBody]int[] list)
        {
            try
            {
                IList<Ticket> entityList = new List<Ticket>();

                try
                {
                    foreach (var id in list)
                    {
                        Ticket entityInDb = _business.GetById(id);

                        //Verifica se objeto existe
                        if (entityInDb == null)
                            return BadRequest("Nenhum registro deletado. Verifique os dados enviados.");
                        else
                            entityList.Add(entityInDb);
                    }

                    if (_business.Delete(entityList))
                    {
                        //Monta response
                        _result = Ok(Retorno<bool>.Criar(true, "Deleção de Lista Realizada Com Sucesso", true));

                        //Retorna o response
                        return _result;
                    }
                    else
                        return BadRequest("Nenhum registro deletado. Verifique os dados enviados.");
                }
                catch (Exception)
                {
                    throw new HttpResponseException(HttpStatusCode.InternalServerError);
                }
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }
    }
}
