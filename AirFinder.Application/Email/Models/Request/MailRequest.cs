using Microsoft.AspNetCore.Http;

namespace AirFinder.Application.Email.Models.Request
{
    public class MailRequest
    {
        public string ToMail { get; set; } = String.Empty;
        public string Subject { get; set; } = String.Empty;
        public string Body { get; set; } = String.Empty;
        public List<IFormFile>? Attachments { get; set; }
    }
}
