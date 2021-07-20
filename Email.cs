using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SendGridNet
{
    class Email
    {
        public async Task SendEmail(
            string Toemail,
            List<string> adjuntos,
            string titulo,
            string html,
            Config config
        )
        {
            try
            {
                await Task.Run(() =>
                {
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient(config.Smtp);
                    mail.From = new MailAddress(config.MailRemitente);
                    mail.To.Add(Toemail);
                    mail.Subject = titulo;
                    mail.Body = html;
                    mail.IsBodyHtml = true;
                    
                    /*foreach (var filePath in adjuntos) 
                     * {
                        Attachment attachment = new Attachment(filePath);
                        mail.Attachments.Add(attachment);
                    }*/
                  
                    SmtpServer.Port = 25;
                    SmtpServer.Credentials = new System.Net.NetworkCredential(config.ApiKey, config.ApiKeyPass);
                    SmtpServer.EnableSsl = true;
                    SmtpServer.Send(mail);
                });
            }
            catch (SmtpException ex)
            {
                throw ex;
            }
        }
    }
}
