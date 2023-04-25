using AirFinder.Application.Common;
using AirFinder.Application.Email.Models.Request;
using AirFinder.Domain.SeedWork.Notification;
using AirFinder.Infra.Utils.Configuration;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace AirFinder.Application.Email.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        public MailService(IOptionsSnapshot<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToMail));
            email.Subject = mailRequest.Subject;
            var builder = new BodyBuilder();
            if(mailRequest.Attachments != null) 
            {
                byte[] filebytes;
                foreach(var file in mailRequest.Attachments)
                {
                    if(file.Length > 0)
                    {
                        using(var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            filebytes = ms.ToArray();
                        }
                        builder.Attachments.Add(file.Name, filebytes, ContentType.Parse(file.ContentType));
                    }
                }
            }
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(
                _mailSettings.Host,
                _mailSettings.Port,
                SecureSocketOptions.StartTls
            );
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
