using Microsoft.AspNetCore.Http;

namespace AirFinder.Application.Email.Models.Request
{
    public class MailRequest
    {
        public string ToMail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
}
