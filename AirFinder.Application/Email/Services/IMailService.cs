using AirFinder.Domain.Email.Models.Requests;

namespace AirFinder.Application.Email.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
