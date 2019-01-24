using System;
using System.Collections.Generic;
using System.Configuration;
using Atendimento.Entities.Entities;
using Atendimento.Infra;

namespace Atendimento.Business.Classes
{
    /// <summary>
    /// Classe com métodos comuns de envio de email, usados por alguns controllers
    /// </summary>
    public static class Emailer
    {
        /// <summary>
        /// Método que envia email de recuperação de senha
        /// </summary>
        /// <param name="userLogin"></param>
        public static void EnviarEmailRecover(UserLogin userLogin)
        {
            var subject = string.Empty;
            var emailFrom = string.Empty;
            var emailTo = string.Empty;
            var emailReply = string.Empty;

            try
            {
                subject = "Senha de acesso ao sistema de atendimento";
                emailFrom = ConfigurationManager.AppSettings["EmailSuporte"].ToString();
                emailTo = userLogin.Email;

                EmailOLD.SendNetEmail(emailFrom, emailTo, emailReply, EmailOLD.FormatarCorpoEmailEsqueciSenha(userLogin), subject, false, System.Net.Mail.MailPriority.High);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Emails para o Cliente

        /// <summary>
        /// Método que envia email para o cliente
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="usuarioCliente"></param>
        public static void EnviarEmailNovoTicketCliente(Ticket ticket, UsuarioCliente usuarioCliente)
        {
            var subject = string.Empty;
            var emailFrom = string.Empty;
            var emailReply = string.Empty;

            try
            {
                subject = "Confirmação de novo atendimento - Ticket #" + ticket.Id.ToString();
                emailFrom = ConfigurationManager.AppSettings["EmailSuporte"].ToString();

                Email.SendNetEmail(emailFrom, usuarioCliente.Email, null, Email.FormatarCorpoNovoChamadoCliente(ticket, usuarioCliente), subject, true, System.Net.Mail.MailPriority.High);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Método que envia email para o cliente
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="statusTicket"></param>
        /// <param name="usuarioCliente"></param>
        public static void EnviarEmailAlteracaoStatusTicketCliente(Ticket ticket, StatusTicket statusTicket, UsuarioCliente usuarioCliente)
        {
            var subject = string.Empty;
            var emailFrom = string.Empty;
            var emailReply = string.Empty;

            try
            {
                subject = "Confirmação de alteração de status - Ticket #" + ticket.Id.ToString();
                emailFrom = ConfigurationManager.AppSettings["EmailSuporte"].ToString();

                Email.SendNetEmail(emailFrom, usuarioCliente.Email, null, Email.FormatarCorpoAlteracaoStatusTicketCliente(ticket, statusTicket, usuarioCliente), subject, true, System.Net.Mail.MailPriority.High);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Método que envia email para o cliente
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="classificacao"></param>
        /// <param name="usuarioCliente"></param>
        public static void EnviarEmailAlteracaoClassificacaoCliente(Ticket ticket, Classificacao classificacao, UsuarioCliente usuarioCliente)
        {
            var subject = string.Empty;
            var emailFrom = string.Empty;
            var emailReply = string.Empty;

            try
            {
                subject = "Confirmação de alteração de classificação - Ticket #" + ticket.Id.ToString();
                emailFrom = ConfigurationManager.AppSettings["EmailSuporte"].ToString();

                Email.SendNetEmail(emailFrom, usuarioCliente.Email, null, Email.FormatarCorpoAlteracaoClassificacaoCliente(ticket, classificacao, usuarioCliente), subject, true, System.Net.Mail.MailPriority.High);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Método que envia email para o cliente, confirmando envio de uma nova mensagem
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="ticketMensagem"></param>
        public static void EnviarEmailNovaMensagemCliente(Ticket ticket, TicketMensagem ticketMensagem)
        {
            var subject = string.Empty;
            var emailFrom = string.Empty;

            try
            {
                subject = "Confirmação de envio de nova mensagem - Ticket #" + ticket.Id.ToString();
                emailFrom = ConfigurationManager.AppSettings["EmailSuporte"].ToString();

                Email.SendNetEmail(emailFrom, ticket.UsuarioCliente.Email, null, Email.FormatarCorpoNovaMensagemCliente(ticket, ticketMensagem), subject, true, System.Net.Mail.MailPriority.High);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Emails para o Suporte

        /// <summary>
        /// Método que envia email para o suporte, informando de novo ticket criado pelo suporte (em nome do cliente)
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="usuarioCliente"></param>
        /// <param name="atendenteEmpresa"></param>
        /// <param name="listaAtendentes"></param>
        public static void EnviarEmailNovoTicketSuporte(Ticket ticket, UsuarioCliente usuarioCliente, AtendenteEmpresa atendenteEmpresa, List<AtendenteEmpresa> listaAtendentes)
        {
            var subject = string.Empty;
            var emailFrom = string.Empty;
            var emailsEmCopia = MontarListaEmailsEmCopia(listaAtendentes);

            try
            {
                subject = "Confirmação de novo atendimento - Ticket #" + ticket.Id.ToString();
                emailFrom = ConfigurationManager.AppSettings["EmailSuporte"].ToString();

                Email.SendNetEmail(emailFrom, atendenteEmpresa.Email, emailsEmCopia, Email.FormatarCorpoNovoChamadoSuporte(ticket, usuarioCliente), subject, true, System.Net.Mail.MailPriority.High);

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Método que envia email para o suporte, informando de novo ticket criado pelo cliente
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="usuarioCliente"></param>
        /// <param name="atendenteEmpresa"></param>
        /// <param name="listaAtendentes"></param>
        public static void EnviarEmailNovoTicketSuporte(Ticket ticket, UsuarioCliente usuarioCliente, List<AtendenteEmpresa> listaAtendentes)
        {
            var subject = string.Empty;
            var emailFrom = string.Empty;
            
            try
            {
                subject = "Confirmação de novo atendimento - Ticket #" + ticket.Id.ToString();
                emailFrom = ConfigurationManager.AppSettings["EmailSuporte"].ToString();

                //Envia email para todos os atendentes
                foreach (var atendente in listaAtendentes)
                {
                    Email.SendNetEmail(emailFrom, atendente.Email, null, Email.FormatarCorpoNovoChamadoSuporte(ticket, usuarioCliente), subject, true, System.Net.Mail.MailPriority.High);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Método que envia email para o suporte, confirmando nova mensagem
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="ticketMensagem"></param>
        /// <param name="usuarioCliente"></param>
        /// <param name="listaAtendentes"></param>
        public static void EnviarEmailNovaMensagemSuporte(Ticket ticket, TicketMensagem ticketMensagem, UsuarioCliente usuarioCliente, List<AtendenteEmpresa> listaAtendentes)
        {
            var subject = string.Empty;
            var emailFrom = string.Empty;
            var emailsEmCopia = MontarListaEmailsEmCopia(listaAtendentes);

            try
            {
                subject = "Confirmação de envio de nova mensagem - Ticket #" + ticket.Id.ToString();
                emailFrom = ConfigurationManager.AppSettings["EmailSuporte"].ToString();

                //Envia email para todos os atendentes
                foreach (var atendente in listaAtendentes)
                {
                    Email.SendNetEmail(emailFrom, atendente.Email, null, Email.FormatarCorpoNovaMensagemSuporte(ticket, usuarioCliente, ticketMensagem), subject, true, System.Net.Mail.MailPriority.High);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Método que envia email para o suporte, informando de nova mensagem interna
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="ticketMensagem"></param>
        /// <param name="atendenteEmpresa"></param>
        /// <param name="listaAtendentes"></param>
        public static void EnviarEmailInterno(Ticket ticket, TicketMensagem ticketMensagem, AtendenteEmpresa atendenteEmpresa, List<AtendenteEmpresa> listaAtendentes)
        {
            var subject = string.Empty;
            var emailFrom = string.Empty;
            var emailsEmCopia = MontarListaEmailsEmCopia(listaAtendentes);

            try
            {
                subject = "Confirmação de novo atendimento interno - Ticket #" + ticket.Id.ToString();
                emailFrom = ConfigurationManager.AppSettings["EmailSuporte"].ToString();

                Email.SendNetEmail(emailFrom, atendenteEmpresa.Email, emailsEmCopia, Email.FormatarCorpoNovaMensagemInterna(ticket, ticketMensagem, atendenteEmpresa), subject, true, System.Net.Mail.MailPriority.High);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        /// <summary>
        /// Método que monta lista de endereços de emails de atendentes da mesma empresa do atendente
        /// que abriu um novo atendimento ou enviou uma nova mensagem, com base no flag dele, indicando
        /// se é pra enviar emails em copia ou não. 
        /// Se for pra enviar emails em copia, coleta todos os emails dos atendentes da mesma empresa.
        /// </summary>
        /// <param name="listaAtendentes"></param>
        /// <returns></returns>
        private static List<string> MontarListaEmailsEmCopia(List<AtendenteEmpresa> listaAtendentes)
        {
            List<string> emailsEmCopia = null;

            //Verifica se é preciso enviar o email em copia
            if (listaAtendentes != null)
            {
                emailsEmCopia = new List<string>();

                foreach (var atendente in listaAtendentes)
                {
                    emailsEmCopia.Add(atendente.Email);
                }
            }

            return emailsEmCopia;
        }
    }
}