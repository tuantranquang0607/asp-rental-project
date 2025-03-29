using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EmailService
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration configuration;

        public EmailSender(EmailConfiguration configuration)
        {
            this.configuration = configuration;
        }

        private MailMessage CreateMessage(EmailMessage message)
        {
            MailMessage mail = new MailMessage();

            mail.From = new MailAddress(configuration.From);

            foreach (var recipient in message.To)
            {
                mail.To.Add(recipient);
            }

            mail.Subject = message.Subject;

            mail.Body = message.Content;

            return mail;
        }

        public void Send(EmailMessage message)
        {
            using (SmtpClient client = CreateSmtpClient())
            {
                var mail = CreateMessage(message);

                client.Send(mail);
            }
        }

        private SmtpClient CreateSmtpClient()
        {
            SmtpClient smtpClient = new SmtpClient(configuration.SmtpServer, configuration.Port);

            smtpClient.Credentials = new NetworkCredential(configuration.UserName, configuration.Password);
            smtpClient.EnableSsl = configuration.EnableSSL;

            return smtpClient;
        }
    }
}
