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
    /// Empresa Controller
    /// </summary>
    [RoutePrefix("api/Empresa")]
    [Authorize]
    public class EmpresaController : ApiController
    {
        private readonly IEmpresaBusiness _empresaBusiness;
        private IHttpActionResult _result;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="empresaBusiness"></param>
        public EmpresaController(IEmpresaBusiness empresaBusiness)
        {
            _empresaBusiness = empresaBusiness;
        }

        /// <summary>
        /// Recupera todas as empresas
        /// </summary>
        /// <returns></returns>
        [Route(nameof(GetAll))]
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            try
            {
                //Mapeia os dados da fonte (source class) para o destino (destiny class)
                var lista = _empresaBusiness.GetAll().ToList().Select(Mapper.Map<Empresa, EmpresaResponse>);

                var totalRegistros = lista.Count();

                //Monta response
                _result = Ok(Retorno<IEnumerable<EmpresaResponse>>.Criar(true, "Consulta Realizada Com Sucesso", lista, totalRegistros, totalRegistros));

                //Retorna o response
                return _result;
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Recupera uma empresa
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route(nameof(GetById))]
        [HttpGet]
        public IHttpActionResult GetById(int id)
        {
            try
            {
                var entity = _empresaBusiness.GetById(id);

                if (entity == null)
                    return NotFound();

                //Monta response
                _result = Ok(Retorno<EmpresaResponse>.Criar(true, "Consulta Realizada Com Sucesso", Mapper.Map<Empresa, EmpresaResponse>(entity)));

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
        public IHttpActionResult GetAllPaged(FilterEmpresaRequest advancedFilter)
        {
            try
            {
                var result = _empresaBusiness.GetAllPaged(advancedFilter);

                var lista = result.Empresas.ToList().Select(Mapper.Map<Empresa, EmpresaResponse>);
                var totalGeral = result.TotalGeral;
                var totalLinhas = lista.Count();

                //Monta response
                _result = Ok(Retorno<IEnumerable<EmpresaResponse>>.Criar(true, "Consulta Realizada Com Sucesso", lista, totalGeral));

                //Retorna o response
                return _result;
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Recupera total de atendentes associados a empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [Route(nameof(GetTotalAtendentesEmpresa))]
        [HttpGet]
        //[AllowAnonymous]
        public IHttpActionResult GetTotalAtendentesEmpresa(int idEmpresa)
        {
            try
            {
                //Recupera o total de registros de tickets associados a um usuario
                var total = _empresaBusiness.GetTotalAtendentesEmpresa(idEmpresa);

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
        /// Insere empresa
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route(nameof(Insert))]
        [HttpPost]
        public IHttpActionResult Insert([FromBody]EmpresaRequest request)
        {
            try
            {
                //Valida objeto
                if (!ModelState.IsValid)
                    return BadRequest("Dados inválidos.");

                var entity = Mapper.Map<EmpresaRequest, Empresa>(request);

                _empresaBusiness.Insert(ref entity);

                if (entity.Id > 0)
                {
                    //Monta response
                    _result = Ok(Retorno<EmpresaResponse>.Criar(true, "Inclusão Realizada Com Sucesso", Mapper.Map<Empresa, EmpresaResponse>(entity)));
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
        /// Insere lista de empresas
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [Route(nameof(InsertList))]
        [HttpPost]
        public IHttpActionResult InsertList(IEnumerable<EmpresaRequest> list)
        {
            IList<Empresa> entityList = new List<Empresa>();

            try
            {
                //Valida objeto
                if (!ModelState.IsValid)
                    return BadRequest("Dados inválidos.");

                foreach (var item in list)
                {
                    var entity = Mapper.Map<EmpresaRequest, Empresa>(item);

                    entityList.Add(entity);
                }

                var rows = _empresaBusiness.Insert(entityList);

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
        /// Atualiza empresa
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route(nameof(Update))]
        [HttpPut]
        public IHttpActionResult Update(int id, [FromBody]EmpresaRequest request)
        {
            try
            {
                //Valida objeto
                if (!ModelState.IsValid)
                    return BadRequest("Dados inválidos.");

                var entityInDb = _empresaBusiness.GetById(id);

                //Verifica se objeto existe
                if (entityInDb == null)
                    return NotFound();

                //Mapeio os dados do dto para o objeto recuperado do banco, atualizando os dados do objeto do banco
                Mapper.Map(request, entityInDb);

                if (_empresaBusiness.Update(entityInDb))
                {
                    //Monta response
                    _result = Ok(Retorno<Empresa>.Criar(true, "Atualização Realizada Com Sucesso", entityInDb));

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
        /// Atualiza lista de empresas
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [Route(nameof(UpdateList))]
        [HttpPut]
        public IHttpActionResult UpdateList(IEnumerable<EmpresaUpdate> list)
        {
            IList<Empresa> entityList = new List<Empresa>();

            try
            {
                //Valida objeto
                if (!ModelState.IsValid)
                    return BadRequest("Dados inválidos.");

                foreach (var item in list)
                {
                    var entityInDb = _empresaBusiness.GetById(item.Id);

                    //Verifica se objeto existe
                    if (entityInDb == null)
                        return BadRequest("Nenhum registro atualizado. Verifique os dados enviados.");
                    else
                    {
                        Mapper.Map(item, entityInDb);
                        entityList.Add(entityInDb);
                    }
                }

                if (_empresaBusiness.Update(entityList))
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
        /// Deleta empresa
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route(nameof(Delete))]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                var entityInDb = _empresaBusiness.GetById(id);

                //Verifica se objeto existe
                if (entityInDb == null)
                    return NotFound();

                if (_empresaBusiness.Delete(id))
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
        /// Deleta lista de empresas
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [Route(nameof(DeleteList))]
        [HttpDelete]
        public IHttpActionResult DeleteList([FromBody]int[] list)
        {
            try
            {
                IList<Empresa> entityList = new List<Empresa>();

                try
                {
                    foreach (var id in list)
                    {
                        var entityInDb = _empresaBusiness.GetById(id);

                        //Verifica se objeto existe
                        if (entityInDb == null)
                            return BadRequest("Nenhum registro deletado. Verifique os dados enviados.");
                        else
                            entityList.Add(entityInDb);
                    }

                    if (_empresaBusiness.Delete(entityList))
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
