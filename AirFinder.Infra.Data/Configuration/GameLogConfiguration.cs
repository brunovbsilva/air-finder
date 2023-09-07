using AirFinder.Domain.GameLogs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace AirFinder.Infra.Data.Configuration
{
    [ExcludeFromCodeCoverage]
    public class GameLogConfiguration : IEntityTypeConfiguration<GameLog>
    {
        public void Configure(EntityTypeBuilder<GameLog> builder)
        {
            builder.ToTable("GameLogs");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("Id").HasColumnType("uniqueidentifier").HasDefaultValueSql("newid()").IsRequired();
            builder.Property(e => e.GameId).HasColumnName("GameId").HasColumnType("uniqueidentifier").IsRequired();
            builder.Property(e => e.UserId).HasColumnName("UserId").HasColumnType("uniqueidentifier").IsRequired();
            builder.Property(e => e.JoinDate).HasColumnName("JoinDate").HasColumnType("bigint").IsRequired();
            builder.Property(e => e.PaymentDate).HasColumnName("PaymentDate").HasColumnType("bigint");
            builder.Property(e => e.ValidateDate).HasColumnName("ValidateDate").HasColumnType("bigint");

            builder.HasOne(e => e.Game)
                .WithMany()
                .HasForeignKey(e => e.GameId);
            builder.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
