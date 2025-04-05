using Company.G03.PL.Sittings;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net.Mail;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Company.G03.PL.Helpers
{
    public class MailService(IOptions<MailSettings> _options) : IMailservice
    {
      

        public void SendEmail(Email email)
        {
            //Build the Message 
            var mail = new MimeMessage();
            mail.Subject = email.Subject;   
            mail.From.Add( new MailboxAddress(_options.Value.DisplayName, _options.Value.Email));
            mail.To.Add(MailboxAddress.Parse(email.To));

            var builder = new BodyBuilder();

            builder.TextBody = email.Body;
            mail.Body = builder.ToMessageBody();
            // Establish connection with the mail server
            using var smtp = new SmtpClient();
            smtp.Connect(_options.Value.Host, _options.Value.Port ,MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(_options.Value.Email, _options.Value.Password);
           //Send the message 

            smtp.Send(mail);
        }
    }
}
