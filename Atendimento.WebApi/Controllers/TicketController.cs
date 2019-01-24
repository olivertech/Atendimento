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
    [RoutePrefix("api/Ticket")]
    [Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TicketController : ApiController
    {
        private readonly ITicketBusiness _business;
        private readonly IUsuarioClienteBusiness _usuarioClienteBusiness;
        private readonly IAtendenteEmpresaBusiness _atendenteEmpresaBusiness;
        private readonly IStatusTicketBusiness _statusTicketBusiness;
        private readonly IClassificacaoBusiness _classificacaoBusiness;
        private readonly IAnexoBusiness _anexoBusiness;
        private IHttpActionResult _result;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="business"></param>
        /// <param name="usuarioClienteBusiness"></param>
        /// <param name="atendenteEmpresaBusiness"></param>
        /// <param name="statusTicketBusiness"></param>
        /// <param name="classificacaoBusiness"></param>
        /// <param name="anexoBusiness"></param>
        public TicketController(ITicketBusiness business,
                                IUsuarioClienteBusiness usuarioClienteBusiness,
                                IAtendenteEmpresaBusiness atendenteEmpresaBusiness,
                                IStatusTicketBusiness statusTicketBusiness,
                                IClassificacaoBusiness classificacaoBusiness,
                                IAnexoBusiness anexoBusiness)
        {
            _business = business;
            _usuarioClienteBusiness = usuarioClienteBusiness;
            _atendenteEmpresaBusiness = atendenteEmpresaBusiness;
            _statusTicketBusiness = statusTicketBusiness;
            _classificacaoBusiness = classificacaoBusiness;
            _anexoBusiness = anexoBusiness;
        }

        /// <summary>
        /// Recupera todos os tickets
        /// </summary>
        /// <returns></returns>
        [Route(nameof(GetAll))]
        [HttpGet]
        //[AllowAnonymous]
        public IHttpActionResult GetAll()
        {
            try
            {
                //Mapeia os dados da fonte (source class) para o destino (destiny class)
                var lista = _business.GetAll().ToList().Select(Mapper.Map<Ticket, TicketResponse>);

                var totalRegistros = lista.Count();

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

        /// <summary>
        /// Recupera um ticket
        /// </summary>
        /// <param name="ticketRequest"></param>
        /// <returns></returns>
        [Route(nameof(GetById))]
        [HttpPost]
        //[AllowAnonymous]
        public IHttpActionResult GetById(TicketFilledRequest ticketRequest)
        {
            try
            {
                var entity = _business.GetByIdFilled(ticketRequest.Id, ticketRequest.WithAnexos);

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

        /// <summary>
        /// Recupera total de tickets
        /// </summary>
        /// <param name="idStatusTicket"></param>
        /// <returns></returns>
        [Route(nameof(GetCount))]
        [HttpGet]
        //[AllowAnonymous]
        public IHttpActionResult GetCount(int idStatusTicket)
        {
            try
            {
                //Recupera o total de registros de tickets de acordo com o filtro informado
                var total = _business.GetCount(idStatusTicket);

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
        /// Recupera total de tickets
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        [Route(nameof(GetTotalTicketsUsuario))]
        [HttpGet]
        //[AllowAnonymous]
        public IHttpActionResult GetTotalTicketsUsuario(int idUsuario)
        {
            try
            {
                //Recupera o total de registros de tickets associados a um usuario
                var total = _business.GetTotalTicketsUsuario(idUsuario);

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
        /// Recupera todos os contadores associados aos diferentes tipos de ticket
        /// </summary>
        /// <param name="idCliente"></param>
        /// <returns></returns>
        [Route(nameof(GetCounts))]
        [HttpGet]
        //[Authorize]
        public IHttpActionResult GetCounts(int idCliente)
        {
            try
            {
                //Recupera todos os totais de registros de tickets
                var response = _business.GetCounts(idCliente);

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

        /// <summary>
        /// Recupera tickets de forma paginada
        /// </summary>
        /// <param name="advancedFilter"></param>
        /// <returns></returns>
        [Route(nameof(GetAllPaged))]
        [HttpPost]
        //[AllowAnonymous]
        public IHttpActionResult GetAllPaged(FilterRequest advancedFilter)
        {
            try
            {
                var result = _business.GetAllPaged(advancedFilter);

                var lista = result.Tickets.ToList().Select(Mapper.Map<Ticket, TicketResponse>);
                var totalGeral = result.TotalGeral;
                var totalFiltrado = result.TotalFiltrado;
                var totalLinhas = lista.Count();

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

        /// <summary>
        /// Insere ticket
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route(nameof(Insert))]
        [HttpPost]
        //[AllowAnonymous]
        public IHttpActionResult Insert([FromBody]TicketRequest request)
        {
            AtendenteEmpresa atendenteEmpresa = null;
            List<AtendenteEmpresa> listaAtendentes = null;

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

                    //Trata dos anexos
                    if (Directory.Exists(pathAnexosUsuario))
                    {
                        //Zipa todos os anexos
                        var zipName = Arquivo.Compress(ConfigurationManager.AppSettings["CaminhoFisicoAnexo"], pathAnexosUsuario, entity.Id);

                        //======================================
                        //Guarda anexo (zip) no banco de dados
                        //======================================
                        var anexo = new Anexo
                        {
                            IdTicket = entity.Id,
                            Nome = zipName
                        };

                        _anexoBusiness.Insert(ref anexo);
                    }

                    //===========================================================================================
                    //Enviar email de confirmação de criação do novo atendimento
                    //===========================================================================================
                    var usuarioCliente = _usuarioClienteBusiness.GetById(request.IdUsuarioCliente);

                    if (request.UserTypeAgent == "S")
                    {
                        atendenteEmpresa = _atendenteEmpresaBusiness.GetById(request.IdAtendente);

                        if (atendenteEmpresa != null)
                        {
                            if (atendenteEmpresa.Copia)
                            {
                                listaAtendentes = _atendenteEmpresaBusiness.GetList(x => x.IdEmpresa == atendenteEmpresa.IdEmpresa && x.Id != atendenteEmpresa.Id).ToList();
                            }
                        }
                    }
                    else
                    {
                        listaAtendentes = _atendenteEmpresaBusiness.GetAll(atendenteEmpresa.IdEmpresa).ToList();
                    }

                    var statusTicket = _statusTicketBusiness.GetById(request.IdStatusTicket);

                    _business.EnviarEmailConfirmacao(request.UserTypeAgent, statusTicket, null, entity, usuarioCliente, atendenteEmpresa, listaAtendentes, "insert");
                    //===========================================================================================
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
        /// Insere lista de tickets
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [Route(nameof(InsertList))]
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

                var rows = _business.Insert(entityList);

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
        /// Atualiza ticket
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route(nameof(Update))]
        [HttpPut]
        //[AllowAnonymous]
        public IHttpActionResult Update(int id, [FromBody]TicketRequest request)
        {
            try
            {
                //Valida objeto
                if (!ModelState.IsValid)
                    return BadRequest("Dados inválidos.");

                var entityInDb = _business.GetById(id);

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

        /// <summary>
        /// Atualiza o status do ticket
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route(nameof(UpdateStatusTicket))]
        [HttpPut]
        //[AllowAnonymous]
        public IHttpActionResult UpdateStatusTicket([FromBody]TicketUpdateStatusRequest request)
        {
            AtendenteEmpresa atendenteEmpresa = null;
            List<AtendenteEmpresa> listaAtendentes = null;

            try
            {
                //Valida objeto
                if (!ModelState.IsValid)
                    return BadRequest("Dados inválidos.");

                var entityInDb = _business.GetById(request.Id);

                //Verifica se objeto existe
                if (entityInDb == null)
                    return NotFound();

                entityInDb.IdStatusTicket = request.IdStatusTicket;

                if (request.IdStatusTicket == 1 || request.IdStatusTicket == 4 || request.IdStatusTicket == 5)
                {
                    entityInDb.DataHoraFinal = null;
                    entityInDb.DataHoraAlteracao = DateTime.Now;
                }

                if (_business.UpdateStatusTicket(entityInDb))
                {
                    //Recupera o ticket atualizado
                    entityInDb = _business.GetById(request.Id);

                    //Monta response
                    _result = Ok(Retorno<Ticket>.Criar(true, "Atualização Realizada Com Sucesso", entityInDb));

                    //===========================================================================================
                    //Enviar email de atualização do atendimento
                    //===========================================================================================
                    var usuarioCliente = _usuarioClienteBusiness.GetById(request.IdUsuarioCliente);
                    var statusTicket = _statusTicketBusiness.GetById(request.IdStatusTicket);

                    if (request.UserTypeAgent == "S")
                    {
                        atendenteEmpresa = _atendenteEmpresaBusiness.GetById(request.IdAtendente);

                        if (atendenteEmpresa != null)
                        {
                            if (atendenteEmpresa.Copia)
                            {
                                listaAtendentes = _atendenteEmpresaBusiness.GetList(x => x.IdEmpresa == atendenteEmpresa.IdEmpresa && x.Id != atendenteEmpresa.Id).ToList();
                            }
                        }
                    }

                    _business.EnviarEmailConfirmacao(request.UserTypeAgent, statusTicket, null, entityInDb, usuarioCliente, atendenteEmpresa, listaAtendentes, "update");
                    //===========================================================================================

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
        /// Atualiza a classificação do atendimento
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route(nameof(UpdateClassificacao))]
        [HttpPut]
        //[AllowAnonymous]
        public IHttpActionResult UpdateClassificacao([FromBody]TicketUpdateClassificacaoRequest request)
        {
            AtendenteEmpresa atendenteEmpresa = null;
            List<AtendenteEmpresa> listaAtendentes = null;

            try
            {
                //Valida objeto
                if (!ModelState.IsValid)
                    return BadRequest("Dados inválidos.");

                var entityInDb = _business.GetById(request.Id);

                //Verifica se objeto existe
                if (entityInDb == null)
                    return NotFound();

                entityInDb.IdClassificacao = request.IdClassificacao;

                if (_business.Update(entityInDb))
                {
                    //Recupera o ticket atualizado
                    entityInDb = _business.GetById(request.Id);

                    //Monta response
                    _result = Ok(Retorno<Ticket>.Criar(true, "Atualização Realizada Com Sucesso", entityInDb));

                    //===========================================================================================
                    //Enviar email de atualização do atendimento
                    //===========================================================================================
                    var usuarioCliente = _usuarioClienteBusiness.GetById(request.IdUsuarioCliente);
                    var classificacao = _classificacaoBusiness.GetById(entityInDb.IdClassificacao);

                    if (request.UserTypeAgent == "S")
                    {
                        atendenteEmpresa = _atendenteEmpresaBusiness.GetById(request.IdAtendente);

                        if (atendenteEmpresa != null)
                        {
                            if (atendenteEmpresa.Copia)
                            {
                                listaAtendentes = _atendenteEmpresaBusiness.GetList(x => x.IdEmpresa == atendenteEmpresa.IdEmpresa && x.Id != atendenteEmpresa.Id).ToList();
                            }
                        }
                    }

                    _business.EnviarEmailConfirmacao(request.UserTypeAgent, null, classificacao, entityInDb, usuarioCliente, atendenteEmpresa, listaAtendentes, nameof(classificacao));
                    //===========================================================================================

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
        /// Atualiza lista de tickes
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [Route(nameof(UpdateList))]
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
                    var entityInDb = _business.GetById(item.Id);

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

        /// <summary>
        /// Deleta ticket
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route(nameof(Delete))]
        [HttpDelete]
        //[AllowAnonymous]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                var entityInDb = _business.GetById(id);

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

        /// <summary>
        /// Deleta lista de tickets
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [Route(nameof(DeleteList))]
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
                        var entityInDb = _business.GetById(id);

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
