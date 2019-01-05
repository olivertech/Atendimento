using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using Atendimento.Entities.Response;

namespace Atendimento.WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("api/ListaTelefonica")]
    [Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ListaTelefonicaController : ApiController
    {
        private IHttpActionResult _result;
        private static IList<Contato> _contatos = new List<Contato>()
            {
                new Contato { Id = 1, Serial = "qweqweqweqweqqweq", Nome = "Marcelo", Telefone = "999999991", Cor = "blue", Data = DateTime.Today, Operadora = new Operadora { Id = 1, Nome = "Oi", Codigo = "14", Categoria = "Celular", Preco = 2 } },
                new Contato { Id = 2, Serial = "qweqweqweqweqqweq", Nome = "Yasmin", Telefone = "999999992", Cor = "red", Data = DateTime.Today, Operadora = new Operadora { Id = 2, Nome = "Vivo", Codigo = "41", Categoria = "Celular", Preco = 1 } },
                new Contato { Id = 3, Serial = "qweqweqweqweqqweq", Nome = "Simone", Telefone = "999999993", Cor = "green", Data = DateTime.Today, Operadora = new Operadora { Id = 3, Nome = "Tim", Codigo = "41", Categoria = "Celular", Preco = 3 } },
                new Contato { Id = 4, Serial = "qweqweqweqweqqweq", Nome = "Marcelo", Telefone = "999999991", Cor = "blue", Data = DateTime.Today, Operadora = new Operadora { Id = 1, Nome = "Oi", Codigo = "14", Categoria = "Celular", Preco = 2 } },
                new Contato { Id = 5, Serial = "qweqweqweqweqqweq", Nome = "Yasmin", Telefone = "999999992", Cor = "red", Data = DateTime.Today, Operadora = new Operadora { Id = 2, Nome = "Vivo", Codigo = "41", Categoria = "Celular", Preco = 1 } },
                new Contato { Id = 6, Serial = "qweqweqweqweqqweq", Nome = "Simone", Telefone = "999999993", Cor = "green", Data = DateTime.Today, Operadora = new Operadora { Id = 3, Nome = "Tim", Codigo = "41", Categoria = "Celular", Preco = 3 } },
                new Contato { Id = 7, Serial = "qweqweqweqweqqweq", Nome = "Marcelo", Telefone = "999999991", Cor = "blue", Data = DateTime.Today, Operadora = new Operadora { Id = 1, Nome = "Oi", Codigo = "14", Categoria = "Celular", Preco = 2 } },
                new Contato { Id = 8, Serial = "qweqweqweqweqqweq", Nome = "Yasmin", Telefone = "999999992", Cor = "red", Data = DateTime.Today, Operadora = new Operadora { Id = 2, Nome = "Vivo", Codigo = "41", Categoria = "Celular", Preco = 1 } },
                new Contato { Id = 9, Serial = "qweqweqweqweqqweq", Nome = "Simone", Telefone = "999999993", Cor = "green", Data = DateTime.Today, Operadora = new Operadora { Id = 3, Nome = "Tim", Codigo = "41", Categoria = "Celular", Preco = 3 } },
                new Contato { Id = 10, Serial = "qweqweqweqweqqweq", Nome = "Marcelo", Telefone = "999999991", Cor = "blue", Data = DateTime.Today, Operadora = new Operadora { Id = 1, Nome = "Oi", Codigo = "14", Categoria = "Celular", Preco = 2 } },
                //new Contato { Id = 11, Serial = "qweqweqweqweqqweq", Nome = "Yasmin", Telefone = "999999992", Cor = "red", Data = DateTime.Today, Operadora = new Operadora { Id = 2, Nome = "Vivo", Codigo = "41", Categoria = "Celular", Preco = 1 } },
                //new Contato { Id = 12, Serial = "qweqweqweqweqqweq", Nome = "Simone", Telefone = "999999993", Cor = "green", Data = DateTime.Today, Operadora = new Operadora { Id = 3, Nome = "Tim", Codigo = "41", Categoria = "Celular", Preco = 3 } },
                //new Contato { Id = 13, Serial = "qweqweqweqweqqweq", Nome = "Marcelo", Telefone = "999999991", Cor = "blue", Data = DateTime.Today, Operadora = new Operadora { Id = 1, Nome = "Oi", Codigo = "14", Categoria = "Celular", Preco = 2 } },
                //new Contato { Id = 14, Serial = "qweqweqweqweqqweq", Nome = "Yasmin", Telefone = "999999992", Cor = "red", Data = DateTime.Today, Operadora = new Operadora { Id = 2, Nome = "Vivo", Codigo = "41", Categoria = "Celular", Preco = 1 } },
                //new Contato { Id = 15, Serial = "qweqweqweqweqqweq", Nome = "Simone", Telefone = "999999993", Cor = "green", Data = DateTime.Today, Operadora = new Operadora { Id = 3, Nome = "Tim", Codigo = "41", Categoria = "Celular", Preco = 3 } },
                //new Contato { Id = 16, Serial = "qweqweqweqweqqweq", Nome = "Marcelo", Telefone = "999999991", Cor = "blue", Data = DateTime.Today, Operadora = new Operadora { Id = 1, Nome = "Oi", Codigo = "14", Categoria = "Celular", Preco = 2 } },
                //new Contato { Id = 17, Serial = "qweqweqweqweqqweq", Nome = "Yasmin", Telefone = "999999992", Cor = "red", Data = DateTime.Today, Operadora = new Operadora { Id = 2, Nome = "Vivo", Codigo = "41", Categoria = "Celular", Preco = 1 } },
                //new Contato { Id = 18, Serial = "qweqweqweqweqqweq", Nome = "Simone", Telefone = "999999993", Cor = "green", Data = DateTime.Today, Operadora = new Operadora { Id = 3, Nome = "Tim", Codigo = "41", Categoria = "Celular", Preco = 3 } },
                //new Contato { Id = 19, Serial = "qweqweqweqweqqweq", Nome = "Marcelo", Telefone = "999999991", Cor = "blue", Data = DateTime.Today, Operadora = new Operadora { Id = 1, Nome = "Oi", Codigo = "14", Categoria = "Celular", Preco = 2 } },
                //new Contato { Id = 20, Serial = "qweqweqweqweqqweq", Nome = "Yasmin", Telefone = "999999992", Cor = "red", Data = DateTime.Today, Operadora = new Operadora { Id = 2, Nome = "Vivo", Codigo = "41", Categoria = "Celular", Preco = 1 } },
                //new Contato { Id = 21, Serial = "qweqweqweqweqqweq", Nome = "Simone", Telefone = "999999993", Cor = "green", Data = DateTime.Today, Operadora = new Operadora { Id = 3, Nome = "Tim", Codigo = "41", Categoria = "Celular", Preco = 3 } },
                //new Contato { Id = 22, Serial = "qweqweqweqweqqweq", Nome = "Marcelo", Telefone = "999999991", Cor = "blue", Data = DateTime.Today, Operadora = new Operadora { Id = 1, Nome = "Oi", Codigo = "14", Categoria = "Celular", Preco = 2 } },
                //new Contato { Id = 23, Serial = "qweqweqweqweqqweq", Nome = "Yasmin", Telefone = "999999992", Cor = "red", Data = DateTime.Today, Operadora = new Operadora { Id = 2, Nome = "Vivo", Codigo = "41", Categoria = "Celular", Preco = 1 } },
                //new Contato { Id = 24, Serial = "qweqweqweqweqqweq", Nome = "Simone", Telefone = "999999993", Cor = "green", Data = DateTime.Today, Operadora = new Operadora { Id = 3, Nome = "Tim", Codigo = "41", Categoria = "Celular", Preco = 3 } },
                //new Contato { Id = 25, Serial = "qweqweqweqweqqweq", Nome = "Marcelo", Telefone = "999999991", Cor = "blue", Data = DateTime.Today, Operadora = new Operadora { Id = 1, Nome = "Oi", Codigo = "14", Categoria = "Celular", Preco = 2 } },
                //new Contato { Id = 26, Serial = "qweqweqweqweqqweq", Nome = "Yasmin", Telefone = "999999992", Cor = "red", Data = DateTime.Today, Operadora = new Operadora { Id = 2, Nome = "Vivo", Codigo = "41", Categoria = "Celular", Preco = 1 } },
                //new Contato { Id = 27, Serial = "qweqweqweqweqqweq", Nome = "Simone", Telefone = "999999993", Cor = "green", Data = DateTime.Today, Operadora = new Operadora { Id = 3, Nome = "Tim", Codigo = "41", Categoria = "Celular", Preco = 3 } },
                //new Contato { Id = 28, Serial = "qweqweqweqweqqweq", Nome = "Marcelo", Telefone = "999999991", Cor = "blue", Data = DateTime.Today, Operadora = new Operadora { Id = 1, Nome = "Oi", Codigo = "14", Categoria = "Celular", Preco = 2 } },
                //new Contato { Id = 29, Serial = "qweqweqweqweqqweq", Nome = "Yasmin", Telefone = "999999992", Cor = "red", Data = DateTime.Today, Operadora = new Operadora { Id = 2, Nome = "Vivo", Codigo = "41", Categoria = "Celular", Preco = 1 } },
                //new Contato { Id = 30, Serial = "qweqweqweqweqqweq", Nome = "Simone", Telefone = "999999993", Cor = "green", Data = DateTime.Today, Operadora = new Operadora { Id = 3, Nome = "Tim", Codigo = "41", Categoria = "Celular", Preco = 3 } },
                //new Contato { Id = 31, Serial = "qweqweqweqweqqweq", Nome = "Marcelo", Telefone = "999999991", Cor = "blue", Data = DateTime.Today, Operadora = new Operadora { Id = 1, Nome = "Oi", Codigo = "14", Categoria = "Celular", Preco = 2 } },
                //new Contato { Id = 32, Serial = "qweqweqweqweqqweq", Nome = "Yasmin", Telefone = "999999992", Cor = "red", Data = DateTime.Today, Operadora = new Operadora { Id = 2, Nome = "Vivo", Codigo = "41", Categoria = "Celular", Preco = 1 } },
                //new Contato { Id = 33, Serial = "qweqweqweqweqqweq", Nome = "Simone", Telefone = "999999993", Cor = "green", Data = DateTime.Today, Operadora = new Operadora { Id = 3, Nome = "Tim", Codigo = "41", Categoria = "Celular", Preco = 3 } },
                //new Contato { Id = 34, Serial = "qweqweqweqweqqweq", Nome = "Marcelo", Telefone = "999999991", Cor = "blue", Data = DateTime.Today, Operadora = new Operadora { Id = 1, Nome = "Oi", Codigo = "14", Categoria = "Celular", Preco = 2 } },
                //new Contato { Id = 35, Serial = "qweqweqweqweqqweq", Nome = "Yasmin", Telefone = "999999992", Cor = "red", Data = DateTime.Today, Operadora = new Operadora { Id = 2, Nome = "Vivo", Codigo = "41", Categoria = "Celular", Preco = 1 } },
                //new Contato { Id = 36, Serial = "qweqweqweqweqqweq", Nome = "Simone", Telefone = "999999993", Cor = "green", Data = DateTime.Today, Operadora = new Operadora { Id = 3, Nome = "Tim", Codigo = "41", Categoria = "Celular", Preco = 3 } },
                //new Contato { Id = 37, Serial = "qweqweqweqweqqweq", Nome = "Marcelo", Telefone = "999999991", Cor = "blue", Data = DateTime.Today, Operadora = new Operadora { Id = 1, Nome = "Oi", Codigo = "14", Categoria = "Celular", Preco = 2 } },
                //new Contato { Id = 38, Serial = "qweqweqweqweqqweq", Nome = "Yasmin", Telefone = "999999992", Cor = "red", Data = DateTime.Today, Operadora = new Operadora { Id = 2, Nome = "Vivo", Codigo = "41", Categoria = "Celular", Preco = 1 } },
                //new Contato { Id = 39, Serial = "qweqweqweqweqqweq", Nome = "Simone", Telefone = "999999993", Cor = "green", Data = DateTime.Today, Operadora = new Operadora { Id = 3, Nome = "Tim", Codigo = "41", Categoria = "Celular", Preco = 3 } },
                //new Contato { Id = 40, Serial = "qweqweqweqweqqweq", Nome = "Marcelo", Telefone = "999999991", Cor = "blue", Data = DateTime.Today, Operadora = new Operadora { Id = 1, Nome = "Oi", Codigo = "14", Categoria = "Celular", Preco = 2 } },
                //new Contato { Id = 41, Serial = "qweqweqweqweqqweq", Nome = "Yasmin", Telefone = "999999992", Cor = "red", Data = DateTime.Today, Operadora = new Operadora { Id = 2, Nome = "Vivo", Codigo = "41", Categoria = "Celular", Preco = 1 } },
                //new Contato { Id = 42, Serial = "qweqweqweqweqqweq", Nome = "Simone", Telefone = "999999993", Cor = "green", Data = DateTime.Today, Operadora = new Operadora { Id = 3, Nome = "Tim", Codigo = "41", Categoria = "Celular", Preco = 3 } }
            };

        private static IList<Operadora> _operadoras = new List<Operadora>()
            {
                new Operadora { Id = 1, Nome = "Oi", Codigo = "14", Categoria = "Celular", Preco = 2 },
                new Operadora { Id = 2, Nome = "Vivo", Codigo = "41", Categoria = "Celular", Preco = 1 },
                new Operadora { Id = 3, Nome = "Tim", Codigo = "41", Categoria = "Celular", Preco = 3 },
                new Operadora { Id = 4, Nome = "GVT", Codigo = "25", Categoria = "Fixo", Preco = 1 },
                new Operadora { Id = 5, Nome = "Embratel", Codigo = "21", Categoria = "Fixo", Preco = 2 }
            };

        /// <summary>
        /// Recupera contatos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetContatos")]
        public IHttpActionResult GetContatos()
        {
            _result = Ok(Retorno<IEnumerable<Contato>>.Criar(true, "Consulta Realizada Com Sucesso", _contatos));

            return _result;
        }

        /// <summary>
        /// Recupera contato
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("GetContato")]
        [HttpGet]
        public IHttpActionResult GetContato(int id)
        {
            try
            {
                var contato = _contatos.Where(x => x.Id == id).SingleOrDefault();

                if (contato != null)
                {
                    //Monta response
                    _result = Ok(Retorno<Contato>.Criar(true, "Consulta Realizada Com Sucesso", contato));

                    //Retorna o response
                    return _result;
                }
                else
                    return NotFound();
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Insere contato
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Route("InsertContato")]
        [HttpPost]
        public IHttpActionResult InsertContato([FromBody]Contato dto)
        {
            try
            {
                var id = _contatos.Count() + 1;
                dto.Id = id;

                _contatos.Add(dto);

                _result = Ok(Retorno<Contato>.Criar(true, "Inclusão Realizada Com Sucesso", dto));

                return _result;
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Deleta Contato
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("DeleteContato")]
        [HttpDelete]
        public IHttpActionResult DeleteContato(int id)
        {
            try
            {
                Contato entityInDb = _contatos.Where(x => x.Id == id).SingleOrDefault();

                //Verifica se objeto existe
                if (entityInDb == null)
                    return NotFound();

                _contatos.Remove(entityInDb);

                _result = Ok(Retorno<bool>.Criar(true, "Deleção Realizada Com Sucesso", true));

                return _result;

            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Recupera operadoras
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetOperadoras")]
        public IHttpActionResult GetOperadoras()
        {
            _result = Ok(Retorno<IEnumerable<Operadora>>.Criar(true, "Consulta Realizada Com Sucesso", _operadoras));

            return _result;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Contato
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Nome { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Telefone { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Operadora Operadora { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Cor { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime Data { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Serial { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Operadora
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Nome { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Codigo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Categoria { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double Preco { get; set; }
    }
}
