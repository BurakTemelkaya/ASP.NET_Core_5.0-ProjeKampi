using System;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace CoreLayer.Utilities.MailUtilities
{
    public class MailManager : IMailService
    {
        readonly IConfiguration _configuration;

        public MailManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool SendMail(string mail, string subject, string message)
        {          
            try
            {
                SmtpClient smtp = new();
                var senderMail = _configuration["MailInfo:Mail"];
                var senderPassword = _configuration["MailInfo:Password"];
                smtp.Credentials = new System.Net.NetworkCredential(senderMail, senderPassword);
                smtp.Port = int.Parse(_configuration["MailInfo:Port"]);
                smtp.Host = _configuration["MailInfo:Host"];
                smtp.EnableSsl = Convert.ToBoolean(_configuration["MailInfo:SSL"]);

                MailMessage eMail = new();
                eMail.From = new MailAddress(_configuration["MailInfo:Mail"]);
                eMail.To.Add(mail);
                eMail.Subject = subject;
                eMail.IsBodyHtml = true;
                eMail.Body = message;

                smtp.SendAsync(eMail, (object)eMail);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool SendMails(string[] mail, string subject, string message)
        {
            for (int i = 0; i < mail.Length; i++)
            {
                try
                {
                    SendMail(mail[i], subject, message);
                }
                catch
                {
                    
                }
            }
            return true;
        }
       
    }
}
