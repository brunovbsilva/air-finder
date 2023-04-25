using AirFinder.Application.Email.Services;
using AirFinder.Application.Users.Services;
using AirFinder.Domain.People;
using AirFinder.Domain.SeedWork.Notification;
using AirFinder.Domain.Tokens;
using AirFinder.Domain.Users;
using AirFinder.Infra.Data;
using AirFinder.Infra.Data.Repository;
using AirFinder.Infra.Utils.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AirFinder.Infra.IoC
{
    public static class NativeInjector
    {
        public static void AddLocalHttpClients(this IServiceCollection services, IConfiguration configuration)
        {

        }

        public static void AddLocalServices(this IServiceCollection services, IConfiguration configuration)
        {
            #region Service
            services.AddScoped<INotification, Notification>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<IUserService, UserService>();
            #endregion

            #region Repository
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITokenRepository, TokenRepository>();
            #endregion
        }

        public static void AddLocalUnitOfWork(this IServiceCollection services, IConfiguration configuration)
        {
            var connString = Builders.BuildConnectionString(configuration);
            services.AddDbContext<Context>(options => options.UseLazyLoadingProxies().UseSqlServer(connString));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
