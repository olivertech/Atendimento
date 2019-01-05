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
        /// <param name="idUsuario"></param>
        /// <param name="fullFileName"></param>
        /// <returns></returns>
        public static string Compress(string pathZip, string pathAnexosUsuario, int idUsuario)
        {
            ZipFile zip = new ZipFile();

            DirectoryInfo info = new DirectoryInfo(pathAnexosUsuario);

            FileInfo[] files = info.GetFiles();

            for (int i = 0; i < files.Length; i++)
            {
                zip.AddFile(pathAnexosUsuario + "/" + files[i].Name, "");
            }

            string zipName = FormatarNomeArquivoZip(idUsuario);

            zip.Save(pathZip + "/" + zipName);

            Directory.Delete(pathAnexosUsuario, true);

            return zipName;
        }

        /// <summary>
        /// Método que muda o nome do arquivo zipado pra que todos sigam um mesmo
        /// padrão de nomenclatura no sistema, formado da seguinte maneira:
        /// 
        /// IdUsuario_<nome-original-do-anexo-sem-extensao>-<milisegundos>-<numero-aleatorio-de-4-digitos>.extens
        /// 
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public static string FormatarNomeArquivoZip(int idUsuario)
        {
            var miliseconds = (DateTime.Now - DateTime.MinValue).Ticks.ToString();
            return idUsuario.ToString() + "_anexos_" + miliseconds + "_" + RandomNumber(1000, 9999) + ".zip";
        }

        public static string RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max).ToString();
        }
    }
}
