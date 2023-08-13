using Microsoft.AspNetCore.Http;

namespace AirFinder.Domain.Email.Models.Requests
{
    public class MailRequest
    {
        public string ToMail { get; set; } = String.Empty;
        public string Subject { get; set; } = String.Empty;
        public string Body { get; set; } = String.Empty;
        public List<IFormFile>? Attachments { get; set; }
    }
}
