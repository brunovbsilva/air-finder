using AirFinder.Domain.GameLogs;
using AirFinder.Domain.Games;
using AirFinder.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirFinder.Infra.Data.Configuration
{
    public class GameLogConfiguration : IEntityTypeConfiguration<GameLog>
    {
        public void Configure(EntityTypeBuilder<GameLog> builder)
        {
            builder.ToTable("GameLogs");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("Id").HasColumnType("uniqueidentifier").HasDefaultValueSql("newid()").IsRequired();
            builder.Property(e => e.GameId).HasColumnName("GameId").HasColumnType("uniqueidentifier").IsRequired();
            builder.Property(e => e.UserId).HasColumnName("UserId").HasColumnType("uniqueidentifier").IsRequired();
            builder.Property(e => e.CreationDate).HasColumnName("CreationDate").HasColumnType("bigint").IsRequired();
            builder.Property(e => e.LastUpdateDate).HasColumnName("LastUpdateDate").HasColumnType("bigint");
            builder.Property(e => e.LastUpdateUserId).HasColumnName("LastUpdateUserId").HasColumnType("uniqueidentifier");
            builder.Property(e => e.Status).HasColumnName("Status").HasColumnType("int").IsRequired();
            builder.Property(e => e.Description).HasColumnName("Description").HasColumnType("varchar(80)").IsRequired();

            builder.HasOne(e => e.Game)
                .WithMany()
                .HasForeignKey(e => e.GameId);
            builder.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(e => e.LastUpdateUser)
                .WithMany()
                .HasForeignKey(e => e.LastUpdateUserId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
