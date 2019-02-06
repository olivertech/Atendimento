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
    /// AtendenteEmpresa Controller
    /// </summary>
    [RoutePrefix("api/AtendenteEmpresa")]
    [Authorize]
    public class AtendenteEmpresaController : ApiController
    {
        private readonly IAtendenteEmpresaBusiness _atendenteEmpresaBusiness;
        private IHttpActionResult _result;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="atendenteEmpresaBusiness"></param>
        public AtendenteEmpresaController(IAtendenteEmpresaBusiness atendenteEmpresaBusiness)
        {
            _atendenteEmpresaBusiness = atendenteEmpresaBusiness;
        }

        /// <summary>
        /// Recuperar todos os atendentes da empresa
        /// </summary>
        /// <returns></returns>
        [Route(nameof(GetAll))]
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            try
            {
                //Mapeia os dados da fonte (source class) para o destino (destiny class)
                var lista = _atendenteEmpresaBusiness.GetAll().ToList().Select(Mapper.Map<AtendenteEmpresa, AtendenteEmpresaResponse>);

                var totalRegistros = lista.Count();

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

        /// <summary>
        /// Recuperar um atendente
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route(nameof(GetById))]
        [HttpGet]
        public IHttpActionResult GetById(int id)
        {
            try
            {
                var entity = _atendenteEmpresaBusiness.GetById(id);

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

        /// <summary>
        /// Recupera lista paginada de empresas
        /// </summary>
        /// <param name="advancedFilter"></param>
        /// <returns></returns>
        [Route(nameof(GetAllPaged))]
        [HttpPost]
        public IHttpActionResult GetAllPaged(FilterAtendenteEmpresaRequest advancedFilter)
        {
            try
            {
                var result = _atendenteEmpresaBusiness.GetAllPaged(advancedFilter);

                var lista = result.Atendentes.ToList().Select(Mapper.Map<AtendenteEmpresa, AtendenteEmpresaResponse>);
                var totalGeral = result.TotalGeral;
                var totalLinhas = lista.Count();

                //Monta response
                _result = Ok(Retorno<IEnumerable<AtendenteEmpresaResponse>>.Criar(true, "Consulta Realizada Com Sucesso", lista, totalGeral));

                //Retorna o response
                return _result;
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Inserir atendente
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route(nameof(Insert))]
        [HttpPost]
        public IHttpActionResult Insert([FromBody]AtendenteEmpresaRequest request)
        {
            try
            {
                //Valida objeto
                if (!ModelState.IsValid)
                    return BadRequest("Dados inválidos.");

                var entity = Mapper.Map<AtendenteEmpresaRequest, AtendenteEmpresa>(request);

                _atendenteEmpresaBusiness.Insert(ref entity);

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

        /// <summary>
        /// Inserir lista de atendentes
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [Route(nameof(InsertList))]
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

                var rows = _atendenteEmpresaBusiness.Insert(entityList);

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
        /// Atualizar atendente
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route(nameof(Update))]
        [HttpPut]
        public IHttpActionResult Update(int id, [FromBody]AtendenteEmpresaRequest request)
        {
            try
            {
                //Valida objeto
                if (!ModelState.IsValid)
                    return BadRequest("Dados inválidos.");

                var entityInDb = _atendenteEmpresaBusiness.GetById(id);

                //Verifica se objeto existe
                if (entityInDb == null)
                    return NotFound();

                //Mapeio os dados do dto para o objeto recuperado do banco, atualizando os dados do objeto do banco
                Mapper.Map(request, entityInDb);

                if (_atendenteEmpresaBusiness.Update(entityInDb))
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

        /// <summary>
        /// Atualizar lista de atendentes
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [Route(nameof(UpdateList))]
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
                    var entityInDb = _atendenteEmpresaBusiness.GetById(item.Id);

                    //Verifica se objeto existe
                    if (entityInDb == null)
                        return BadRequest("Nenhum registro atualizado. Verifique os dados enviados.");
                    else
                    {
                        Mapper.Map(item, entityInDb);
                        entityList.Add(entityInDb);
                    }
                }

                if (_atendenteEmpresaBusiness.Update(entityList))
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
        /// Deletar atendente
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route(nameof(Delete))]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                var entityInDb = _atendenteEmpresaBusiness.GetById(id);

                //Verifica se objeto existe
                if (entityInDb == null)
                    return NotFound();

                if (_atendenteEmpresaBusiness.Delete(id))
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
        /// Deletar lista de atendentes
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [Route(nameof(DeleteList))]
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
                        var entityInDb = _atendenteEmpresaBusiness.GetById(id);

                        //Verifica se objeto existe
                        if (entityInDb == null)
                            return BadRequest("Nenhum registro deletado. Verifique os dados enviados.");
                        else
                            entityList.Add(entityInDb);
                    }

                    if (_atendenteEmpresaBusiness.Delete(entityList))
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
