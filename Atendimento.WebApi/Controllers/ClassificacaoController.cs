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
    /// Classificacao Controller
    /// </summary>
    [RoutePrefix("api/Classificacao")]
    [Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ClassificacaoController : ApiController
    {
        private readonly IClassificacaoBusiness _business;
        private IHttpActionResult _result;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="business"></param>
        public ClassificacaoController(IClassificacaoBusiness business)
        {
            _business = business;
        }

        /// GET: api/Classificacao
        [Route("GetAll")]
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            try
            {
                //Mapeia os dados da fonte (source class) para o destino (destiny class)
                IEnumerable<ClassificacaoResponse> lista = _business.GetAll().ToList().Select(Mapper.Map<Classificacao, ClassificacaoResponse>);

                int totalRegistros = lista.Count();

                //Monta response
                _result = Ok(Retorno<IEnumerable<ClassificacaoResponse>>.Criar(true, "Consulta Realizada Com Sucesso", lista, totalRegistros, totalRegistros));

                //Retorna o response
                return _result;
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        /// GET: api/Classificacao/5
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
                _result = Ok(Retorno<ClassificacaoResponse>.Criar(true, "Consulta Realizada Com Sucesso", Mapper.Map<Classificacao, ClassificacaoResponse>(entity)));

                //Retorna o response
                return _result;
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        /// POST: api/Classificacao
        [Route("Insert")]
        [HttpPost]
        public IHttpActionResult Insert([FromBody]ClassificacaoRequest request)
        {
            try
            {
                //Valida objeto
                if (!ModelState.IsValid)
                    return BadRequest("Dados inválidos.");

                var entity = Mapper.Map<ClassificacaoRequest, Classificacao>(request);

                _business.Insert(ref entity);

                if (entity.Id > 0)
                {
                    //Monta response
                    _result = Ok(Retorno<ClassificacaoResponse>.Criar(true, "Inclusão Realizada Com Sucesso", Mapper.Map<Classificacao, ClassificacaoResponse>(entity)));
                }

                //Retorna o response
                return _result;
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        /// POST: api/Classificacao
        [Route("InsertList")]
        [HttpPost]
        public IHttpActionResult InsertList(IEnumerable<ClassificacaoRequest> list)
        {
            IList<Classificacao> entityList = new List<Classificacao>();

            try
            {
                //Valida objeto
                if (!ModelState.IsValid)
                    return BadRequest("Dados inválidos.");

                foreach (var item in list)
                {
                    var entity = Mapper.Map<ClassificacaoRequest, Classificacao>(item);

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

        /// PUT: api/Classificacao/5
        [Route("Update")]
        [HttpPut]
        public IHttpActionResult Update(int id, [FromBody]ClassificacaoRequest request)
        {
            try
            {
                //Valida objeto
                if (!ModelState.IsValid)
                    return BadRequest("Dados inválidos.");

                Classificacao entityInDb = _business.GetById(id);

                //Verifica se objeto existe
                if (entityInDb == null)
                    return NotFound();

                //Mapeio os dados do dto para o objeto recuperado do banco, atualizando os dados do objeto do banco
                Mapper.Map(request, entityInDb);

                if (_business.Update(entityInDb))
                {
                    //Monta response
                    _result = Ok(Retorno<Classificacao>.Criar(true, "Atualização Realizada Com Sucesso", entityInDb));

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

        /// PUT: api/Classificacao/5
        [Route("UpdateList")]
        [HttpPut]
        public IHttpActionResult UpdateList(IEnumerable<ClassificacaoUpdate> list)
        {
            IList<Classificacao> entityList = new List<Classificacao>();

            try
            {
                //Valida objeto
                if (!ModelState.IsValid)
                    return BadRequest("Dados inválidos.");

                foreach (var item in list)
                {
                    Classificacao entityInDb = _business.GetById(item.Id);

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

        /// DELETE: api/Classificacao/5
        [Route("Delete")]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                Classificacao entityInDb = _business.GetById(id);

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

        /// DELETE: api/Classificacao/5
        [Route("DeleteList")]
        [HttpDelete]
        public IHttpActionResult DeleteList([FromBody]int[] list)
        {
            try
            {
                IList<Classificacao> entityList = new List<Classificacao>();

                try
                {
                    foreach (var id in list)
                    {
                        Classificacao entityInDb = _business.GetById(id);

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