using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Atendimento.Business.Interfaces.Interfaces;
using Atendimento.Entities.Entities;
using Atendimento.Entities.Requests;
using Atendimento.Entities.Responses;
using AutoMapper;

namespace Atendimento.WebApi.Controllers
{
    /// <summary>
    /// Cliente Controller
    /// </summary>
    [RoutePrefix("api/Cliente")]
    [Authorize]
    public class ClienteController : ApiController
    {
        private readonly IClienteBusiness _clienteBusiness;
        private IHttpActionResult _result;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="clienteBusiness"></param>
        public ClienteController(IClienteBusiness clienteBusiness)
        {
            _clienteBusiness = clienteBusiness;
        }

        /// <summary>
        /// Recupera todos os clientes
        /// </summary>
        /// <returns></returns>
        [Route(nameof(GetAll))]
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            try
            {
                //Mapeia os dados da fonte (source class) para o destino (destiny class)
                var lista = _clienteBusiness.GetAll().ToList().Select(Mapper.Map<Cliente, ClienteResponse>);

                var totalRegistros = lista.Count();

                //Monta response
                _result = Ok(Retorno<IEnumerable<ClienteResponse>>.Criar(true, "Consulta Realizada Com Sucesso", lista, totalRegistros, totalRegistros));

                //Retorna o response
                return _result;
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Recupera um cliente
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route(nameof(GetById))]
        [HttpGet]
        public IHttpActionResult GetById(int id)
        {
            try
            {
                var entity = _clienteBusiness.GetById(id);

                if (entity == null)
                    return NotFound();

                //Monta response
                _result = Ok(Retorno<ClienteResponse>.Criar(true, "Consulta Realizada Com Sucesso", Mapper.Map<Cliente, ClienteResponse>(entity)));

                //Retorna o response
                return _result;
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Recupera lista paginada de clientes
        /// </summary>
        /// <param name="advancedFilter"></param>
        /// <returns></returns>
        [Route(nameof(GetAllPaged))]
        [HttpPost]
        public IHttpActionResult GetAllPaged(FilterClienteRequest advancedFilter)
        {
            try
            {
                var result = _clienteBusiness.GetAllPaged(advancedFilter);

                var lista = result.Clientes.ToList().Select(Mapper.Map<Cliente, ClienteResponse>);
                var totalGeral = result.TotalGeral;
                var totalLinhas = lista.Count();

                //Monta response
                _result = Ok(Retorno<IEnumerable<ClienteResponse>>.Criar(true, "Consulta Realizada Com Sucesso", lista, totalGeral));

                //Retorna o response
                return _result;
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Insere cliente
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route(nameof(Insert))]
        [HttpPost]
        public IHttpActionResult Insert([FromBody]ClienteRequest request)
        {
            try
            {
                //Valida objeto
                if (!ModelState.IsValid)
                    return BadRequest("Dados inválidos.");

                var entity = Mapper.Map<ClienteRequest, Cliente>(request);

                _clienteBusiness.Insert(ref entity);

                if (entity.Id > 0)
                {
                    //Monta response
                    _result = Ok(Retorno<ClienteResponse>.Criar(true, "Inclusão Realizada Com Sucesso", Mapper.Map<Cliente, ClienteResponse>(entity)));
                }

                //Retorna o response
                return _result;
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Insere lista de clientes
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [Route(nameof(InsertList))]
        [HttpPost]
        public IHttpActionResult InsertList(IEnumerable<ClienteRequest> list)
        {
            IList<Cliente> entityList = new List<Cliente>();

            try
            {
                //Valida objeto
                if (!ModelState.IsValid)
                    return BadRequest("Dados inválidos.");

                foreach (var item in list)
                {
                    var entity = Mapper.Map<ClienteRequest, Cliente>(item);

                    entityList.Add(entity);
                }

                var rows = _clienteBusiness.Insert(entityList);

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

        /// <summary>
        /// Atualiza cliente
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route(nameof(Update))]
        [HttpPut]
        public IHttpActionResult Update(int id, [FromBody]ClienteRequest request)
        {
            try
            {
                //Valida objeto
                if (!ModelState.IsValid)
                    return BadRequest("Dados inválidos.");

                var entityInDb = _clienteBusiness.GetById(id);

                //Verifica se objeto existe
                if (entityInDb == null)
                    return NotFound();

                //Mapeio os dados do dto para o objeto recuperado do banco, atualizando os dados do objeto do banco
                Mapper.Map(request, entityInDb);

                if (_clienteBusiness.Update(entityInDb))
                {
                    //Monta response
                    _result = Ok(Retorno<Cliente>.Criar(true, "Atualização Realizada Com Sucesso", entityInDb));

                    //Retorna o response
                    return _result;
                }
                else
                    return BadRequest("Nenhum registro atualizado. Verifique os dados enviados.");
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Atualiza lista de clientes
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [Route(nameof(UpdateList))]
        [HttpPut]
        public IHttpActionResult UpdateList(IEnumerable<ClienteUpdate> list)
        {
            IList<Cliente> entityList = new List<Cliente>();

            try
            {
                //Valida objeto
                if (!ModelState.IsValid)
                    return BadRequest("Dados inválidos.");

                foreach (var item in list)
                {
                    var entityInDb = _clienteBusiness.GetById(item.Id);

                    //Verifica se objeto existe
                    if (entityInDb == null)
                        return BadRequest("Nenhum registro atualizado. Verifique os dados enviados.");
                    else
                    {
                        Mapper.Map(item, entityInDb);
                        entityList.Add(entityInDb);
                    }
                }

                if (_clienteBusiness.Update(entityList))
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

        /// <summary>
        /// Deleta cliente
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route(nameof(Delete))]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                var entityInDb = _clienteBusiness.GetById(id);

                //Verifica se objeto existe
                if (entityInDb == null)
                    return NotFound();

                if (_clienteBusiness.Delete(id))
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

        /// <summary>
        /// Deleta lista de clientes
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [Route(nameof(DeleteList))]
        [HttpDelete]
        public IHttpActionResult DeleteList([FromBody]int[] list)
        {
            try
            {
                IList<Cliente> entityList = new List<Cliente>();

                try
                {
                    foreach (var id in list)
                    {
                        var entityInDb = _clienteBusiness.GetById(id);

                        //Verifica se objeto existe
                        if (entityInDb == null)
                            return BadRequest("Nenhum registro deletado. Verifique os dados enviados.");
                        else
                            entityList.Add(entityInDb);
                    }

                    if (_clienteBusiness.Delete(entityList))
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
