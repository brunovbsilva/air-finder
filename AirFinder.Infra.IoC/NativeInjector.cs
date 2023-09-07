using AirFinder.Application.Battlegrounds.Services;
using AirFinder.Application.Email.Services;
using AirFinder.Application.Games.Services;
using AirFinder.Application.Imgur.Services;
using AirFinder.Application.Users.Services;
using AirFinder.Domain.Battlegrounds;
using AirFinder.Domain.GameLogs;
using AirFinder.Domain.Games;
using AirFinder.Domain.People;
using AirFinder.Domain.SeedWork.Notification;
using AirFinder.Domain.Tokens;
using AirFinder.Domain.Users;
using AirFinder.Infra.Data;
using AirFinder.Infra.Data.Repository;
using AirFinder.Infra.Security;
using AirFinder.Infra.Utils.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace AirFinder.Infra.IoC
{
    [ExcludeFromCodeCoverage]
    public static class NativeInjector
    {
        public static void AddLocalHttpClients(this IServiceCollection services, IConfiguration configuration)
        {
            var urlImgur = configuration["App:Settings:ImgurUrl"];

            services.AddHttpClient<IImgurService, ImgurService>(c =>
            {
                c.BaseAddress = new Uri(urlImgur!);
                c.Timeout = TimeSpan.FromSeconds(10);
            });
        }

        public static void AddLocalServices(this IServiceCollection services, IConfiguration configuration)
        {
            #region Service
            services.AddScoped<INotification, Notification>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IGameService, GameService>();
            services.AddScoped<IBattlegroundService, BattlegroundService>();
            #endregion

            #region Repository
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddScoped<IGameRepository, GameRepository>();
            services.AddScoped<IBattlegroundRepository, BattlegroundRepository>();
            services.AddScoped<IGameLogRepository, GameLogRepository>();
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
