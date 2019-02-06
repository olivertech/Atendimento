using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Http;
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
    public class TicketMensagemController : ApiController
    {
        private readonly ITicketMensagemBusiness _ticketMensagemBusiness;
        private readonly ITicketBusiness _ticketBusiness;
        private readonly IUsuarioClienteBusiness _usuarioClienteBusiness;
        private readonly IAtendenteEmpresaBusiness _atendenteEmpresaBusiness;
        private readonly IAnexoBusiness _anexoBusiness;
        private IHttpActionResult _result;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="ticketMensagemBusiness"></param>
        /// <param name="usuarioClienteBusiness"></param>
        /// <param name="atendenteEmpresaBusiness"></param>
        /// <param name="ticketBusiness"></param>
        /// <param name="anexoBusiness"></param>
        public TicketMensagemController(ITicketMensagemBusiness ticketMensagemBusiness,
                                        IUsuarioClienteBusiness usuarioClienteBusiness,
                                        IAtendenteEmpresaBusiness atendenteEmpresaBusiness,
                                        ITicketBusiness ticketBusiness,
                                        IAnexoBusiness anexoBusiness)
        {
            _ticketMensagemBusiness = ticketMensagemBusiness;
            _ticketBusiness = ticketBusiness;
            _usuarioClienteBusiness = usuarioClienteBusiness;
            _atendenteEmpresaBusiness = atendenteEmpresaBusiness;
            _anexoBusiness = anexoBusiness;
        }

        /// <summary>
        /// Recupera todas as mensagens
        /// </summary>
        /// <returns></returns>
        [Route(nameof(GetAll))]
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            try
            {
                //Mapeia os dados da fonte (source class) para o destino (destiny class)
                var lista = _ticketMensagemBusiness.GetAll().ToList().Select(Mapper.Map<TicketMensagem, TicketMensagemResponse>);

                var totalRegistros = lista.Count();

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

        /// <summary>
        /// Recupera todas as mensagens associada a um ticket
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route(nameof(GetAllByTicketId))]
        [HttpPost]
        public IHttpActionResult GetAllByTicketId(TicketMensagensRequest request)
        {
            try
            {
                //Mapeia os dados da fonte (source class) para o destino (destiny class)
                IEnumerable<TicketMensagemResponse> lista = _ticketMensagemBusiness.GetAllByTicketId(request).ToList();

                var totalRegistros = lista.Count();

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

        /// <summary>
        /// Recupera uma mensagem
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route(nameof(GetById))]
        [HttpGet]
        public IHttpActionResult GetById(int id)
        {
            try
            {
                var entity = _ticketMensagemBusiness.GetById(id);

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

        /// <summary>
        /// Insere mensagem
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route(nameof(Insert))]
        [HttpPost]
        public IHttpActionResult Insert([FromBody]TicketMensagemRequest request)
        {
            UsuarioCliente usuarioCliente = null;
            AtendenteEmpresa atendenteEmpresa = null;
            Ticket ticket = null;
            List<AtendenteEmpresa> listaAtendentes = null;

            try
            {
                //Valida objeto
                if (!ModelState.IsValid)
                    return BadRequest("Dados inválidos.");

                var entity = Mapper.Map<TicketMensagemRequest, TicketMensagem>(request);
                var pathAnexosUsuario = request.PathAnexos;

                ticket = _ticketBusiness.GetById(request.IdTicket);

                ticket.DataHoraAlteracao = DateTime.Now;
                ticket.DataHoraUltimaMensagem = DateTime.Now;

                //Se for uma mensagem interna enviada pelo suporte
                if (request.UserType == "S" && request.Interno)
                {
                    entity.IdAtendenteEmpresa = request.IdAutor;
                    ticket.IdStatusTicket = request.IdStatusTicket > 0 ? request.IdStatusTicket : 5; //Em Análise
                }
                else
                {
                    //Se for uma mensagem enviada pelo suporte
                    if (request.UserType == "S" && !request.Interno)
                    {
                        entity.IdAtendenteEmpresa = request.IdAutor;
                        ticket.IdStatusTicket = request.IdStatusTicket > 0 ? request.IdStatusTicket : 4; //Pendente com Cliente
                    }
                    else
                    {
                        //Se for uma mensagem enviada pelo usuário cliente
                        if (request.UserType == "C")
                        {
                            entity.IdUsuarioCliente = request.IdAutor;
                            ticket.IdStatusTicket = 1; //Aguardando Atendimento
                        }
                    }
                }

                //Insere a nova mensagem
                _ticketMensagemBusiness.Insert(ref entity);

                if (entity.Id > 0)
                {
                    //Atualiza o status do ticket, para refletir o novo momento do atendimento
                    _ticketBusiness.UpdateStatusTicket(ticket);

                    //Monta response
                    _result = Ok(Retorno<TicketMensagemResponse>.Criar(true, "Inclusão Realizada Com Sucesso", Mapper.Map<TicketMensagem, TicketMensagemResponse>(entity)));

                    if (Directory.Exists(pathAnexosUsuario))
                    {
                        //Zipa todos os anexos
                        var zipName = Arquivo.Compress(ConfigurationManager.AppSettings["CaminhoFisicoAnexo"], pathAnexosUsuario, entity.Id);

                        //======================================
                        //Guarda anexo (zip) no banco de dados
                        //======================================
                        var anexo = new Anexo
                        {
                            IdTicketMensagem = entity.Id,
                            Nome = zipName
                        };

                        _anexoBusiness.Insert(ref anexo);
                    }

                    //===========================================================================================
                    //Enviar email de confirmação de nova mensagem
                    //===========================================================================================

                    var ticketResponse = _ticketBusiness.GetByIdFilled(request.IdTicket, false);

                    if (request.UserType == "S")
                    {
                        usuarioCliente = _usuarioClienteBusiness.GetById(ticketResponse.UsuarioCliente.Id);
                        atendenteEmpresa = _atendenteEmpresaBusiness.GetById(request.IdAutor);

                        if (atendenteEmpresa.Copia)
                        {
                            listaAtendentes = _atendenteEmpresaBusiness.GetList(x => x.IdEmpresa == atendenteEmpresa.IdEmpresa && x.Id != atendenteEmpresa.Id).ToList();
                        }
                    }
                    else
                    {
                        usuarioCliente = _usuarioClienteBusiness.GetById(request.IdAutor);
                        listaAtendentes = _atendenteEmpresaBusiness.GetAll(ticketResponse.UsuarioCliente.Cliente.IdEmpresa).ToList();
                    }

                    try
                    {
                        _ticketMensagemBusiness.EnviarEmailConfirmacao(request, entity, ticketResponse, atendenteEmpresa, usuarioCliente, listaAtendentes);
                    }
                    catch (Exception)
                    {
                        //Monta response
                        _result = Ok(Retorno<TicketMensagemResponse>.Criar(true, "Inclusão Realizada Com Sucesso - Email de confirmação não enviado", Mapper.Map<TicketMensagem, TicketMensagemResponse>(entity)));
                    }
                   
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
        /// Insere lista de mensagens
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [Route(nameof(InsertList))]
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

                var rows = _ticketMensagemBusiness.Insert(entityList);

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
        /// Atualiza mensagem
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route(nameof(Update))]
        [HttpPut]
        public IHttpActionResult Update(int id, [FromBody]TicketMensagemRequest request)
        {
            try
            {
                //Valida objeto
                if (!ModelState.IsValid)
                    return BadRequest("Dados inválidos.");

                var entityInDb = _ticketMensagemBusiness.GetById(id);

                //Verifica se objeto existe
                if (entityInDb == null)
                    return NotFound();

                //Mapeio os dados do dto para o objeto recuperado do banco, atualizando os dados do objeto do banco
                Mapper.Map(request, entityInDb);

                if (_ticketMensagemBusiness.Update(entityInDb))
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

        /// <summary>
        /// Atualiza lista de mensagens
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [Route(nameof(UpdateList))]
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
                    var entityInDb = _ticketMensagemBusiness.GetById(item.Id);

                    //Verifica se objeto existe
                    if (entityInDb == null)
                        return BadRequest("Nenhum registro atualizado. Verifique os dados enviados.");
                    else
                    {
                        Mapper.Map(item, entityInDb);
                        entityList.Add(entityInDb);
                    }
                }

                if (_ticketMensagemBusiness.Update(entityList))
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
        /// Deleta mensagem
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route(nameof(Delete))]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                var entityInDb = _ticketMensagemBusiness.GetById(id);

                //Verifica se objeto existe
                if (entityInDb == null)
                    return NotFound();

                if (_ticketMensagemBusiness.Delete(id))
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
        /// Deleta lista de mensagens
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [Route(nameof(DeleteList))]
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
                        var entityInDb = _ticketMensagemBusiness.GetById(id);

                        //Verifica se objeto existe
                        if (entityInDb == null)
                            return BadRequest("Nenhum registro deletado. Verifique os dados enviados.");
                        else
                            entityList.Add(entityInDb);
                    }

                    if (_ticketMensagemBusiness.Delete(entityList))
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
