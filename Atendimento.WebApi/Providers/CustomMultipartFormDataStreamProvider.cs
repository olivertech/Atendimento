using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Atendimento.WebApi.Providers
{
    /// <summary>
    /// Classe custom que trata a recepção de arquivos enviados por formulário
    /// com Content-Type = multipart/form-data
    /// </summary>
    public class CustomMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="rootPath"></param>
        public CustomMultipartFormDataStreamProvider(string rootPath)
        : base(rootPath) { }

        /// <summary>
        /// Método que sobreescreve método original do provider MultipartFormDataStreamProvider,
        /// que permite salvar todos os arquivos enviados pelo formulario, mantendo seus nomes
        /// originais
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        public override string GetLocalFileName(HttpContentHeaders headers)
        {
            var filename = headers.ContentDisposition.FileName;

            return !string.IsNullOrWhiteSpace(filename) ? filename.Replace("\"", string.Empty) : Guid.NewGuid().ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        public static implicit operator CustomMultipartFormDataStreamProvider(HttpResponseMessage v)
        {
            throw new NotImplementedException();
        }
    }
}