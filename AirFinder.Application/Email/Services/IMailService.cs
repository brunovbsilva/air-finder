using AirFinder.Application.Email.Models.Request;

namespace AirFinder.Application.Email.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
