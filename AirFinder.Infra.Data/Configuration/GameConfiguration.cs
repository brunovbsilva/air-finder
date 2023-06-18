using AirFinder.Domain.BattleGrounds;
using AirFinder.Domain.Games;
using AirFinder.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirFinder.Infra.Data.Configuration
{
    public class GameConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder.ToTable("Games");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("Id").HasColumnType("uniqueidentifier").HasDefaultValueSql("newid()").IsRequired();
            builder.Property(e => e.Name).HasColumnName("Name").HasColumnType("varchar(80)").IsRequired();
            builder.Property(e => e.Description).HasColumnName("Description").HasColumnType("varchar(max)").IsRequired();
            builder.Property(e => e.MillisDate).HasColumnName("Date").HasColumnType("bigint").IsRequired();
            builder.Property(e => e.IdBattleGround).HasColumnName("IdBattleGround").HasColumnType("uniqueidentifier").IsRequired();
            builder.Property(e => e.IdCreator).HasColumnName("IdCreator").HasColumnType("uniqueidentifier").IsRequired();
            builder.Property(e => e.Status).HasColumnName("Status").HasColumnType("int").IsRequired();

            builder.HasOne(e => e.BattleGroud)
                .WithMany()
                .HasForeignKey(e => e.IdBattleGround);
            builder.HasOne(e => e.Creator)
                .WithMany()
                .HasForeignKey(e => e.IdCreator);
        }
    }
}
