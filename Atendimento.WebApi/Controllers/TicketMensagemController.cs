using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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
    [RoutePrefix("api/TicketMensagem")]
    [Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TicketMensagemController : ApiController
    {
        private readonly ITicketMensagemBusiness _business;
        private readonly IAnexoBusiness _anexoBusiness;
        private IHttpActionResult _result;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="business"></param>
        public TicketMensagemController(ITicketMensagemBusiness business,
                                        IAnexoBusiness anexoBusiness)
        {
            _business = business;
            _anexoBusiness = anexoBusiness;
        }

        /// GET: api/TicketMensagem
        [Route("GetAll")]
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            try
            {
                //Mapeia os dados da fonte (source class) para o destino (destiny class)
                IEnumerable<TicketMensagemResponse> lista = _business.GetAll().ToList().Select(Mapper.Map<TicketMensagem, TicketMensagemResponse>);

                int totalRegistros = lista.Count();

                //Monta response
                _result = Ok(Retorno<IEnumerable<TicketMensagemResponse>>.Criar(true, "Consulta Realizada Com Sucesso", lista, totalRegistros, totalRegistros));

                //Retorna o response
                return _result;
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        /// GET: api/TicketMensagem
        [Route("GetAllByTicketId")]
        [HttpGet]
        public IHttpActionResult GetAllByTicketId(int idTicket)
        {
            try
            {
                //Mapeia os dados da fonte (source class) para o destino (destiny class)
                IEnumerable<TicketMensagemResponse> lista = _business.GetAllByTicketId(idTicket).ToList();

                int totalRegistros = lista.Count();

                //Monta response
                _result = Ok(Retorno<IEnumerable<TicketMensagemResponse>>.Criar(true, "Consulta Realizada Com Sucesso", lista, totalRegistros, totalRegistros));

                //Retorna o response
                return _result;
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        /// GET: api/TicketMensagem/5
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
                _result = Ok(Retorno<TicketMensagemResponse>.Criar(true, "Consulta Realizada Com Sucesso", Mapper.Map<TicketMensagem, TicketMensagemResponse>(entity)));

                //Retorna o response
                return _result;
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        /// POST: api/TicketMensagem
        [Route("Insert")]
        [HttpPost]
        public IHttpActionResult Insert([FromBody]TicketMensagemRequest request)
        {
            try
            {
                //Valida objeto
                if (!ModelState.IsValid)
                    return BadRequest("Dados inválidos.");

                var entity = Mapper.Map<TicketMensagemRequest, TicketMensagem>(request);
                var pathAnexosUsuario = request.PathAnexos;

                if (request.TipoUsuario == "Atendimento")
                {
                    entity.IdAtendenteEmpresa = request.IdAutor;
                }
                else
                {
                    if (request.TipoUsuario == "Cliente")
                    {
                        entity.IdUsuarioCliente = request.IdAutor;
                    }
                }

                _business.Insert(ref entity);

                if (entity.Id > 0)
                {
                    //Monta response
                    _result = Ok(Retorno<TicketMensagemResponse>.Criar(true, "Inclusão Realizada Com Sucesso", Mapper.Map<TicketMensagem, TicketMensagemResponse>(entity)));

                    if (Directory.Exists(pathAnexosUsuario))
                    {
                        //Zipa todos os anexos
                        string zipName = Arquivo.Compress(ConfigurationManager.AppSettings["CaminhoFisicoAnexo"], pathAnexosUsuario, entity.Id);

                        //======================================
                        //Guarda anexo (zip) no banco de dados
                        //======================================
                        Anexo anexo = new Anexo
                        {
                            IdTicketMensagem = entity.Id,
                            Nome = zipName
                        };

                        _anexoBusiness.Insert(ref anexo);
                    }

                    //PAREI AQUI !!! ENVIAR EMAIL PARA O CLIENTE ASSOCIADO AO TICKET. VER SE NO SISTEMA ATUAL, É ENVIADO EMAIL TB PRA ALTERAÇÃO DO STATUS DO TICKET
                }

                //Retorna o response
                return _result;
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        /// POST: api/TicketMensagem
        [Route("InsertList")]
        [HttpPost]
        public IHttpActionResult InsertList(IEnumerable<TicketMensagemRequest> list)
        {
            IList<TicketMensagem> entityList = new List<TicketMensagem>();

            try
            {
                //Valida objeto
                if (!ModelState.IsValid)
                    return BadRequest("Dados inválidos.");

                foreach (var item in list)
                {
                    var entity = Mapper.Map<TicketMensagemRequest, TicketMensagem>(item);

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

        /// PUT: api/TicketMensagem/5
        [Route("Update")]
        [HttpPut]
        public IHttpActionResult Update(int id, [FromBody]TicketMensagemRequest request)
        {
            try
            {
                //Valida objeto
                if (!ModelState.IsValid)
                    return BadRequest("Dados inválidos.");

                TicketMensagem entityInDb = _business.GetById(id);

                //Verifica se objeto existe
                if (entityInDb == null)
                    return NotFound();

                //Mapeio os dados do dto para o objeto recuperado do banco, atualizando os dados do objeto do banco
                Mapper.Map(request, entityInDb);

                if (_business.Update(entityInDb))
                {
                    //Monta response
                    _result = Ok(Retorno<TicketMensagem>.Criar(true, "Atualização Realizada Com Sucesso", entityInDb));

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

        /// PUT: api/TicketMensagem/5
        [Route("UpdateList")]
        [HttpPut]
        public IHttpActionResult UpdateList(IEnumerable<TicketMensagemUpdate> list)
        {
            IList<TicketMensagem> entityList = new List<TicketMensagem>();

            try
            {
                //Valida objeto
                if (!ModelState.IsValid)
                    return BadRequest("Dados inválidos.");

                foreach (var item in list)
                {
                    TicketMensagem entityInDb = _business.GetById(item.Id);

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

        /// DELETE: api/TicketMensagem/5
        [Route("Delete")]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                TicketMensagem entityInDb = _business.GetById(id);

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

        /// DELETE: api/TicketMensagem/5
        [Route("DeleteList")]
        [HttpDelete]
        public IHttpActionResult DeleteList([FromBody]int[] list)
        {
            try
            {
                IList<TicketMensagem> entityList = new List<TicketMensagem>();

                try
                {
                    foreach (var id in list)
                    {
                        TicketMensagem entityInDb = _business.GetById(id);

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
