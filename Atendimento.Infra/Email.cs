using System;
using System.Collections.Generic;
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
    public sealed class Email : BaseUtil
    {
        #region Rotinas de envio de email

        /// <summary>
        /// Método de envio de email no padrão anterior, usado para poder
        /// se compativel com o servidor smtp do radar55
        /// </summary>
        /// <param name="emailFrom"></param>
        /// <param name="emailTo"></param>
        /// <param name="emailsEmCopia"></param>
        /// <param name="body"></param>
        /// <param name="subject"></param>
        /// <param name="FormatoHTML"></param>
        /// <param name="prioridade"></param>
        public static void SendNetEmail(string emailFrom, string emailTo, List<string> emailsEmCopia, string body, string subject, bool FormatoHTML, MailPriority prioridade)
        {
            try
            {
                var mensagem = new MailMessage();

                if (!IsValid(emailFrom, "leve")) { return; }
                if (!IsValid(emailTo, "leve")) { return; }

                mensagem.From = new MailAddress(emailFrom);
                mensagem.To.Add(new MailAddress(emailTo));

                if (emailsEmCopia != null)
                {
                    foreach (var email in emailsEmCopia)
                    {
                        if (!IsValid(email, "leve")) { return; }
                        mensagem.CC.Add(new MailAddress(email));
                    }
                }

                mensagem.ReplyToList.Add(new MailAddress("no-replay@" + ConfigurationManager.AppSettings["DominioEmailEmpresa"].ToString().ToLower()));

                //======================================================

                mensagem.Body = body;
                mensagem.Subject = subject;
                mensagem.IsBodyHtml = FormatoHTML;
                mensagem.SubjectEncoding = Encoding.UTF8;
                mensagem.BodyEncoding = Encoding.UTF8;
                mensagem.Priority = prioridade;

                if (ConfigurationManager.AppSettings["local"] == "Des")
                {
                    using (var cliente = new SmtpClient(ConfigurationManager.AppSettings["SMTP"].ToString()))
                    {
                        var credenciais = new NetworkCredential(ConfigurationManager.AppSettings["EmailSuporte"].ToString(), ConfigurationManager.AppSettings["Password"].ToString());
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
                }
                else
                {
                    using (var smtp = new SmtpClient())
                    {
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.Port = 25;
                        smtp.Host = ConfigurationManager.AppSettings["SMTP"].ToString();
                        smtp.Send(mensagem);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Rotinas de formatação de corpo de email

        #region Corpo Email Esqueci Senha

        /// <summary>
        /// Método que formata o corpo do email de esqueci a senha
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        public static string FormatarCorpoEmailEsqueciSenha(UserLogin userLogin)
        {
            var corpo = new StringBuilder();

            var logo = RecuperarUrl() + "Images/logo-text.png";

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
            corpo.Append(ConfigurationManager.AppSettings["TelefonesAssinatura"] + "<br />");
            corpo.Append("</td></tr>");
            corpo.Append("</table>");
            corpo.Append("</body>");
            corpo.Append("</html>");

            return corpo.ToString();
        }

        #endregion

        #region Corpo Email Novo Atendimento - Ticket

        /// <summary>
        /// Método que formata o corpo do email
        /// enviado para o cliente com os dados
        /// do atendimento criando pelo suporte
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="usuarioCliente"></param>
        /// <returns></returns>
        public static string FormatarCorpoNovoChamadoCliente(Ticket ticket, UsuarioCliente usuarioCliente)
        {
            var corpo = new StringBuilder();

            var logo = ConfigurationManager.AppSettings["LogoCabecalho"];

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
            corpo.Append("Prezado(a) " + usuarioCliente.Nome + ",<br /><br />");

            corpo.Append("Confirmada a criação do novo atendimento - Ticket " + ticket.Titulo + " - (#" + ticket.Id + "). Estaremos respondendo em breve.<br /><br />");

            corpo.Append("Atenciosamente,<br />");
            corpo.Append(ConfigurationManager.AppSettings["TextoAssinatura"] + "<br />");
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
        /// do atendimento aberto pelo cliente
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="usuarioCliente"></param>
        /// <returns></returns>
        public static string FormatarCorpoNovoChamadoSuporte(Ticket ticket, UsuarioCliente usuarioCliente)
        {
            var corpo = new StringBuilder();

            var logo = ConfigurationManager.AppSettings["LogoCabecalho"];

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
            corpo.Append("Prezado Suporte,<br /><br />");

            corpo.Append("Confirmado o recebimento de novo atendimento - Ticket " + ticket.Titulo + " - (#" + ticket.Id + "), associado ao usuário " + usuarioCliente.Nome + ".<br /><br />");
            corpo.Append("Acesse o <a href='" + ConfigurationManager.AppSettings["SistemaAtendimentoAssinatura"] + "'>sistema de atendimento</a> para dar prosseguimento ao suporte.<br /><br />");

            corpo.Append("Atenciosamente,<br />");
            corpo.Append(ConfigurationManager.AppSettings["TextoAssinatura"] + "<br />");
            corpo.Append("<a href='" + ConfigurationManager.AppSettings["UrlAssinatura"] + "'>" + ConfigurationManager.AppSettings["SiteAssinatura"] + "</a><br />");
            corpo.Append(ConfigurationManager.AppSettings["TelefonesAssinatura"] + "<br />");
            corpo.Append("</td></tr>");
            corpo.Append("</table>");
            corpo.Append("</body>");
            corpo.Append("</html>");

            return corpo.ToString();
        }

        #endregion

        #region Corpo Email Alteração Status Ticket

        /// <summary>
        /// Método que formata o corpo do email enviado ao cliente
        /// para alterações do status do atendimento (ticket)
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="statusTicket"></param>
        /// <param name="usuarioCliente"></param>
        /// <returns></returns>
        public static string FormatarCorpoAlteracaoStatusTicketCliente(Ticket ticket, StatusTicket statusTicket, UsuarioCliente usuarioCliente)
        {
            var corpo = new StringBuilder();

            var logo = ConfigurationManager.AppSettings["LogoCabecalho"];

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
            corpo.Append("Prezado(a) " + usuarioCliente.Nome + ",<br /><br />");

            corpo.Append("Confirmada a alteração do status do atendimento - Ticket " + ticket.Titulo + " - (#" + ticket.Id + "), passando para o status " + statusTicket.Nome + ".<br /><br />");
            corpo.Append("Acesse o <a href='" + ConfigurationManager.AppSettings["SistemaAtendimentoAssinatura"] + "'>sistema de atendimento</a> para obter maiores informações.<br /><br />");

            corpo.Append("Atenciosamente,<br />");
            corpo.Append(ConfigurationManager.AppSettings["TextoAssinatura"] + "<br />");
            corpo.Append("<a href='" + ConfigurationManager.AppSettings["UrlAssinatura"] + "'>" + ConfigurationManager.AppSettings["SiteAssinatura"] + "</a><br />");
            corpo.Append(ConfigurationManager.AppSettings["TelefonesAssinatura"] + "<br />");
            corpo.Append("</td></tr>");
            corpo.Append("</table>");
            corpo.Append("</body>");
            corpo.Append("</html>");

            return corpo.ToString();
        }

        #endregion

        #region Corpo Email Alteração Classificacao

        /// <summary>
        /// Método que formata o corpo do email enviado ao cliente
        /// para alterações da classificação do atendimento (ticket)
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="classificacao"></param>
        /// <param name="usuarioCliente"></param>
        /// <returns></returns>
        public static string FormatarCorpoAlteracaoClassificacaoCliente(Ticket ticket, Classificacao classificacao, UsuarioCliente usuarioCliente)
        {
            var corpo = new StringBuilder();

            var logo = ConfigurationManager.AppSettings["LogoCabecalho"];

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
            corpo.Append("Prezado(a) " + usuarioCliente.Nome + ",<br /><br />");

            corpo.Append("Confirmada a alteração da classificação do atendimento - Ticket " + ticket.Titulo + " - (#" + ticket.Id + "), passando para " + classificacao.Nome + ".<br /><br />");
            corpo.Append("Acesse o <a href='" + ConfigurationManager.AppSettings["SistemaAtendimentoAssinatura"] + "'>sistema de atendimento</a> para obter maiores informações.<br /><br />");

            corpo.Append("Atenciosamente,<br />");
            corpo.Append(ConfigurationManager.AppSettings["TextoAssinatura"] + "<br />");
            corpo.Append("<a href='" + ConfigurationManager.AppSettings["UrlAssinatura"] + "'>" + ConfigurationManager.AppSettings["SiteAssinatura"] + "</a><br />");
            corpo.Append(ConfigurationManager.AppSettings["TelefonesAssinatura"] + "<br />");
            corpo.Append("</td></tr>");
            corpo.Append("</table>");
            corpo.Append("</body>");
            corpo.Append("</html>");

            return corpo.ToString();
        }

        #endregion

        #region Corpo Email Interno

        /// <summary>
        /// Método que formata o corpo do email de aviso de nova mensagem interna,
        /// enviado apenas para o atendimento (suporte).
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="ticketMensagem"></param>
        /// <param name="atendenteEmpresa"></param>
        /// <returns></returns>
        public static string FormatarCorpoNovaMensagemInterna(Ticket ticket, TicketMensagem ticketMensagem, AtendenteEmpresa atendenteEmpresa)
        {
            var corpo = new StringBuilder();

            var logo = ConfigurationManager.AppSettings["LogoCabecalho"];

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
            corpo.Append("Prezado Suporte,<br /><br />");

            corpo.Append("Confirmado o recebimento de nova mensagem interna de atendimento - Ticket " + ticket.Titulo + " - (#" + ticket.Id + "), criado pelo atendente " + atendenteEmpresa.Nome + ".<br /><br />");

            corpo.Append("</td></tr>");
            corpo.Append("<tr><td><br />");

            corpo.Append("<h3>Ticket</h3><hr><br />");

            corpo.Append("<table style=\"width: 70%;\" cellpadding=\"4\" cellspacing=\"4\">");
            corpo.Append("    <tr><td style=\"width:20%; vertical-align:top\"><strong>Usuário</strong></td>");
            corpo.Append("        <td style=\"width:80%\">" + ticket.UsuarioCliente.Nome + " (" + ticket.UsuarioCliente.Email + ")</td></tr>");
            corpo.Append("    <tr><td style=\"width:20%; vertical-align:top\"><strong>Classificação</strong></td>");
            corpo.Append("        <td>" + ticket.Classificacao.Nome + "</td></tr>");
            corpo.Append("    <tr><td style=\"width:20%; vertical-align:top\"><strong>Categoria</strong></td>");
            corpo.Append("        <td>" + ticket.Categoria.Nome + "</td></tr>");
            corpo.Append("    <tr><td style=\"width:20%; vertical-align:top\"><strong>Título</strong></td>");
            corpo.Append("        <td>" + ticket.Titulo + "</td></tr>");
            corpo.Append("    <tr><td style=\"width:20%; vertical-align:top\"><strong>Descrição</strong></td>");
            corpo.Append("        <td>" + ticket.Descricao.Replace("\n", "<br />").Replace("\r", "") + "</td></tr>");
            corpo.Append("</table>");

            corpo.Append("</td></tr>");
            corpo.Append("<tr><td><br />");

            corpo.Append("<h3>Nova Mensagem</h3><hr><br />");
            corpo.Append("<p>" + ticketMensagem.Descricao.Replace("\n", "<br />").Replace("\r", "") + "</p><br /><br />");

            corpo.Append("Acesse o <a href='" + ConfigurationManager.AppSettings["SistemaAtendimentoAssinatura"] + "'>sistema de atendimento</a> para dar prosseguimento ao suporte.<br /><br />");

            corpo.Append("</td></tr>");
            corpo.Append("<tr><td><br />");

            corpo.Append("Atenciosamente,<br />");
            corpo.Append(ConfigurationManager.AppSettings["TextoAssinatura"] + "<br />");
            corpo.Append("<a href='" + ConfigurationManager.AppSettings["UrlAssinatura"] + "'>" + ConfigurationManager.AppSettings["SiteAssinatura"] + "</a><br />");
            corpo.Append(ConfigurationManager.AppSettings["TelefonesAssinatura"] + "<br />");
            corpo.Append("</td></tr>");
            corpo.Append("</table>");
            corpo.Append("</body>");
            corpo.Append("</html>");

            return corpo.ToString();
        }

        #endregion

        #region Corpo Email Nova Mensagem Cliente

        /// <summary>
        /// Método que formata o corpo do email
        /// enviado para o suporte com os dados
        /// do atendimento solicitado no site Algorix
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="ticketMensagem"></param>
        /// <returns></returns>
        public static string FormatarCorpoNovaMensagemCliente(Ticket ticket, TicketMensagem ticketMensagem)
        {
            var corpo = new StringBuilder();

            var logo = ConfigurationManager.AppSettings["LogoCabecalho"];

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
            corpo.Append("Prezado(a) " + ticket.UsuarioCliente.Nome + ",<br /><br />");

            corpo.Append("Confirmado o recebimento de nova mensagem de atendimento - Ticket " + ticket.Titulo + " - (#" + ticket.Id + "). Estaremos respondendo em breve.<br /><br />");

            corpo.Append("</td></tr>");
            corpo.Append("<tr><td><br />");

            corpo.Append("<h3>Ticket</h3><hr><br />");

            corpo.Append("<table style=\"width: 70%;\" cellpadding=\"4\" cellspacing=\"4\">");
            corpo.Append("    <tr><td style=\"width:20%; vertical-align:top\"><strong>Classificação</strong></td>");
            corpo.Append("        <td>" + ticket.Classificacao.Nome + "</td></tr>");
            corpo.Append("    <tr><td style=\"width:20%; vertical-align:top\"><strong>Categoria</strong></td>");
            corpo.Append("        <td>" + ticket.Categoria.Nome + "</td></tr>");
            corpo.Append("    <tr><td style=\"width:20%; vertical-align:top\"><strong>Título</strong></td>");
            corpo.Append("        <td>" + ticket.Titulo + "</td></tr>");
            corpo.Append("    <tr><td style=\"width:20%; vertical-align:top\"><strong>Descrição</strong></td>");
            corpo.Append("        <td>" + ticket.Descricao.Replace("\n", "<br />").Replace("\r", "") + "</td></tr>");
            corpo.Append("</table>");

            corpo.Append("</td></tr>");
            corpo.Append("<tr><td><br />");

            corpo.Append("<h3>Nova Mensagem</h3><hr><br />");
            corpo.Append("<p>" + ticketMensagem.Descricao.Replace("\n", "<br />").Replace("\r", "") + "</p><br /><br />");

            corpo.Append("Acesse o <a href='" + ConfigurationManager.AppSettings["SistemaAtendimentoAssinatura"] + "'>sistema de atendimento</a> para dar prosseguimento ao suporte.<br /><br />");

            corpo.Append("</td></tr>");
            corpo.Append("<tr><td><br />");

            corpo.Append("Atenciosamente,<br />");
            corpo.Append(ConfigurationManager.AppSettings["TextoAssinatura"] + "<br />");
            corpo.Append("<a href='" + ConfigurationManager.AppSettings["UrlAssinatura"] + "'>" + ConfigurationManager.AppSettings["SiteAssinatura"] + "</a><br />");
            corpo.Append(ConfigurationManager.AppSettings["TelefonesAssinatura"] + "<br />");
            corpo.Append("</td></tr>");
            corpo.Append("</table>");
            corpo.Append("</body>");
            corpo.Append("</html>");

            return corpo.ToString();
        }

        #endregion

        #region Corpo Email Nova Mensagem Suporte

        /// <summary>
        /// Método que formata o corpo do email
        /// enviado para o suporte com os dados
        /// do atendimento solicitado no site Algorix
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="usuarioCliente"></param>
        /// <param name="ticketMensagem"></param>
        /// <returns></returns>
        public static string FormatarCorpoNovaMensagemSuporte(Ticket ticket, UsuarioCliente usuarioCliente, TicketMensagem ticketMensagem)
        {
            var corpo = new StringBuilder();

            var logo = ConfigurationManager.AppSettings["LogoCabecalho"];

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
            corpo.Append("Prezado(a) Suporte,<br /><br />");

            corpo.Append("Confirmado o recebimento de nova mensagem de atendimento - Ticket " + ticket.Titulo + " - (#" + ticket.Id + "), criado pelo cliente " + usuarioCliente.Nome + ".<br /><br />");

            corpo.Append("</td></tr>");
            corpo.Append("<tr><td><br />");

            corpo.Append("<h3>Ticket</h3><hr><br />");

            corpo.Append("<table style=\"width: 70%;\" cellpadding=\"4\" cellspacing=\"4\">");
            corpo.Append("    <tr><td style=\"width:20%; vertical-align:top\"><strong>Usuário</strong></td>");
            corpo.Append("        <td style=\"width:80%\">" + ticket.UsuarioCliente.Nome + " (" + ticket.UsuarioCliente.Email + ")</td></tr>");
            corpo.Append("    <tr><td style=\"width:20%; vertical-align:top\"><strong>Classificação</strong></td>");
            corpo.Append("        <td>" + ticket.Classificacao.Nome + "</td></tr>");
            corpo.Append("    <tr><td style=\"width:20%; vertical-align:top\"><strong>Categoria</strong></td>");
            corpo.Append("        <td>" + ticket.Categoria.Nome + "</td></tr>");
            corpo.Append("    <tr><td style=\"width:20%; vertical-align:top\"><strong>Título</strong></td>");
            corpo.Append("        <td>" + ticket.Titulo + "</td></tr>");
            corpo.Append("    <tr><td style=\"width:20%; vertical-align:top\"><strong>Descrição</strong></td>");
            corpo.Append("        <td>" + ticket.Descricao.Replace("\n", "<br />").Replace("\r", "") + "</td></tr>");
            corpo.Append("</table>");

            corpo.Append("</td></tr>");
            corpo.Append("<tr><td><br />");

            corpo.Append("<h3>Nova Mensagem</h3><hr><br />");
            corpo.Append("<p>" + ticketMensagem.Descricao.Replace("\n", "<br />").Replace("\r", "") + "</p><br /><br />");

            corpo.Append("Acesse o <a href='" + ConfigurationManager.AppSettings["SistemaAtendimentoAssinatura"] + "'>sistema de atendimento</a> para dar prosseguimento ao suporte.<br /><br />");

            corpo.Append("</td></tr>");
            corpo.Append("<tr><td><br />");

            corpo.Append("Atenciosamente,<br />");
            corpo.Append(ConfigurationManager.AppSettings["TextoAssinatura"] + "<br />");
            corpo.Append("<a href='" + ConfigurationManager.AppSettings["UrlAssinatura"] + "'>" + ConfigurationManager.AppSettings["SiteAssinatura"] + "</a><br />");
            corpo.Append(ConfigurationManager.AppSettings["TelefonesAssinatura"] + "<br />");
            corpo.Append("</td></tr>");
            corpo.Append("</table>");
            corpo.Append("</body>");
            corpo.Append("</html>");

            return corpo.ToString();
        }

        #endregion

        #endregion

        #region Rotina de validação de email

        /// <summary>
        /// Método que verifica se um endereço email é valido
        /// </summary>
        /// <param name="email">Enderço email</param>
        /// <param name="tipo">Se é um testo do tipo Leniente (leve) ou Restrito (pesado). Enviar os valores "leve" ou "pesado" para esse parametro</param>
        /// <returns>Status do email</returns>
        public static bool IsValid(string email, string tipo)
        {
            var pattern = "";

            pattern = tipo == "leve" ? @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" : tipo == "pesado" ? @"^(([^<>()[\]\\.,;:\s@\""]+"
                  + @"(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@"
                  + @"((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}"
                  + @"\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+"
                  + @"[a-zA-Z]{2,}))$" : @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";

            var reStrict = new Regex(pattern);
            var isStrictMatch = reStrict.IsMatch(email);

            return isStrictMatch;
        }

        #endregion
    }
}
