using AirFinder.Infra.Data.Configuration;
using Microsoft.EntityFrameworkCore;

namespace AirFinder.Infra.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PersonConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TokenControlConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BattleGroundConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(GameConfiguration).Assembly);
        }
    }
}
