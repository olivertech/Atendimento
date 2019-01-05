using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Atendimento.Infra.Base
{
    public class BaseUtil
    {
        #region RecuperarUrl

        /// <summary>
        /// Método que recupera a url corrente, para definir se
        /// está em ambiente de desenvolvimento ou produção, com
        /// base na appsetting Local definida na web.config
        /// </summary>
        /// <returns>Url corrente</returns>
        public string RecuperarUrl()
        {
            string url = string.Empty;

            if (ConfigurationManager.AppSettings["Local"] == "Des")
            {
                url = ConfigurationManager.AppSettings["UrlDes"].ToString();
            }
            else
            {
                url = ConfigurationManager.AppSettings["UrlProd"].ToString();
            }

            return url;
        }

        #endregion
    }
}
