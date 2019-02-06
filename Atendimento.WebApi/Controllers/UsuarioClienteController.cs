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
    /// UsuarioCliente Controller
    /// </summary>
    [RoutePrefix("api/UsuarioCliente")]
    [Authorize]
    public class UsuarioClienteController : ApiController
    {
        private readonly IUsuarioClienteBusiness _usuarioClienteBusiness;
        private IHttpActionResult _result;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="usuarioClienteBusiness"></param>
        public UsuarioClienteController(IUsuarioClienteBusiness usuarioClienteBusiness)
        {
            _usuarioClienteBusiness = usuarioClienteBusiness;
        }

        /// <summary>
        /// Recupera todos os usuarios de cliente
        /// </summary>
        /// <returns></returns>
        [Route(nameof(GetAll))]
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            try
            {
                //Mapeia os dados da fonte (source class) para o destino (destiny class)
                var lista = _usuarioClienteBusiness.GetAll().ToList().Select(Mapper.Map<UsuarioCliente, UsuarioClienteResponse>);

                var totalRegistros = lista.Count();

                //Monta response
                _result = Ok(Retorno<IEnumerable<UsuarioClienteResponse>>.Criar(true, "Consulta Realizada Com Sucesso", lista, totalRegistros, totalRegistros));

                //Retorna o response
                return _result;
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Recupera todos os usuarios associados a um terminado cliente
        /// </summary>
        /// <param name="idCliente"></param>
        /// <returns></returns>
        [Route(nameof(GetAllById))]
        [HttpGet]
        public IHttpActionResult GetAllById(int idCliente)
        {
            try
            {
                var lista = _usuarioClienteBusiness.GetAllById(idCliente).ToList().Select(Mapper.Map<UsuarioCliente, UsuarioClienteResponse>);

                var totalRegistros = lista.Count();

                //Monta response
                _result = Ok(Retorno<IEnumerable<UsuarioClienteResponse>>.Criar(true, "Consulta Realizada Com Sucesso", lista, totalRegistros, totalRegistros));

                //Retorna o response
                return _result;
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Recupera um usuario do cliente
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route(nameof(GetById))]
        [HttpGet]
        public IHttpActionResult GetById(int id)
        {
            try
            {
                var entity = _usuarioClienteBusiness.GetById(id);

                if (entity == null)
                    return NotFound();

                //Monta response
                _result = Ok(Retorno<UsuarioClienteResponse>.Criar(true, "Consulta Realizada Com Sucesso", Mapper.Map<UsuarioCliente, UsuarioClienteResponse>(entity)));

                //Retorna o response
                return _result;
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Recupera total de tickets
        /// </summary>
        /// <param name="idCliente"></param>
        /// <returns></returns>
        [Route(nameof(GetCount))]
        [HttpGet]
        //[AllowAnonymous]
        public IHttpActionResult GetCount(int idCliente)
        {
            try
            {
                //Recupera o total de registros de usuarios associados ao cliente
                var total = _usuarioClienteBusiness.GetCount(idCliente);

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

        /// <summary>
        /// Recupera lista paginada de usuarios
        /// </summary>
        /// <param name="advancedFilter"></param>
        /// <returns></returns>
        [Route(nameof(GetAllPaged))]
        [HttpPost]
        public IHttpActionResult GetAllPaged(FilterUsuarioRequest advancedFilter)
        {
            try
            {
                var result = _usuarioClienteBusiness.GetAllPaged(advancedFilter);

                var lista = result.Usuarios.ToList().Select(Mapper.Map<UsuarioCliente, UsuarioClienteResponse>);
                var totalGeral = result.TotalGeral;
                var totalLinhas = lista.Count();

                //Monta response
                _result = Ok(Retorno<IEnumerable<UsuarioClienteResponse>>.Criar(true, "Consulta Realizada Com Sucesso", lista, totalGeral));

                //Retorna o response
                return _result;
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Insere usuario do cliente
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route(nameof(Insert))]
        [HttpPost]
        public IHttpActionResult Insert([FromBody]UsuarioClienteRequest request)
        {
            try
            {
                //Valida objeto
                if (!ModelState.IsValid)
                    return BadRequest("Dados inválidos.");

                var entity = Mapper.Map<UsuarioClienteRequest, UsuarioCliente>(request);

                _usuarioClienteBusiness.Insert(ref entity);

                if (entity.Id > 0)
                {
                    //Monta response
                    _result = Ok(Retorno<UsuarioClienteResponse>.Criar(true, "Inclusão Realizada Com Sucesso", Mapper.Map<UsuarioCliente, UsuarioClienteResponse>(entity)));
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
        /// Insere lista de usuarios do cliente
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [Route(nameof(InsertList))]
        [HttpPost]
        public IHttpActionResult InsertList(IEnumerable<UsuarioClienteRequest> list)
        {
            IList<UsuarioCliente> entityList = new List<UsuarioCliente>();

            try
            {
                //Valida objeto
                if (!ModelState.IsValid)
                    return BadRequest("Dados inválidos.");

                foreach (var item in list)
                {
                    var entity = Mapper.Map<UsuarioClienteRequest, UsuarioCliente>(item);

                    entityList.Add(entity);
                }

                var rows = _usuarioClienteBusiness.Insert(entityList);

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
        /// Atualiza usuario do cliente
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route(nameof(Update))]
        [HttpPut]
        public IHttpActionResult Update(int id, [FromBody]UsuarioClienteRequest request)
        {
            try
            {
                //Valida objeto
                if (!ModelState.IsValid)
                    return BadRequest("Dados inválidos.");

                var entityInDb = _usuarioClienteBusiness.GetById(id);

                //Verifica se objeto existe
                if (entityInDb == null)
                    return NotFound();

                //Mapeio os dados do dto para o objeto recuperado do banco, atualizando os dados do objeto do banco
                Mapper.Map(request, entityInDb);

                if (_usuarioClienteBusiness.Update(entityInDb))
                {
                    //Monta response
                    _result = Ok(Retorno<UsuarioCliente>.Criar(true, "Atualização Realizada Com Sucesso", entityInDb));

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
        /// Atualiza lista de usuarios do cliente
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [Route(nameof(UpdateList))]
        [HttpPut]
        public IHttpActionResult UpdateList(IEnumerable<UsuarioClienteUpdate> list)
        {
            var entityList = new List<UsuarioCliente>();

            try
            {
                //Valida objeto
                if (!ModelState.IsValid)
                    return BadRequest("Dados inválidos.");

                foreach (var item in list)
                {
                    var entityInDb = _usuarioClienteBusiness.GetById(item.Id);

                    //Verifica se objeto existe
                    if (entityInDb == null)
                        return BadRequest("Nenhum registro atualizado. Verifique os dados enviados.");
                    else
                    {
                        Mapper.Map(item, entityInDb);
                        entityList.Add(entityInDb);
                    }
                }

                if (_usuarioClienteBusiness.Update(entityList))
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
        /// deleta usuario do cliente
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route(nameof(Delete))]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                var entityInDb = _usuarioClienteBusiness.GetById(id);

                //Verifica se objeto existe
                if (entityInDb == null)
                    return NotFound();

                if (_usuarioClienteBusiness.Delete(id))
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
        /// Deleta lista de usuarios do cliente
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [Route(nameof(DeleteList))]
        [HttpDelete]
        public IHttpActionResult DeleteList([FromBody]int[] list)
        {
            try
            {
                IList<UsuarioCliente> entityList = new List<UsuarioCliente>();

                try
                {
                    foreach (var id in list)
                    {
                        var entityInDb = _usuarioClienteBusiness.GetById(id);

                        //Verifica se objeto existe
                        if (entityInDb == null)
                            return BadRequest("Nenhum registro deletado. Verifique os dados enviados.");
                        else
                            entityList.Add(entityInDb);
                    }

                    if (_usuarioClienteBusiness.Delete(entityList))
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
