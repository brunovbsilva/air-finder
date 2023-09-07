using AirFinder.Infra.Data.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace AirFinder.Infra.Data
{
    [ExcludeFromCodeCoverage]
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PersonConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TokenControlConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BattlegroundConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(GameConfiguration).Assembly);
        }
    }
}
