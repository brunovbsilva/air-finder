using AirFinder.Application.Email.Services;
using System.Globalization;

namespace AirFinder.Application.Common
{
    public abstract class BaseService
    {
        protected readonly IMailService _mailService;
        public BaseService(IMailService mailService) 
        {
            _mailService = mailService;
        }

        public async Task<T?> ExecuteAsync<T>(Func<Task<T>> action)
        {
            try
            {
                return await action();
            }
            catch (Exception ex)
            {
                return default;
            }
            return default;
        }
        
    }
}
