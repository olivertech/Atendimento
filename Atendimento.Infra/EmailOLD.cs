using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using Atendimento.Entities.Entities;
using Atendimento.Infra.Base;

namespace Atendimento.Infra
{
    /// <summary>
    /// Classe com funções de envio de email
    /// </summary>
    public sealed class EmailOLD : BaseUtil
    {
        /// <summary>
        /// Método que verifica se um endereço email é valido
        /// </summary>
        /// <param name="email">Enderço email</param>
        /// <param name="tipo">Se é um testo do tipo Leniente (leve) ou Restrito (pesado). Enviar os valores "leve" ou "pesado" para esse parametro</param>
        /// <returns>Status do email</returns>
        public static bool IsValid(string email, string tipo)
        {
            string pattern = "";

            if (tipo == "leve")
            {
                pattern = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            }
            else if (tipo == "pesado")
            {
                pattern = @"^(([^<>()[\]\\.,;:\s@\""]+"
                  + @"(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@"
                  + @"((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}"
                  + @"\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+"
                  + @"[a-zA-Z]{2,}))$";
            }
            else
            {
                pattern = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            }

            Regex reStrict = new Regex(pattern);
            bool isStrictMatch = reStrict.IsMatch(email);
            return isStrictMatch;
        }

        /// <summary>
        /// Método para envio de email de confirmação de cadastro
        /// </summary>
        /// <param name="emailTo">Endereço de destino</param>
        /// <param name="nomeTo">Nome de destino</param>
        /// <param name="emailFrom">Endereço de origem</param>
        /// <param name="nomeFrom">Nome de origem</param>
        /// <param name="body">Corpo do email</param>
        /// <param name="subject">Assunto</param>
        public static void SendNetEmail(string emailTo, string nomeTo, string body, string subject, string blindEmail)
        {
            try
            {
                System.Net.Mail.MailMessage mensagem = new System.Net.Mail.MailMessage();

                mensagem.From = new MailAddress(ConfigurationManager.AppSettings["EmailFrom"].ToString(), ConfigurationManager.AppSettings["NomeFrom"].ToString(), System.Text.Encoding.UTF8);
                mensagem.To.Add(new MailAddress(emailTo, nomeTo));

                mensagem.Body = body;
                mensagem.Subject = subject;
                mensagem.IsBodyHtml = true;
                mensagem.SubjectEncoding = System.Text.Encoding.UTF8;
                mensagem.BodyEncoding = System.Text.Encoding.UTF8;
                mensagem.Priority = System.Net.Mail.MailPriority.High;

                if (blindEmail != "")
                    mensagem.Bcc.Add(new MailAddress(blindEmail));

                if (ConfigurationManager.AppSettings["local"] == "Des")
                {
                    SmtpClient cliente = new SmtpClient(ConfigurationManager.AppSettings["SMTP"].ToString());

                    NetworkCredential credenciais = new NetworkCredential(ConfigurationManager.AppSettings["EmailFrom"].ToString(), ConfigurationManager.AppSettings["Password"].ToString());
                    cliente.Credentials = credenciais;
                    cliente.Port = Convert.ToInt16(ConfigurationManager.AppSettings["Porta"]);
                    cliente.EnableSsl = true;

                    //Defino o timeout máximo em 10 minutos para envio do email
                    cliente.Timeout = 600000;

                    cliente.Send(mensagem);
                }
                else
                {
                    SmtpClient smtp = new SmtpClient();
                    //smtp.DeliveryMethod = SmtpDeliveryMethod.PickupDirectoryFromIis; --> Usado quando o email é enviado pelo IIS do servidor de aplicação
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Port = 25;
                    smtp.Host = ConfigurationManager.AppSettings["SMTP"].ToString();
                    smtp.Send(mensagem);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        ///// <summary>
        ///// Método de envio de email no padrão anterior, usado para poder 
        ///// se compativel com o servidor smtp antigo
        ///// </summary>
        ///// <param name="emailTo"></param>
        ///// <param name="body"></param>
        ///// <param name="subject"></param>
        //public static void SendWebEmail(string emailTo, string body, string subject)
        //{
        //    try
        //    {
        //        System.Web.Mail.MailMessage mensagem = new System.Web.Mail.MailMessage();
        //        mensagem.To = emailTo;
        //        mensagem.From = ConfigurationManager.AppSettings["EmailComercial"].ToString();
        //        mensagem.Subject = subject;
        //        mensagem.BodyFormat = MailFormat.Html;
        //        mensagem.Body = body;
        //        mensagem.Priority = System.Web.Mail.MailPriority.High;
        //        mensagem.BodyEncoding = System.Text.Encoding.UTF8;

        //        SmtpMail.SmtpServer = ConfigurationManager.AppSettings["SMTP"].ToString();
        //        SmtpMail.Send(mensagem);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        /// <summary>
        /// Método de envio de email no padrão anterior, usado para poder 
        /// se compativel com o servidor smtp do radar55
        /// </summary>
        /// <param name="emailTo"></param>
        /// <param name="body"></param>
        /// <param name="subject"></param>
        public static void SendNetEmail(string emailFrom, string emailTo, string emailReply, string body, string subject, bool FormatoHTML, System.Net.Mail.MailPriority prioridade)
        {
            try
            {
                MailMessage mensagem = new MailMessage();

                //======================================================
                //ATENÇÃO PARA NÃO ESQUECER DE TROCAR AS LINHAS ABAIXO
                //======================================================

                //Usar esse bloco para testes
                //mensagem.From = new MailAddress("comercial@algorix.com");
                //mensagem.To.Add(new MailAddress("olivertech@terra.com.br"));

                //Usar esse bloco para produção
                mensagem.From = new MailAddress(emailFrom);
                mensagem.To.Add(new MailAddress(emailTo));

                if (emailReply != string.Empty)
                    mensagem.ReplyToList.Add(new MailAddress(emailReply));

                //======================================================

                mensagem.Body = body;
                mensagem.Subject = subject;
                mensagem.IsBodyHtml = FormatoHTML;
                mensagem.SubjectEncoding = Encoding.UTF8;
                mensagem.BodyEncoding = Encoding.UTF8;
                mensagem.Priority = prioridade;

                if (ConfigurationManager.AppSettings["local"] == "Des")
                {
                    SmtpClient cliente = new SmtpClient(ConfigurationManager.AppSettings["SMTP"].ToString());

                    NetworkCredential credenciais = new NetworkCredential(ConfigurationManager.AppSettings["EmailSuporte"].ToString(), ConfigurationManager.AppSettings["Password"].ToString());
                    cliente.Credentials = credenciais;
                    cliente.Port = Convert.ToInt16(ConfigurationManager.AppSettings["Porta"]);
                    cliente.EnableSsl = true;

                    ServicePointManager.ServerCertificateValidationCallback = delegate (object s,
                        System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                        System.Security.Cryptography.X509Certificates.X509Chain chain,
                        System.Net.Security.SslPolicyErrors sslPolicyErrors)
                    {
                        return true;
                    };

                    //Defino o timeout máximo em 10 minutos para envio do email
                    cliente.Timeout = 600000;

                    cliente.Send(mensagem);
                }
                else
                {
                    SmtpClient smtp = new SmtpClient();
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Port = 25;
                    smtp.Host = ConfigurationManager.AppSettings["SMTP"].ToString();
                    smtp.Send(mensagem);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método que envia email
        /// </summary>
        /// <param name="emailFrom">Email From</param>
        /// <param name="emailTo">Email To</param>
        /// <param name="emailCopy">Email a ser copiado (CC)</param>
        /// <param name="body">Corpo do email</param>
        /// <param name="subject">Assunto do email</param>
        /// <param name="FormatoHTML">Se é no formato HTML</param>
        /// <param name="prioridade">Prioridade do email</param>
        public static void SendNetEmailWithCopy(string emailFrom, string emailTo, string emailCopy, string body, string subject, bool FormatoHTML, System.Net.Mail.MailPriority prioridade)
        {
            try
            {
                System.Net.Mail.MailMessage mensagem = new System.Net.Mail.MailMessage();

                //======================================================
                //ATENÇÃO PARA NÃO ESQUECER DE TROCAR AS LINHAS ABAIXO
                //======================================================

                //Usar esse bloco para testes
                //mensagem.From = new MailAddress("comercial@algorix.com");
                //mensagem.To.Add(new MailAddress("olivertech@terra.com.br"));

                //Usar esse bloco para produção
                mensagem.From = new MailAddress("no-reply@algorix.com");
                mensagem.To.Add(new MailAddress(emailTo));

                if (emailCopy != string.Empty)
                    mensagem.CC.Add(new MailAddress(emailCopy));

                //======================================================

                mensagem.ReplyToList.Add(new MailAddress("no-reply@algorix.com"));
                mensagem.Body = body;
                mensagem.Subject = subject;
                mensagem.IsBodyHtml = FormatoHTML;
                mensagem.SubjectEncoding = System.Text.Encoding.UTF8;
                mensagem.BodyEncoding = System.Text.Encoding.UTF8;
                mensagem.Priority = prioridade;

                if (ConfigurationManager.AppSettings["local"] == "Des")
                {
                    SmtpClient cliente = new SmtpClient(ConfigurationManager.AppSettings["SMTP"].ToString());

                    NetworkCredential credenciais = new NetworkCredential(ConfigurationManager.AppSettings["EmailFaleConosco"].ToString(), ConfigurationManager.AppSettings["Password"].ToString());
                    cliente.Credentials = credenciais;
                    cliente.Port = Convert.ToInt16(ConfigurationManager.AppSettings["Porta"]);
                    cliente.EnableSsl = true;

                    //Defino o timeout máximo em 10 minutos para envio do email
                    cliente.Timeout = 600000;

                    cliente.Send(mensagem);
                }
                else
                {
                    SmtpClient smtp = new SmtpClient
                    {
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        Port = 25,
                        Host = ConfigurationManager.AppSettings["SMTP"].ToString()
                    };
                    smtp.Send(mensagem);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        ///// <summary>
        ///// Método que formata o corpo do email de esqueci a senha
        ///// </summary>
        ///// <param name="dto">DTO com os dados do email</param>
        ///// <returns>String com o corpo do email formatado</returns>
        //public static string FormatarCorpoEmailFaleConoscoHTML(FaleConoscoDTO dto)
        //{
        //    StringBuilder corpo = new StringBuilder();

        //    corpo.Append("<html>");
        //    corpo.Append("<head>");
        //    corpo.Append("<title>Algorix</title>");
        //    corpo.Append("<style type=\"text/css\">");
        //    corpo.Append("body");
        //    corpo.Append("{");
        //    corpo.Append("    margin:0px;");
        //    corpo.Append("    padding:0px;");
        //    corpo.Append("    font-family: Segoe UI;");
        //    corpo.Append("    height:100%;");
        //    corpo.Append("    width:100%;");
        //    corpo.Append("    font-size:11pt;");
        //    corpo.Append("}");
        //    corpo.Append("</style>");
        //    corpo.Append("</head>");
        //    corpo.Append("<body>");
        //    corpo.Append("<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" style=\"width:700px;\">");
        //    corpo.Append("<tr><td colspan=\"2\"><font style=\"font-weight:bold;font-size:12pt;\">Atendimento Algorix,</font><br /><br /></td></tr>");
        //    corpo.Append("<tr><td colspan=\"2\">Seguem os dados do Fale Conosco:<br /><br /></td></tr>");
        //    corpo.Append("<tr><td colspan=\"2\" style=\"vertical-align:top;width:50px;font-weight:bold;font-size:12pt;\">Dados de Contato<br /></td></tr>");
        //    corpo.Append("<tr><td style=\"vertical-align:top;width:80px\">Empresa :</td><td style=\"vertical-align:top;width:150px\">" + dto.empresa + "</td></tr>");
        //    corpo.Append("<tr><td style=\"vertical-align:top;width:80px\">Nome :</td><td style=\"vertical-align:top;width:150px\">" + dto.nome + "</td></tr>");
        //    corpo.Append("<tr><td style=\"vertical-align:top;width:80px\">Email :</td><td style=\"vertical-align:top;width:150px\">" + dto.email + "</td></tr>");
        //    corpo.Append("<tr><td style=\"vertical-align:top;width:80px\">Telefone :</td><td style=\"vertical-align:top;width:150px\">" + (dto.telefone.Length > 0 ? dto.telefone : "-") + "</td></tr>");
        //    corpo.Append("<tr><td style=\"vertical-align:top;width:80px\">Celular :</td><td style=\"vertical-align:top;width:150px\">" + (dto.celular.Length > 0 ? dto.celular : "-") + "</td></tr>");
        //    corpo.Append("<tr><td style=\"vertical-align:top;width:80px\">Estado :</td><td style=\"vertical-align:top;width:150px\">" + (dto.UF.Length > 0 ? dto.UF : "-") + "</td></tr>");
        //    corpo.Append("<tr><td colspan=\"2\" style=\"vertical-align:top;width:50px;font-weight:bold;font-size:12pt;\"><br />Sobre a Empresa<br /></td></tr>");
        //    corpo.Append("<tr><td style=\"vertical-align:top;width:80px\">Clientes que pretende alcançar :</td><td style=\"vertical-align:top;width:150px\">" + (dto.qtdClientes.Length > 0 ? dto.qtdClientes : "-") + "</td></tr>");
        //    corpo.Append("<tr><td style=\"vertical-align:top;width:80px\">Venda mensal :</td><td style=\"vertical-align:top;width:150px\">" + (dto.vendaMensal.Length > 0 ? "R$ " + dto.vendaMensal : "-") + "</td></tr>");
        //    corpo.Append("<tr><td style=\"vertical-align:top;width:80px\">Quantidade de lojas :</td><td style=\"vertical-align:top;width:150px\">" + (dto.qtdLojas.Length > 0 ? dto.qtdLojas : "-") + "</td></tr>");
        //    corpo.Append("<tr><td style=\"vertical-align:top;width:80px\">Área de atuação :</td><td style=\"vertical-align:top;width:150px\">" + (dto.areaAtuacaoTexto.Length > 0 ? dto.areaAtuacaoTexto : "-") + "</td></tr>");

        //    if (dto.outraAreaAtuacao.Trim().Length > 0)
        //    {
        //        corpo.Append("<tr><td style=\"vertical-align:top;width:80px\">Outra área de atuação :</td><td style=\"vertical-align:top;width:150px\">" + dto.outraAreaAtuacao + "</td></tr>");
        //    }

        //    corpo.Append("<tr><td style=\"vertical-align:top;width:80px\">Produtos vendidos :</td><td style=\"vertical-align:top;width:150px\">" + (dto.produtosVendidos.Length > 0 ? dto.produtosVendidos : "-") + "</td></tr>");
        //    corpo.Append("<tr><td style=\"vertical-align:top;width:80px\">URL :</td><td style=\"vertical-align:top;width:150px\">" + (dto.URL.Length > 0 ? dto.URL : "-") + "</td></tr>");
        //    corpo.Append("<tr><td style=\"vertical-align:top;width:80px\">Mensagem :</td><td style=\"vertical-align:top;width:150px\">" + (dto.comentario.Length > 0 ? dto.comentario : "-") + "</td></tr>");
        //    corpo.Append("<tr><td colspan=\"2\" style=\"vertical-align:top;width:50px;font-weight:bold;font-size:12pt;\"><br />Origem do contato<br /></td></tr>");
        //    corpo.Append("<tr><td style=\"vertical-align:top;width:80px\">IP :</td><td style=\"vertical-align:top;width:150px\">" + dto.IP + "</td></tr>");
        //    corpo.Append("<tr><td style=\"vertical-align:top;width:80px\">Origem :</td><td style=\"vertical-align:top;width:150px\">" + (dto.origem.Length > 0 ? dto.origem : "-") + "</td></tr>");
        //    corpo.Append("</table>");
        //    corpo.Append("</body>");
        //    corpo.Append("</html>");

        //    return corpo.ToString();
        //}

        ///// <summary>
        ///// Método que formata o corpo do email de esqueci a senha
        ///// </summary>
        ///// <param name="dto">DTO com os dados do email</param>
        ///// <returns>String com o corpo do email formatado</returns>
        //public static string FormatarCorpoEmailFaleConoscoTXT(FaleConoscoDTO dto)
        //{
        //    StringBuilder corpo = new StringBuilder();

        //    corpo.Append("Atendimento Algorix,\n\n");
        //    corpo.Append("Seguem os dados do Fale Conosco.\n\n");
        //    corpo.Append("Dados de Contato\n\n");
        //    corpo.Append("Empresa : " + dto.empresa + "\n");
        //    corpo.Append("Nome : " + dto.nome + "\n");
        //    corpo.Append("Email : " + dto.email + "\n");
        //    corpo.Append("Telefone : " + (dto.telefone.Length > 0 ? dto.telefone + "\n" : "-\n"));
        //    corpo.Append("Celular : " + (dto.celular.Length > 0 ? dto.celular + "\n" : "-\n"));
        //    corpo.Append("Estado : " + (dto.UF.Length > 0 ? dto.UF + "\n\n" : "-\n\n"));
        //    corpo.Append("Sobre a Empresa\n\n");
        //    corpo.Append("Clientes que pretende alcançar : " + (dto.qtdClientes.Length > 0 ? dto.qtdClientes : "-"));
        //    corpo.Append(Environment.NewLine);
        //    corpo.Append("Venda mensal : R$ " + (dto.vendaMensal != "0,00" ? dto.vendaMensal : "-"));
        //    corpo.Append("\n");
        //    corpo.Append("Quantidade de lojas : " + (dto.qtdLojas.Length > 0 ? dto.qtdLojas : "-"));
        //    corpo.Append("\n");
        //    corpo.Append("Área de atuação : " + (dto.areaAtuacaoTexto != null ? dto.areaAtuacaoTexto : "-"));
        //    corpo.Append("\n");
        //    if ((dto.outraAreaAtuacao != null) && (dto.outraAreaAtuacao != ""))
        //        corpo.Append("Outra área de atuação : " + dto.outraAreaAtuacao);
        //    corpo.Append("\n");
        //    corpo.Append("Produtos vendidos : " + (dto.produtosVendidos.Length > 0 ? dto.produtosVendidos : "-"));
        //    corpo.Append("\n");
        //    corpo.Append("URL : " + (dto.URL.Length > 0 ? dto.URL : "-"));
        //    corpo.Append("\n");
        //    corpo.Append("Mensagem : " + (dto.comentario.Length > 0 ? dto.comentario : "-"));
        //    corpo.Append("\n");
        //    corpo.Append("Origem do contato\n\n");
        //    corpo.Append("IP : " + dto.IP);
        //    corpo.Append("\n");
        //    corpo.Append("Origem : " + (dto.origem.Length > 0 ? dto.origem : "-"));
        //    corpo.Append("\n\n");

        //    return corpo.ToString();
        //}

        /// <summary>
        /// Método que formata o corpo do email de esqueci a senha
        /// </summary>
        /// <param name="dto">DTO com os dados do email</param>
        /// <returns>String com o corpo do email formatado</returns>
        public static string FormatarCorpoEmailEsqueciSenha(UserLogin userLogin)
        {
            StringBuilder corpo = new StringBuilder();

            string logo = RecuperarUrl() + "Images/logo-text.png";

            corpo.Append("<html>");
            corpo.Append("<head>");
            corpo.Append("<title>SAC - Sistema de Atendimento ao Cliente</title>");
            corpo.Append("<style type=\"text/css\">");
            corpo.Append("body");
            corpo.Append("{");
            corpo.Append("    margin:0px;");
            corpo.Append("    padding:0px;");
            corpo.Append("    font-family: Trebuchet MS, Lucida Sans Unicode, Arial, sans-serif;");
            corpo.Append("    height:100%;");
            corpo.Append("    width:100%;");
            corpo.Append("    font-size:11pt;");
            corpo.Append("}");
            corpo.Append("</style>");
            corpo.Append("</head>");
            corpo.Append("<body>");
            corpo.Append("<table cellspacing=\"4\" cellpadding=\"4\" border=\"0\" style=\"width:700px;\">");
            corpo.Append("<tr><td style=\"text-align: left; vertical-align: middle\"><img src=\"" + logo + "\" alt=\"\" /></td></tr>");
            corpo.Append("<tr><td><br />");
            corpo.Append("Prezado(a) " + userLogin.Nome + ",<br /><br />");

            corpo.Append("Segue a sua senha de acesso ao sistema de atendimento Algorix.\n\n");
            corpo.Append("Senha : " + userLogin.Password + "\n\n");

            corpo.Append("Atenciosamente,<br /><br />");
            corpo.Append(ConfigurationManager.AppSettings["Assinatura"] + "<br />");
            corpo.Append("<a href='" + ConfigurationManager.AppSettings["UrlAssinatura"] + "'>" + ConfigurationManager.AppSettings["SiteAssinatura"] + "</a><br />");
            corpo.Append(ConfigurationManager.AppSettings["TelefonesAssinatura"]  + "<br />");
            corpo.Append("</td></tr>");
            corpo.Append("</table>");
            corpo.Append("</body>");
            corpo.Append("</html>");

            return corpo.ToString();
        }

        /// <summary>
        /// Método que formata o corpo do email 
        /// enviado para o suporte de atendimento
        /// do site Algorix
        /// </summary>
        /// <param name="errorMessage">Mensagem de erro</param>
        /// <returns>String com o corpo do email formatado</returns>
        public static string FormatarCorpoEmailSuporte(string errorMessagem)
        {
            StringBuilder corpo = new StringBuilder();

            corpo.Append("Atendimento Algorix,\n\n");
            corpo.Append("Seguem mensagem de erro no sistema de atendimento.\n\n");
            corpo.Append("Mensagem de erro : " + errorMessagem + "\n");

            return corpo.ToString();
        }

        ///// <summary>
        ///// Método que formata o corpo do email 
        ///// enviado para o suporte com os dados
        ///// do atendimento solicitado no site Algorix
        ///// </summary>
        ///// <param name="idTicket">Id do chamado</param>
        ///// <param name="tipo">Tipo de email a ser enviado</param>
        ///// <returns>String com o corpo do email formatado</returns>
        //public static string FormatarCorpoEmailSuporteAtendimentoTXT(TicketDTO dto, string tipo)
        //{
        //    StringBuilder corpo = new StringBuilder();
        //    string url = string.Empty;

        //    if (ConfigurationManager.AppSettings["local"].ToString() == "Des")
        //        url = ConfigurationManager.AppSettings["UrlDes"].ToString();
        //    else
        //        url = ConfigurationManager.AppSettings["UrlProd"].ToString();

        //    corpo.Append("Atendimento Algorix,\n\n");

        //    if (tipo == "Interno")
        //    {
        //        corpo.Append("Foi enviada uma nova mensagem interna. Seguem os dados do chamado e a nova mensagem:\n\n");
        //        corpo.Append("Usuário\n");
        //        corpo.Append("---------\n");
        //        corpo.Append(dto.NmUsuario + " (" + dto.NmEmail + ")\n\n");
        //        corpo.Append("Categoria\n");
        //        corpo.Append("-----------\n");
        //        corpo.Append(dto.NmCategoria + "\n\n");
        //        corpo.Append("Título\n");
        //        corpo.Append("-------\n");
        //        corpo.Append(dto.NmTitulo + "\n\n");
        //        corpo.Append("Descrição\n");
        //        corpo.Append("-----------\n");
        //        corpo.Append(dto.DsTicket + "\n\n");
        //        corpo.Append("Mensagem Interna\n");
        //        corpo.Append("------------------\n");
        //        corpo.Append(dto.DsMensagem + "\n\n");
        //    }
        //    else if (tipo == "Novo")
        //    {
        //        corpo.Append("Foi enviado novo chamado. Seguem os dados do novo chamado:\n\n");
        //        corpo.Append("Usuário\n");
        //        corpo.Append("---------\n");
        //        corpo.Append(dto.NmUsuario + " (" + dto.NmEmail + ")\n\n");
        //        corpo.Append("Categoria\n");
        //        corpo.Append("-----------\n");
        //        corpo.Append(dto.NmCategoria + "\n\n");
        //        corpo.Append("Título\n");
        //        corpo.Append("-------\n");
        //        corpo.Append(dto.NmTitulo + "\n\n");
        //        corpo.Append("Descrição\n");
        //        corpo.Append("-----------\n");
        //        corpo.Append(dto.DsTicket + "\n\n");
        //    }
        //    else if (tipo == "Resposta")
        //    {
        //        corpo.Append("Foi enviada nova resposta a chamado. Seguem os dados do chamado e a nova mensagem:\n\n");
        //        corpo.Append("Usuário\n");
        //        corpo.Append("---------\n");
        //        corpo.Append(dto.NmUsuario + " (" + dto.NmEmail + ")\n\n");
        //        corpo.Append("Categoria\n");
        //        corpo.Append("-----------\n");
        //        corpo.Append(dto.NmCategoria + "\n\n");
        //        corpo.Append("Título\n");
        //        corpo.Append("-------\n");
        //        corpo.Append(dto.NmTitulo + "\n\n");
        //        corpo.Append("Descrição\n");
        //        corpo.Append("-----------\n");
        //        corpo.Append(dto.DsTicket + "\n\n");
        //        corpo.Append("Nova Mensagem\n");
        //        corpo.Append("---------------\n");
        //        corpo.Append(dto.DsMensagem + "\n\n");
        //    }
        //    else if (tipo == "Concluido")
        //    {
        //        corpo.Append("Foi concluído com sucesso o chamado de número " + dto.IdTicket.ToString() + ". Seguem os dados do chamado concluído:\n\n");
        //        corpo.Append("Usuário\n");
        //        corpo.Append("---------\n");
        //        corpo.Append(dto.NmUsuario + " (" + dto.NmEmail + ")\n\n");
        //        corpo.Append("Categoria\n");
        //        corpo.Append("-----------\n");
        //        corpo.Append(dto.NmCategoria + "\n\n");
        //        corpo.Append("Título\n");
        //        corpo.Append("-------\n");
        //        corpo.Append(dto.NmTitulo + "\n\n");
        //        corpo.Append("Descrição\n");
        //        corpo.Append("-----------\n");
        //        corpo.Append(dto.DsTicket + "\n\n");
        //    }
        //    else if (tipo == "Cancelado")
        //    {
        //        corpo.Append("Foi cancelado com sucesso o chamado de número " + dto.IdTicket.ToString() + ". Seguem os dados do chamado cancelado:\n\n");
        //        corpo.Append("Usuário\n");
        //        corpo.Append("---------\n");
        //        corpo.Append(dto.NmUsuario + " (" + dto.NmEmail + ")\n\n");
        //        corpo.Append("Categoria\n");
        //        corpo.Append("-----------\n");
        //        corpo.Append(dto.NmCategoria + "\n\n");
        //        corpo.Append("Título\n");
        //        corpo.Append("-------\n");
        //        corpo.Append(dto.NmTitulo + "\n\n");
        //        corpo.Append("Descrição\n");
        //        corpo.Append("-----------\n");
        //        corpo.Append(dto.DsTicket + "\n\n");
        //    }

        //    corpo.Append("Link do Chamado\n");
        //    corpo.Append("-------------------\n");

        //    string link = "<a href='" + url + "suporte/Default.aspx?iduser=1&id=" + dto.IdTicket + "' target='_blank'>Clique aqui para acessar o chamado</a>";

        //    corpo.Append(link + "\n\n");

        //    return corpo.ToString();
        //}

        ///// <summary>
        ///// Método que formata o corpo do email 
        ///// enviado para o suporte com os dados
        ///// do atendimento solicitado no site Algorix
        ///// </summary>
        ///// <param name="idTicket">Id do chamado</param>
        ///// <param name="tipo">Tipo de email a ser enviado</param>
        ///// <returns>String com o corpo do email formatado</returns>
        //public static string FormatarCorpoEmailSuporteAtendimentoHTML(TicketDTO dto, string tipo)
        //{
        //    StringBuilder corpo = new StringBuilder();
        //    string url = string.Empty;

        //    if (ConfigurationManager.AppSettings["local"].ToString() == "Des")
        //        url = ConfigurationManager.AppSettings["UrlDes"].ToString();
        //    else
        //        url = ConfigurationManager.AppSettings["UrlProd"].ToString();

        //    try
        //    {
        //        corpo.Append("<html>");
        //        corpo.Append("<head>");
        //        corpo.Append("<title>SAC - Sistema de Atendimento ao Cliente Algorix</title>");
        //        corpo.Append("<style type=\"text/css\">");
        //        corpo.Append("body");
        //        corpo.Append("{");
        //        corpo.Append("    margin:0px;");
        //        corpo.Append("    padding:0px;");
        //        corpo.Append("    font-family: Trebuchet MS, Lucida Sans Unicode, Arial, sans-serif;");
        //        corpo.Append("    height:100%;");
        //        corpo.Append("    width:100%;");
        //        corpo.Append("    font-size:11pt;");
        //        corpo.Append("}");
        //        corpo.Append("</style>");
        //        corpo.Append("</head>");
        //        corpo.Append("<body>");
        //        corpo.Append("Atendimento Algorix,<br /><br />");

        //        if (tipo == "Interno")
        //        {
        //            corpo.Append("Foi enviada uma nova mensagem interna. Seguem os dados do chamado e a nova mensagem:<br /><br />");

        //            corpo.Append("<table style=\"width: 70%;\" cellpadding=\"4\" cellspacing=\"4\">");
        //            corpo.Append("    <tr><td style=\"width:20%; vertical-align:top\"><strong>Usuário</strong></td>");
        //            corpo.Append("        <td style=\"width:80%\">" + dto.NmUsuario + " (" + dto.NmEmail + ")</td></tr>");
        //            corpo.Append("    <tr><td style=\"width:20%; vertical-align:top\"><strong>Categoria</strong></td>");
        //            corpo.Append("        <td>" + dto.NmCategoria + "</td></tr>");
        //            corpo.Append("    <tr><td style=\"width:20%; vertical-align:top\"><strong>Título</strong></td>");
        //            corpo.Append("        <td>" + dto.NmTitulo + "</td></tr>");
        //            corpo.Append("    <tr><td style=\"width:20%; vertical-align:top\"><strong>Descrição</strong></td>");
        //            corpo.Append("        <td>" + dto.DsTicket.Replace("\n", "<br />").Replace("\r", "") + "</td></tr>");
        //            corpo.Append("    <tr><td style=\"width:20%; vertical-align:top\"><strong>Mensagem Interna</strong></td>");
        //            corpo.Append("        <td>" + dto.DsMensagem.Replace("\n", "<br />").Replace("\r", "") + "</td></tr>");
        //        }
        //        else if (tipo == "Novo")
        //        {
        //            corpo.Append("Foi enviado novo chamado. Seguem os dados do novo chamado:<br /><br />");

        //            corpo.Append("<table style=\"width: 70%;\" cellpadding=\"4\" cellspacing=\"4\">");
        //            corpo.Append("    <tr><td style=\"width:20%; vertical-align:top\"><strong>Usuário</strong></td>");
        //            corpo.Append("        <td style=\"width:80%\">" + dto.NmUsuario + " (" + dto.NmEmail + ")</td></tr>");
        //            corpo.Append("    <tr><td style=\"width:20%; vertical-align:top\"><strong>Categoria</strong></td>");
        //            corpo.Append("        <td>" + dto.NmCategoria + "</td></tr>");
        //            corpo.Append("    <tr><td style=\"width:20%; vertical-align:top\"><strong>Título</strong></td>");
        //            corpo.Append("        <td>" + dto.NmTitulo + "</td></tr>");
        //            corpo.Append("    <tr><td style=\"width:20%; vertical-align:top\"><strong>Descrição</strong></td>");
        //            corpo.Append("        <td>" + dto.DsTicket.Replace("\n", "<br />").Replace("\r", "") + "</td></tr>");
        //        }
        //        else if (tipo == "Resposta")
        //        {
        //            corpo.Append("Foi enviada nova resposta a chamado. Seguem os dados do chamado e a nova mensagem:<br /><br />");

        //            corpo.Append("<table style=\"width: 70%;\" cellpadding=\"4\" cellspacing=\"4\">");
        //            corpo.Append("    <tr><td style=\"width:20%; vertical-align:top\"><strong>Usuário</strong></td>");
        //            corpo.Append("        <td style=\"width:80%\">" + dto.NmUsuario + " (" + dto.NmEmail + ")</td></tr>");
        //            corpo.Append("    <tr><td style=\"width:20%; vertical-align:top\"><strong>Categoria</strong></td>");
        //            corpo.Append("        <td>" + dto.NmCategoria + "</td></tr>");
        //            corpo.Append("    <tr><td style=\"width:20%; vertical-align:top\"><strong>Título</strong></td>");
        //            corpo.Append("        <td>" + dto.NmTitulo + "</td></tr>");
        //            corpo.Append("    <tr><td style=\"width:20%; vertical-align:top\"><strong>Descrição</strong></td>");
        //            corpo.Append("        <td>" + dto.DsTicket.Replace("\n", "<br />").Replace("\r", "") + "</td></tr>");
        //            corpo.Append("    <tr><td style=\"width:20%; vertical-align:top\"><strong>Nova Mensagem</strong></td>");
        //            corpo.Append("        <td>" + dto.DsMensagem.Replace("\n", "<br />").Replace("\r", "") + "</td></tr>");
        //        }
        //        else if (tipo == "Concluido")
        //        {
        //            corpo.Append("Foi concluído com sucesso o chamado de número " + dto.IdTicket.ToString() + ". Seguem os dados do chamado concluído:<br /><br />");

        //            corpo.Append("<table style=\"width: 70%;\" cellpadding=\"4\" cellspacing=\"4\">");
        //            corpo.Append("    <tr><td style=\"width:20%; vertical-align:top\"><strong>Usuário</strong></td>");
        //            corpo.Append("        <td style=\"width:80%\">" + dto.NmUsuario + " (" + dto.NmEmail + ")</td></tr>");
        //            corpo.Append("    <tr><td style=\"width:20%; vertical-align:top\"><strong>Categoria</strong></td>");
        //            corpo.Append("        <td>" + dto.NmCategoria + "</td></tr>");
        //            corpo.Append("    <tr><td style=\"width:20%; vertical-align:top\"><strong>Título</strong></td>");
        //            corpo.Append("        <td>" + dto.NmTitulo + "</td></tr>");
        //            corpo.Append("    <tr><td style=\"width:20%; vertical-align:top\"><strong>Descrição</strong></td>");
        //            corpo.Append("        <td>" + dto.DsTicket.Replace("\n", "<br />").Replace("\r", "") + "</td></tr>");
        //        }
        //        else if (tipo == "Cancelado")
        //        {
        //            corpo.Append("Foi cancelado com sucesso o chamado de número " + dto.IdTicket.ToString() + ". Seguem os dados do chamado cancelado:<br /><br />");

        //            corpo.Append("<table style=\"width: 70%;\" cellpadding=\"4\" cellspacing=\"4\">");
        //            corpo.Append("    <tr><td style=\"width:20%; vertical-align:top\"><strong>Usuário</strong></td>");
        //            corpo.Append("        <td style=\"width:80%\">" + dto.NmUsuario + " (" + dto.NmEmail + ")</td></tr>");
        //            corpo.Append("    <tr><td style=\"width:20%; vertical-align:top\"><strong>Categoria</strong></td>");
        //            corpo.Append("        <td>" + dto.NmCategoria + "</td></tr>");
        //            corpo.Append("    <tr><td style=\"width:20%; vertical-align:top\"><strong>Título</strong></td>");
        //            corpo.Append("        <td>" + dto.NmTitulo + "</td></tr>");
        //            corpo.Append("    <tr><td style=\"width:20%; vertical-align:top\"><strong>Descrição</strong></td>");
        //            corpo.Append("        <td>" + dto.DsTicket.Replace("\n", "<br />").Replace("\r", "") + "</td></tr>");
        //        }

        //        corpo.Append("    <tr><td style=\"width:20%; vertical-align:top\"><strong>Link do chamado</strong></td>");

        //        string link = "<a href='" + url + "suporte/Default.aspx?iduser=1&id=" + dto.IdTicket + "' target='_blank'>Clique aqui para acessar o chamado</a>";

        //        corpo.Append("        <td>" + link + "</td></tr> ");
        //        corpo.Append("</table>");
        //        corpo.Append("</body>");
        //        corpo.Append("</html>");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("(FormatarCorpoEmailSuporteAtendimentoHTML) - " + ex.Message);
        //    }

        //    return corpo.ToString();
        //}

        /// <summary>
        /// Método que formata o corpo do email 
        /// enviado para o suporte com os dados
        /// do atendimento solicitado no site Algorix
        /// </summary>
        /// <param name="idTicket">Id do chamado</param>
        /// <param name="nomeUsuario">Nome do usuário</param>
        /// <returns>String com o corpo do email formatado</returns>
        public static string FormatarCorpoNovoChamadoCliente(int idTicket, string nomeUsuario)
        {
            StringBuilder corpo = new StringBuilder();

            string logo = RecuperarUrl() + "Images/logo-text.png";

            corpo.Append("<html>");
            corpo.Append("<head>");
            corpo.Append("<title>SAC - Sistema de Atendimento ao Cliente</title>");
            corpo.Append("<style type=\"text/css\">");
            corpo.Append("body");
            corpo.Append("{");
            corpo.Append("    margin:0px;");
            corpo.Append("    padding:0px;");
            corpo.Append("    font-family: Trebuchet MS, Lucida Sans Unicode, Arial, sans-serif;");
            corpo.Append("    height:100%;");
            corpo.Append("    width:100%;");
            corpo.Append("    font-size:11pt;");
            corpo.Append("}");
            corpo.Append("</style>");
            corpo.Append("</head>");
            corpo.Append("<body>");
            corpo.Append("<table cellspacing=\"4\" cellpadding=\"4\" border=\"0\" style=\"width:700px;\">");
            corpo.Append("<tr><td style=\"text-align: left; vertical-align: middle\"><img src=\"" + logo + "\" alt=\"\" /></td></tr>");
            corpo.Append("<tr><td><br />");
            corpo.Append("Prezado(a) " + nomeUsuario + ",<br /><br />");

            corpo.Append("Confirmamos o recebimento do novo chamado de número " + idTicket.ToString() + ". Estaremos enviando resposta em breve.<br /><br />");

            corpo.Append("Atenciosamente,<br /><br />");
            corpo.Append(ConfigurationManager.AppSettings["Assinatura"] + "<br />");
            corpo.Append("<a href='" + ConfigurationManager.AppSettings["UrlAssinatura"] + "'>" + ConfigurationManager.AppSettings["SiteAssinatura"] + "</a><br />");
            corpo.Append(ConfigurationManager.AppSettings["TelefonesAssinatura"] + "<br />");
            corpo.Append("</td></tr>");
            corpo.Append("</table>");
            corpo.Append("</body>");
            corpo.Append("</html>");

            return corpo.ToString();
        }

        /// <summary>
        /// Método que formata o corpo do email 
        /// enviado para o suporte com os dados
        /// do atendimento solicitado no site Algorix
        /// </summary>
        /// <param name="idTicket">Id do chamado</param>
        /// <param name="nomeUsuario">Nome do usuário</param>
        /// <returns>String com o corpo do email formatado</returns>
        public static string FormatarCorpoNovaMensagem(int idTicket, string nomeUsuario, string mensagem)
        {
            StringBuilder corpo = new StringBuilder();

            string logo = RecuperarUrl() + "Images/logo-text.png";

            corpo.Append("<html>");
            corpo.Append("<head>");
            corpo.Append("<title>SAC - Sistema de Atendimento ao Cliente</title>");
            corpo.Append("<style type=\"text/css\">");
            corpo.Append("body");
            corpo.Append("{");
            corpo.Append("    margin:0px;");
            corpo.Append("    padding:0px;");
            corpo.Append("    font-family: Trebuchet MS, Lucida Sans Unicode, Arial, sans-serif;");
            corpo.Append("    height:100%;");
            corpo.Append("    width:100%;");
            corpo.Append("    font-size:11pt;");
            corpo.Append("}");
            corpo.Append("</style>");
            corpo.Append("</head>");
            corpo.Append("<body>");
            corpo.Append("<table cellspacing=\"4\" cellpadding=\"4\" border=\"0\" style=\"width:700px;\">");
            corpo.Append("<tr><td style=\"text-align: left; vertical-align: middle\"><img src=\"" + logo + "\" alt=\"\" /></td></tr>");
            corpo.Append("<tr><td><br />");
            corpo.Append("Prezado(a) " + nomeUsuario + ",<br /><br />");

            corpo.Append("Confirmamos o recebimento da mensagem a seguir, associada ao chamado de número " + idTicket.ToString() + ". Estaremos enviando resposta em breve.<br /><br />");
            corpo.Append(mensagem + "<br /><br />");

            corpo.Append("Atenciosamente,<br /><br />");
            corpo.Append(ConfigurationManager.AppSettings["Assinatura"] + "<br />");
            corpo.Append("<a href='" + ConfigurationManager.AppSettings["UrlAssinatura"] + "'>" + ConfigurationManager.AppSettings["SiteAssinatura"] + "</a><br />");
            corpo.Append(ConfigurationManager.AppSettings["TelefonesAssinatura"] + "<br />");
            corpo.Append("</td></tr>");
            corpo.Append("</table>");
            corpo.Append("</body>");
            corpo.Append("</html>");

            return corpo.ToString();
        }

        ///// <summary>
        ///// Método que formata o corpo do email 
        ///// enviado para o cliente informando que o
        ///// suporte respondeu a um chamado aberto
        ///// por ele e ele pode acessar link no 
        ///// corpo do email
        ///// </summary>
        ///// <param name="dto">Dto com os dados do chamado</param>
        ///// <param name="nomeUsuario">Nome do usuário</param>
        ///// <param name="tipo">Tipo de email a ser enviado</param>
        ///// <returns>String com o corpo do email formatado</returns>
        //public static string FormatarCorpoEmailRespostaCliente(TicketDTO dto, int idUsuario, string tipoUsuario, string nomeUsuario, string tipo)
        //{
        //    StringBuilder corpo = new StringBuilder();
        //    string url = string.Empty;

        //    if (ConfigurationManager.AppSettings["local"].ToString() == "Des")
        //        url = ConfigurationManager.AppSettings["UrlDes"].ToString();
        //    else
        //        url = ConfigurationManager.AppSettings["UrlProd"].ToString();

        //    string enderecoLogo = RecuperarUrl() + "languages/pt/images/imgTopoEmail.jpg";

        //    try
        //    {
        //        corpo.Append("<html>");
        //        corpo.Append("<head>");
        //        corpo.Append("<title>SAC - Sistema de Atendimento ao Cliente Algorix</title>");
        //        corpo.Append("<style type=\"text/css\">");
        //        corpo.Append("body");
        //        corpo.Append("{");
        //        corpo.Append("    margin:0px;");
        //        corpo.Append("    padding:0px;");
        //        corpo.Append("    font-family: Trebuchet MS, Lucida Sans Unicode, Arial, sans-serif;");
        //        corpo.Append("    height:100%;");
        //        corpo.Append("    width:100%;");
        //        corpo.Append("    font-size:11pt;");
        //        corpo.Append("}");
        //        corpo.Append("</style>");
        //        corpo.Append("</head>");
        //        corpo.Append("<body>");
        //        corpo.Append("<table cellspacing=\"4\" cellpadding=\"4\" border=\"0\" style=\"width:700px;\">");
        //        corpo.Append("<tr><td style=\"text-align: left; vertical-align: middle\"><img src=\"" + enderecoLogo + "\" alt=\"\" /></td></tr>");
        //        corpo.Append("<tr><td><br />");
        //        corpo.Append("Prezado(a) " + nomeUsuario + ",<br /><br />");

        //        if (tipo == "Em Analise")
        //        {
        //            if (tipoUsuario == "S")
        //            {
        //                corpo.Append("Informamos que o chamado de número " + dto.IdTicket.ToString() + " está sendo analisado pelo suporte.<br /><br />");
        //                corpo.Append("<a href=\"" + url + "suporte/Default.aspx?id=" + dto.IdTicket.ToString() + "&iduser=" + idUsuario + "\">Clique aqui</a> para acessar o chamado.<br /><br />");
        //            }
        //        }
        //        else if (tipo == "Resposta")
        //        {
        //            if (tipoUsuario == "S")
        //            {
        //                corpo.Append("Informamos que o chamado de número " + dto.IdTicket.ToString() + " foi respondido pelo Suporte.<br /><br />");
        //                corpo.Append("<a href=\"" + url + "suporte/Default.aspx?id=" + dto.IdTicket.ToString() + "&iduser=" + idUsuario + "\">Clique aqui</a> para acessar o chamado e ver a resposta.<br /><br />");
        //            }
        //            else
        //            {
        //                corpo.Append("Confirmamos o recebimento de nova mensagem associada ao chamado de número " + dto.IdTicket.ToString() + ". Estaremos enviando resposta em breve.<br /><br />");
        //            }
        //        }
        //        else if (tipo == "Lembrete")
        //        {
        //            int diasAviso = Convert.ToInt16(ConfigurationManager.AppSettings["DiasAviso"]);
        //            string link = "<a href='" + url + "suporte/Default.aspx?iduser=" + dto.IdUsuario + "&id=" + dto.IdTicket + "' target='_blank'>Clique aqui para acessar o chamado</a>";

        //            corpo.Append("Informamos que o seu chamado de número " + dto.IdTicket.ToString() + " se encontra pendente de resposta há " + dto.NuDias + " dias.<br /><br />");
        //            corpo.Append("Pedimos por gentileza que conclua o chamado, caso o mesmo já tenha sido solucionado.<br /><br />");
        //            corpo.Append(link + "<br /><br />");
        //        }
        //        else if (tipo == "Concluido")
        //            corpo.Append("Confirmamos a conclusão do seu chamado de número " + dto.IdTicket.ToString() + ".<br /><br />");
        //        else if (tipo == "Cancelado")
        //            corpo.Append("Confirmamos o cancelamento do seu chamado de número " + dto.IdTicket.ToString() + ".<br /><br />");

        //        corpo.Append("Atenciosamente,<br /><br />");
        //        corpo.Append("Algorix Sistemas de Informática<br />");
        //        corpo.Append("<a href=\"http://www.algorix.com\">www.algorix.com</a><br />");
        //        corpo.Append("(21)3139-4056 / (21)2496-3688<br />");
        //        corpo.Append("</td></tr>");
        //        corpo.Append("</table>");
        //        corpo.Append("</body>");
        //        corpo.Append("</html>");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("(FormatarCorpoEmailRespostaCliente) - " + ex.Message);
        //    }

        //    return corpo.ToString();
        //}
    }
}
