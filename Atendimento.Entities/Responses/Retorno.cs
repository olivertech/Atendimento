using System.Runtime.Serialization;

namespace Atendimento.Entities.Responses
{
    /// <summary>
    /// Classe genérica usada como response 
    /// para todas as requisições
    /// feitas aos serviços
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DataContract(Name = "return")]
    public class Retorno<T>
    {
        /// <summary>
        /// Inidicador booleano de sucesso ou não da resposta
        /// </summary>
        [DataMember(Name = "success")]
        public bool Sucesso { get; private set; }

        /// <summary>
        /// Mensagem de retorno
        /// </summary>
        [DataMember(Name = "message")]
        public string Mensagem { get; private set; }

        /// <summary>
        /// Número de linhas retornadas
        /// </summary>
        [DataMember(Name = "totalRecords")]
        public int TotalRegistros { get; set; }

        /// <summary>
        /// Número de linhas retornadas
        /// </summary>
        [DataMember(Name = "totalRecordsFiltered")]
        public int TotalRegistrosFiltered { get; set; }

        /// <summary>
        /// Conteudo de resposta composto por uma classe com as propriedades preenchidas
        /// que deverão ser retornadas ao requisitante
        /// </summary>
        [DataMember(Name = "content")]
        public T Resposta { get; private set; }

        /// <summary>
        /// Métodos que criam o objeto de retorno
        /// </summary>
        /// <param name="sucesso"></param>
        /// <param name="mensagem"></param>
        /// <param name="resposta"></param>
        /// <returns></returns>
        public static Retorno<T> Criar(bool sucesso, string mensagem, T resposta)
        {
            return new Retorno<T>()
            {
                Mensagem = mensagem,
                Resposta = resposta,
                Sucesso = sucesso
            };
        }

        public static Retorno<T> Criar(bool sucesso, string mensagem, T resposta, int totalRegistros = 0, int totalRegistrosFiltrado = 0)
        {
            return new Retorno<T>()
            {
                Mensagem = mensagem,
                Resposta = resposta,
                Sucesso = sucesso,
                TotalRegistros = totalRegistros,
                TotalRegistrosFiltered = totalRegistrosFiltrado
            };
        }
    }
}