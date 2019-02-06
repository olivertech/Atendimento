using System;
using System.IO;
using Ionic.Zip;

namespace Atendimento.Infra
{
    public sealed class Arquivo
    {
        /// <summary>
        /// Método que comprime um anexo dentro de um arquivo zip
        /// e depois apaga o arquivo original e a pasta onde foi
        /// salvo temporariamente
        /// </summary>
        /// <param name="pathZip"></param>
        /// <param name="pathAnexosUsuario"></param>
        /// <param name="idTicket"></param>
        /// <returns></returns>
        public static string Compress(string pathZip, string pathAnexosUsuario, int idTicket)
        {
            using (var zip = new ZipFile())
            {
                var info = new DirectoryInfo(pathAnexosUsuario);

                var files = info.GetFiles();

                for (int i = 0; i < files.Length; i++)
                {
                    zip.AddFile(pathAnexosUsuario + "/" + files[i].Name, "");
                }

                var zipName = FormatarNomeArquivoZip(idTicket);

                zip.Save(pathZip + "/" + zipName);

                Directory.Delete(pathAnexosUsuario, true);

                return zipName;
            }
        }

        /// <summary>
        /// Método que muda o nome do arquivo zipado pra que todos sigam um mesmo
        /// padrão de nomenclatura no sistema
        /// </summary>
        /// <param name="idTicket"></param>
        /// <returns></returns>
        public static string FormatarNomeArquivoZip(int idTicket)
        {
            var miliseconds = (DateTime.Now - DateTime.MinValue).Ticks.ToString();
            return idTicket + "_anexos_" + miliseconds + ".zip";
        }

        /// <summary>
        /// Gera numero randomico dentro do intervalor informado
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static string RandomNumber(int min, int max)
        {
            var random = new Random();
            return random.Next(min, max).ToString();
        }
    }
}
