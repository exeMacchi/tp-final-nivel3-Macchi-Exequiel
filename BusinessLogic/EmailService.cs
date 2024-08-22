using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class EmailService
    {
        private MailMessage email;
        private SmtpClient server;

        public EmailService()
        {
            server = new SmtpClient
            {
                Credentials = new NetworkCredential(ConfigurationManager.AppSettings["MAIL_USER"],
                                                    ConfigurationManager.AppSettings["MAIL_PASS"]),
                EnableSsl = true,
                Port = 2525,
                Host = "sandbox.smtp.mailtrap.io"
            };
        }

        /// <summary>
        /// Crear y configurar el correo electrónico que se va a enviar.
        /// </summary>
        /// <param name="sourceEmail">Remitente</param>
        /// <param name="destinationEmail">Destinatario</param>
        /// <param name="subject">Asunto del correo electrónico</param>
        /// <param name="body">Cuerpo HTML del email</param>
        public void CreateMail(string sourceEmail, string destinationEmail, string subject, string body)
        {
            email = new MailMessage(sourceEmail, destinationEmail);
            email.Subject = subject;
            email.IsBodyHtml = true;
            email.Body = body;
        }

        /// <summary>
        /// Enviar el email previamente configurado.
        /// </summary>
        public void SendMail()
        {
            try
            {
                server.Send(email);
            }
            catch (Exception ex)
            {
                throw ex; // El error se propaga hacia arriba
            }
        }
    }
}
