﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using Atendimento.Business.Interfaces.Interfaces;
using Atendimento.Entities.Entities;
using Atendimento.Entities.Requests;
using Atendimento.Entities.Responses;
using AutoMapper;

namespace Atendimento.WebApi.Controllers
{
    /// <summary>
    /// StatusTicket Controller
    /// </summary>
    [RoutePrefix("api/StatusTicket")]
    [Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class StatusTicketController : ApiController
    {
        private readonly IStatusTicketBusiness _business;
        private IHttpActionResult _result;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="business"></param>
        public StatusTicketController(IStatusTicketBusiness business)
        {
            _business = business;
        }

        /// GET: api/StatusTicket
        [Route("GetAll")]
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            try
            {
                //Mapeia os dados da fonte (source class) para o destino (destiny class)
                IEnumerable<StatusTicketResponse> lista = _business.GetAll().ToList().Select(Mapper.Map<StatusTicket, StatusTicketResponse>);

                int totalRegistros = lista.Count();

                //Monta response
                _result = Ok(Retorno<IEnumerable<StatusTicketResponse>>.Criar(true, "Consulta Realizada Com Sucesso", lista, totalRegistros, totalRegistros));

                //Retorna o response
                return _result;
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        /// GET: api/StatusTicket/5
        [Route("GetById")]
        [HttpGet]
        public IHttpActionResult GetById(int id)
        {
            try
            {
                var entity = _business.GetById(id);

                if (entity == null)
                    return NotFound();

                //Monta response
                _result = Ok(Retorno<StatusTicketResponse>.Criar(true, "Consulta Realizada Com Sucesso", Mapper.Map<StatusTicket, StatusTicketResponse>(entity)));

                //Retorna o response
                return _result;
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        /// POST: api/StatusTicket
        [Route("Insert")]
        [HttpPost]
        public IHttpActionResult Insert([FromBody]StatusTicketRequest request)
        {
            try
            {
                //Valida objeto
                if (!ModelState.IsValid)
                    return BadRequest("Dados inválidos.");

                var entity = Mapper.Map<StatusTicketRequest, StatusTicket>(request);

                _business.Insert(ref entity);

                if (entity.Id > 0)
                {
                    //Monta response
                    _result = Ok(Retorno<StatusTicketResponse>.Criar(true, "Inclusão Realizada Com Sucesso", Mapper.Map<StatusTicket, StatusTicketResponse>(entity)));
                }

                //Retorna o response
                return _result;
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        /// POST: api/StatusTicket
        [Route("InsertList")]
        [HttpPost]
        public IHttpActionResult InsertList(IEnumerable<StatusTicketRequest> list)
        {
            IList<StatusTicket> entityList = new List<StatusTicket>();

            try
            {
                //Valida objeto
                if (!ModelState.IsValid)
                    return BadRequest("Dados inválidos.");

                foreach (var item in list)
                {
                    var entity = Mapper.Map<StatusTicketRequest, StatusTicket>(item);

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

        /// PUT: api/StatusTicket/5
        [Route("Update")]
        [HttpPut]
        public IHttpActionResult Update(int id, [FromBody]StatusTicketRequest request)
        {
            try
            {
                //Valida objeto
                if (!ModelState.IsValid)
                    return BadRequest("Dados inválidos.");

                StatusTicket entityInDb = _business.GetById(id);

                //Verifica se objeto existe
                if (entityInDb == null)
                    return NotFound();

                //Mapeio os dados do dto para o objeto recuperado do banco, atualizando os dados do objeto do banco
                Mapper.Map(request, entityInDb);

                if (_business.Update(entityInDb))
                {
                    //Monta response
                    _result = Ok(Retorno<StatusTicket>.Criar(true, "Atualização Realizada Com Sucesso", entityInDb));

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

        /// PUT: api/StatusTicket/5
        [Route("UpdateList")]
        [HttpPut]
        public IHttpActionResult UpdateList(IEnumerable<StatusTicketUpdate> list)
        {
            IList<StatusTicket> entityList = new List<StatusTicket>();

            try
            {
                //Valida objeto
                if (!ModelState.IsValid)
                    return BadRequest("Dados inválidos.");

                foreach (var item in list)
                {
                    StatusTicket entityInDb = _business.GetById(item.Id);

                    //Verifica se objeto existe
                    if (entityInDb == null)
                        return BadRequest("Nenhum registro atualizado. Verifique os dados enviados.");
                    else
                    {
                        Mapper.Map(item, entityInDb);
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

        /// DELETE: api/StatusTicket/5
        [Route("Delete")]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                StatusTicket entityInDb = _business.GetById(id);

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

        /// DELETE: api/StatusTicket/5
        [Route("DeleteList")]
        [HttpDelete]
        public IHttpActionResult DeleteList([FromBody]int[] list)
        {
            try
            {
                IList<StatusTicket> entityList = new List<StatusTicket>();

                try
                {
                    foreach (var id in list)
                    {
                        StatusTicket entityInDb = _business.GetById(id);

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
