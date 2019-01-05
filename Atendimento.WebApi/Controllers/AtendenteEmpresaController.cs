using System;
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
    /// AtendenteEmpresa Controller
    /// </summary>
    [RoutePrefix("api/AtendenteEmpresa")]
    [Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AtendenteEmpresaController : ApiController
    {
        private readonly IAtendenteEmpresaBusiness _business;
        private IHttpActionResult _result;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="business"></param>
        public AtendenteEmpresaController(IAtendenteEmpresaBusiness business)
        {
            _business = business;
        }

        /// GET: api/AtendenteEmpresa
        [Route("GetAll")]
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            try
            {
                //Mapeia os dados da fonte (source class) para o destino (destiny class)
                IEnumerable<AtendenteEmpresaResponse> lista = _business.GetAll().ToList().Select(Mapper.Map<AtendenteEmpresa, AtendenteEmpresaResponse>);

                int totalRegistros = lista.Count();

                //Monta response
                _result = Ok(Retorno<IEnumerable<AtendenteEmpresaResponse>>.Criar(true, "Consulta Realizada Com Sucesso", lista, totalRegistros, totalRegistros));

                //Retorna o response
                return _result;
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        /// GET: api/AtendenteEmpresa/5
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
                _result = Ok(Retorno<AtendenteEmpresaResponse>.Criar(true, "Consulta Realizada Com Sucesso", Mapper.Map<AtendenteEmpresa, AtendenteEmpresaResponse>(entity)));

                //Retorna o response
                return _result;
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        /// POST: api/AtendenteEmpresa
        [Route("Insert")]
        [HttpPost]
        public IHttpActionResult Insert([FromBody]AtendenteEmpresaRequest request)
        {
            try
            {
                //Valida objeto
                if (!ModelState.IsValid)
                    return BadRequest("Dados inválidos.");

                var entity = Mapper.Map<AtendenteEmpresaRequest, AtendenteEmpresa>(request);

                _business.Insert(ref entity);

                if (entity.Id > 0)
                {
                    //Monta response
                    _result = Ok(Retorno<AtendenteEmpresaResponse>.Criar(true, "Inclusão Realizada Com Sucesso", Mapper.Map<AtendenteEmpresa, AtendenteEmpresaResponse>(entity)));
                }

                //Retorna o response
                return _result;
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        /// POST: api/AtendenteEmpresa
        [Route("InsertList")]
        [HttpPost]
        public IHttpActionResult InsertList(IEnumerable<AtendenteEmpresaRequest> list)
        {
            IList<AtendenteEmpresa> entityList = new List<AtendenteEmpresa>();

            try
            {
                //Valida objeto
                if (!ModelState.IsValid)
                    return BadRequest("Dados inválidos.");

                foreach (var item in list)
                {
                    var entity = Mapper.Map<AtendenteEmpresaRequest, AtendenteEmpresa>(item);

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

        /// PUT: api/AtendenteEmpresa/5
        [Route("Update")]
        [HttpPut]
        public IHttpActionResult Update(int id, [FromBody]AtendenteEmpresaRequest request)
        {
            try
            {
                //Valida objeto
                if (!ModelState.IsValid)
                    return BadRequest("Dados inválidos.");

                AtendenteEmpresa entityInDb = _business.GetById(id);

                //Verifica se objeto existe
                if (entityInDb == null)
                    return NotFound();

                //Mapeio os dados do dto para o objeto recuperado do banco, atualizando os dados do objeto do banco
                Mapper.Map(request, entityInDb);

                if (_business.Update(entityInDb))
                {
                    //Monta response
                    _result = Ok(Retorno<AtendenteEmpresa>.Criar(true, "Atualização Realizada Com Sucesso", entityInDb));

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

        /// PUT: api/AtendenteEmpresa/5
        [Route("UpdateList")]
        [HttpPut]
        public IHttpActionResult UpdateList(IEnumerable<AtendenteEmpresaUpdate> list)
        {
            IList<AtendenteEmpresa> entityList = new List<AtendenteEmpresa>();

            try
            {
                //Valida objeto
                if (!ModelState.IsValid)
                    return BadRequest("Dados inválidos.");

                foreach (var item in list)
                {
                    AtendenteEmpresa entityInDb = _business.GetById(item.Id);

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

        /// DELETE: api/AtendenteEmpresa/5
        [Route("Delete")]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                AtendenteEmpresa entityInDb = _business.GetById(id);

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

        /// DELETE: api/AtendenteEmpresa/5
        [Route("DeleteList")]
        [HttpDelete]
        public IHttpActionResult DeleteList([FromBody]int[] list)
        {
            try
            {
                IList<AtendenteEmpresa> entityList = new List<AtendenteEmpresa>();

                try
                {
                    foreach (var id in list)
                    {
                        AtendenteEmpresa entityInDb = _business.GetById(id);

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
