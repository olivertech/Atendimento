using System;
using System.Reflection;
using System.Text;
using Atendimento.Infra.Base;

namespace Atendimento.Infra
{
    /// <summary>
    /// Classe com métodos para tratamento de strings
    /// </summary>
    public sealed class Strings
    {
        #region Atributos

        private static Random random = new Random();

        #endregion

        #region Métodos

        /// <summary>
        /// Método que retorna o primeiro nome de um texto
        /// </summary>
        /// <param name="text">Texto</param>
        /// <returns>Primeira palavra do texto</returns>
        public static string FirstWord(string text)
        {
            string[] word;

            if (text != null)
            {
                word = text.Split(' ');
                return word[0];
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Método que retorna um texto com a primeira letra maiúscula
        /// </summary>
        /// <param name="text">Texto</param>
        /// <returns>Texto transformado</returns>
        public static string UpperFirstChar(string text)
        {
            string retorno = string.Empty;

            if (text != null)
            {
                char[] arrayCaracteres = text.ToCharArray();

                int i = 0;

                foreach (char letra in arrayCaracteres)
                {
                    if (i == 0)
                    {
                        retorno += letra.ToString().ToUpper();
                    }
                    else
                    {
                        retorno += letra.ToString().ToLower();
                    }

                    i++;
                }
            }

            return retorno;
        }

        /// <summary>
        /// Método que retorna um texto com a primeira letra maiúscula
        /// </summary>
        /// <param name="text">Texto</param>
        /// <returns>Texto transformado</returns>
        public static string UpperFirstCharAllWords(string text)
        {
            string retorno = string.Empty;

            if (text != null)
            {
                string[] words = text.Split(new char[] { ' ' });

                foreach (string word in words)
                {
                    char[] arrayCaracteres = word.ToCharArray();

                    int i = 0;

                    foreach (char letra in arrayCaracteres)
                    {
                        if (i == 0)
                        {
                            retorno += letra.ToString().ToUpper();
                        }
                        else
                        {
                            retorno += letra.ToString().ToLower();
                        }

                        i++;
                    }

                    retorno = retorno + " ";
                }
            }

            return retorno;
        }

        /// <summary>
        /// Método que remove brancos de um texto
        /// </summary>
        /// <param name="text">Texto</param>
        /// <returns>Texto transformado</returns>
        public static string RemoveBlanks(string text)
        {
            string textoSemEspaco = string.Empty;

            if (text != null)
            {
                text = text.Trim();

                if (text.Length == 0)
                {
                    return string.Empty;
                }

                char[] delimiter = { ' ' };
                string[] arrayTexto = text.Trim().Split(delimiter);
                int tamanho = arrayTexto.Length - 1;

                for (int i = 0; i <= tamanho; i++)
                {
                    if (arrayTexto[i] != "")
                    {
                        textoSemEspaco = textoSemEspaco + arrayTexto[i].Trim() + " ";
                    }
                }
            }

            return textoSemEspaco.Trim();
        }

        /// <summary>
        /// Método que remove caracteres especiais
        /// de textos que serão usados nos arquivos XML
        /// e converte todo o texto para minusculo
        /// </summary>
        /// <param name="text">Texto</param>
        /// <returns>Texto transformado</returns>
        public static string TranslateSpecialCharacterForXML(string text)
        {
            if (text != null)
            {
                text = RemoveBlanks(text.ToLower().Trim());

                text = text.Replace("'", "&apos;");
                text = text.Replace("\"", "&quot;");
                text = text.Replace("&", "&amp;");
                text = text.Replace("<", "&lt;");
                text = text.Replace(">", "&gt;");
            }

            return text;
        }

        /// <summary>
        /// Método que remove caracteres especiais
        /// de textos que serão usados nos arquivos XML,
        /// mas mantem o texto com o case original
        /// </summary>
        /// <param name="text">Texto</param>
        /// <returns>Texto transformado</returns>
        public static string TranslateSpecialCharacterForXMLOriginal(string text)
        {
            if (text != null)
            {
                text = text.Replace("'", "&apos;");
                text = text.Replace("\"", "&quot;");
                text = text.Replace("&", "&amp;");
                text = text.Replace("<", "&lt;");
                text = text.Replace(">", "&gt;");
            }

            return text;
        }

        /// <summary>
        /// Método que remove caracteres especiais 
        /// de textos, para evitar o envio de códigos
        /// maliciosos através de formulários,
        /// substituindo os espaços em branco por "_"
        /// </summary>
        /// <param name="text">Texto</param>
        /// <returns>Texto transformado</returns>
        public static string RemoveSpecialCharacter(string text)
        {
            if (text != null)
            {
                text = RemoveBlanks(text.ToLower().Trim());

                text = text.Replace("<br>", "");
                text = text.Replace('ç', 'c');
                text = text.Replace('á', 'a');
                text = text.Replace('à', 'a');
                text = text.Replace('ä', 'a');
                text = text.Replace('ã', 'a');
                text = text.Replace('ä', 'a');
                text = text.Replace('â', 'a');
                text = text.Replace('é', 'e');
                text = text.Replace('è', 'e');
                text = text.Replace('ê', 'e');
                text = text.Replace('ë', 'e');
                text = text.Replace('í', 'i');
                text = text.Replace('ì', 'i');
                text = text.Replace('î', 'i');
                text = text.Replace('ï', 'i');
                text = text.Replace('ó', 'o');
                text = text.Replace('ò', 'o');
                text = text.Replace('õ', 'o');
                text = text.Replace('ô', 'o');
                text = text.Replace('ö', 'o');
                text = text.Replace('ú', 'u');
                text = text.Replace('ù', 'u');
                text = text.Replace('ü', 'u');
                text = text.Replace('û', 'u');
                text = text.Replace(".", "");
                text = text.Replace(",", "");
                text = text.Replace(":", "");
                text = text.Replace(";", "");
                text = text.Replace("(", "");
                text = text.Replace(")", "");
                text = text.Replace("?", "");
                text = text.Replace("!", "");
                text = text.Replace("'", "");
                text = text.Replace("`", "");
                text = text.Replace("\"", "");
                text = text.Replace("/", "");
                text = text.Replace("&", "e");
                text = text.Replace("%", "");
                text = text.Replace("<", "");
                text = text.Replace(">", "");
                text = text.Replace("@", "");
                text = text.Replace("*", "");
                text = text.Replace("-", "");
                text = text.Replace("#", "");
                text = text.Replace("’", "");
                text = text.Replace("‘", "");
                text = text.Replace("º", "");
                text = text.Replace(" ", "_");
                text = text.Replace("select ", "");
                text = text.Replace("update ", "");
                text = text.Replace("insert ", "");
                text = text.Replace("delete ", "");
                text = text.Replace("truncate ", "");
                text = text.Replace("drop ", "");
            }

            return text;
        }

        /// <summary>
        /// Método que remove caracteres especiais 
        /// de textos, para evitar o envio de códigos
        /// maliciosos através de formulários, mantendo
        /// os espaços em branco
        /// </summary>
        /// <param name="text">Texto</param>
        /// <returns>Texto transformado</returns>
        public static string RemoveSpecialCharacterWithBlank(string text)
        {
            if (text != null)
            {
                text = RemoveBlanks(text.ToLower().Trim());

                text = text.Replace("<br>", "");
                text = text.Replace("(", "");
                text = text.Replace(")", "");
                text = text.Replace("'", "");
                text = text.Replace("`", "");
                text = text.Replace("\"", "");
                text = text.Replace("&", "e");
                text = text.Replace("%", "");
                text = text.Replace("<", "");
                text = text.Replace(">", "");
                text = text.Replace("*", "");
                text = text.Replace("#", "");
                text = text.Replace("’", "");
                text = text.Replace("‘", "");
                text = text.Replace("º", "");
                text = text.Replace("select ", "");
                text = text.Replace("update ", "");
                text = text.Replace("insert ", "");
                text = text.Replace("delete ", "");
                text = text.Replace("truncate ", "");
                text = text.Replace("drop ", "");
            }

            return text;
        }

        /// <summary>
        /// Método que retira caracteres espeaciais, mantem os espaços 
        /// em branco e não altera a caixa das letras.
        /// </summary>
        /// <param name="text">Texto a ser convertido</param>
        /// <returns></returns>
        public static string RemoveSpecialCharacterWithBlankWithCase(string text)
        {
            if (text != null)
            {
                text = text.Replace("<br>", "");
                //text = text.Replace("(", "");
                //text = text.Replace(")", "");
                text = text.Replace("'", "&apos;");
                text = text.Replace("`", "");
                text = text.Replace("\"", "&quot;");
                text = text.Replace("&", "&amp;");
                text = text.Replace("%", "");
                //text = text.Replace("<", "");
                //text = text.Replace(">", "");
                text = text.Replace("*", "");
                text = text.Replace("#", "");
                text = text.Replace("’", "");
                text = text.Replace("‘", "");
                text = text.Replace("º", "");
                text = text.Replace("select ", "");
                text = text.Replace("update ", "");
                text = text.Replace("insert ", "");
                text = text.Replace("delete ", "");
                text = text.Replace("truncate ", "");
                text = text.Replace("drop ", "");
            }

            return text;
        }

        /// <summary>
        /// Método que remove caracteres especiais 
        /// de textos, para evitar o envio de códigos
        /// maliciosos através de formulários, mantendo
        /// os espaços em branco
        /// </summary>
        /// <param name="text">Texto</param>
        /// <returns>Texto transformado</returns>
        public static string RemoveSpecialCharacterFromEmail(string text)
        {
            if (text != null)
            {
                text = RemoveBlanks(text.ToLower().Trim());

                text = text.Replace("<br>", "");
                text = text.Replace('ç', 'c');
                text = text.Replace('á', 'a');
                text = text.Replace('à', 'a');
                text = text.Replace('ä', 'a');
                text = text.Replace('ã', 'a');
                text = text.Replace('ä', 'a');
                text = text.Replace('â', 'a');
                text = text.Replace('é', 'e');
                text = text.Replace('è', 'e');
                text = text.Replace('ê', 'e');
                text = text.Replace('ë', 'e');
                text = text.Replace('í', 'i');
                text = text.Replace('ì', 'i');
                text = text.Replace('î', 'i');
                text = text.Replace('ï', 'i');
                text = text.Replace('ó', 'o');
                text = text.Replace('ò', 'o');
                text = text.Replace('õ', 'o');
                text = text.Replace('ô', 'o');
                text = text.Replace('ö', 'o');
                text = text.Replace('ú', 'u');
                text = text.Replace('ù', 'u');
                text = text.Replace('ü', 'u');
                text = text.Replace('û', 'u');
                text = text.Replace(",", "");
                text = text.Replace(":", "");
                text = text.Replace(";", "");
                text = text.Replace("(", "");
                text = text.Replace(")", "");
                text = text.Replace("?", "");
                text = text.Replace("!", "");
                text = text.Replace("'", "");
                text = text.Replace("`", "");
                text = text.Replace("\"", "");
                text = text.Replace("&", "e");
                text = text.Replace("%", "");
                text = text.Replace("<", "");
                text = text.Replace(">", "");
                text = text.Replace("*", "");
                text = text.Replace("#", "");
                text = text.Replace("’", "");
                text = text.Replace("‘", "");
                text = text.Replace("º", "");
                text = text.Replace("select ", "");
                text = text.Replace("update ", "");
                text = text.Replace("insert ", "");
                text = text.Replace("delete ", "");
                text = text.Replace("truncate ", "");
                text = text.Replace("drop ", "");
            }

            return text;
        }

        /// <summary>
        /// Método que remove caracteres que podem causar problemas para insert na base
        /// </summary>
        /// <param name="text">Texto</param>
        /// <returns>Texto transformado</returns>
        public static string RemoveSpecialCharacterForDataBase(string text)
        {
            if (text != null)
            {
                text = RemoveBlanks(text.Trim());

                text = text.Replace("<br>", "");
                text = text.Replace(",", "");
                text = text.Replace(":", "");
                text = text.Replace(";", "");
                text = text.Replace("(", "");
                text = text.Replace(")", "");
                text = text.Replace("?", "");
                text = text.Replace("!", "");
                text = text.Replace("'", "");
                text = text.Replace("`", "");
                text = text.Replace("\"", "");
                text = text.Replace("/", "");
                text = text.Replace("&", "e");
                text = text.Replace("%", "");
                text = text.Replace("<", "");
                text = text.Replace(">", "");
                text = text.Replace("@", "");
                text = text.Replace("*", "");
                text = text.Replace("#", "");
                text = text.Replace("’", "");
                text = text.Replace("‘", "");
                text = text.Replace("º", "");
                text = text.Replace("select ", "");
                text = text.Replace("update ", "");
                text = text.Replace("insert ", "");
                text = text.Replace("delete ", "");
                text = text.Replace("truncate ", "");
                text = text.Replace("drop ", "");
            }

            return text;
        }

        /// <summary>
        /// Método que substitui os brancos por "_"
        /// </summary>
        /// <param name="text">Texto</param>
        /// <returns>Texto transformado</returns>
        public static string ReplaceBlanks(string text)
        {
            if (text != null)
            {
                text = text.Replace(" ", "_");
            }

            return text;
        }

        /// <summary>
        /// Método que substitui os brancos por "+"
        /// </summary>
        /// <param name="text">Texto</param>
        /// <returns>Texto transformado</returns>
        public static string ReplaceBlanksWithPlus(string text)
        {
            if (text != null)
            {
                text = text.Replace(" ", "+");
            }

            return text;
        }

        /// <summary>
        /// Método que retorna sequencia de numeros
        /// de forma randomica
        /// </summary>
        /// <returns>Sequencia de números na forma de string</returns>
        public static string RandomInteger(int size)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < size; i++)
            {
                builder.Append(Convert.ToChar(Convert.ToInt32(Math.Floor(10 * random.NextDouble() + 48))));
            }
            return builder.ToString();
        }

        /// <summary>
        /// Método que retorna sequencia de caracteres
        /// de forma randomica
        /// </summary>
        /// <param name="size">Tamanho da sequencia de caracteres</param>
        /// <returns></returns>
        public static string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < size; i++)
            {
                //26 letrad do alfabeto, ascii + 65 para as letras em maiusculo
                builder.Append(Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65))));
            }
            return builder.ToString();
        }

        /// <summary>
        /// Método que substitui os "\n" por "<br />" para poder
        /// jogar o conteúdo formatado na tela
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ReplaceText4HTMLCharacter(string text)
        {
            if (text != null)
            {
                text = RemoveBlanks(text.Trim());
                text = text.Replace("\n", "<br />");
            }

            return text;
        }

        /// <summary>
        /// Método que detecta a incidência
        /// de caracteres especiais em texto
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool DetectSpecialCharacteres(string text)
        {
            if ((text.IndexOf("ç") != -1) ||
                (text.IndexOf("á") != -1) ||
                (text.IndexOf("à") != -1) ||
                (text.IndexOf("ä") != -1) ||
                (text.IndexOf("ã") != -1) ||
                (text.IndexOf("ä") != -1) ||
                (text.IndexOf("â") != -1) ||
                (text.IndexOf("é") != -1) ||
                (text.IndexOf("è") != -1) ||
                (text.IndexOf("ê") != -1) ||
                (text.IndexOf("ë") != -1) ||
                (text.IndexOf("í") != -1) ||
                (text.IndexOf("ì") != -1) ||
                (text.IndexOf("î") != -1) ||
                (text.IndexOf("ï") != -1) ||
                (text.IndexOf("ó") != -1) ||
                (text.IndexOf("ò") != -1) ||
                (text.IndexOf("õ") != -1) ||
                (text.IndexOf("ô") != -1) ||
                (text.IndexOf("ö") != -1) ||
                (text.IndexOf("ú") != -1) ||
                (text.IndexOf("ù") != -1) ||
                (text.IndexOf("ü") != -1) ||
                (text.IndexOf("û") != -1) ||
                (text.IndexOf(",") != -1) ||
                (text.IndexOf(":") != -1) ||
                (text.IndexOf(";") != -1) ||
                (text.IndexOf("(") != -1) ||
                (text.IndexOf(")") != -1) ||
                (text.IndexOf("?") != -1) ||
                (text.IndexOf("!") != -1) ||
                (text.IndexOf("'") != -1) ||
                (text.IndexOf("`") != -1) ||
                (text.IndexOf("\"") != -1) ||
                (text.IndexOf("&") != -1) ||
                (text.IndexOf("%") != -1) ||
                (text.IndexOf("<") != -1) ||
                (text.IndexOf(">") != -1) ||
                (text.IndexOf("*") != -1) ||
                (text.IndexOf("#") != -1) ||
                (text.IndexOf("’") != -1) ||
                (text.IndexOf("‘") != -1) ||
                (text.IndexOf("º") != -1) ||
                (text.IndexOf("select ") != -1) ||
                (text.IndexOf("update ") != -1) ||
                (text.IndexOf("insert ") != -1) ||
                (text.IndexOf("delete ") != -1) ||
                (text.IndexOf("truncate ") != -1) ||
                (text.IndexOf("drop ") != -1))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Método usado para recuperar a parte "String"
        /// e enumeradores que utilizam o custom attribute
        /// StringValue
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetStringValue(Enum value)
        {
            string output = null;
            Type type = value.GetType();

            FieldInfo fi = type.GetField(value.ToString());
            StringValue[] attrs = fi.GetCustomAttributes(typeof(StringValue), false) as StringValue[];

            if (attrs.Length > 0)
            {
                output = attrs[0].Value;
            }

            return output;
        }

        #endregion
    }
}