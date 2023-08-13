using AirFinder.Domain.Battlegrounds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirFinder.Infra.Data.Configuration
{
    public class BattlegroundConfiguration : IEntityTypeConfiguration<Battleground>
    {
        public void Configure(EntityTypeBuilder<Battleground> builder)
        {
            builder.ToTable("Battlegrounds");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("Id").HasColumnType("uniqueidentifier").HasDefaultValueSql("newid()").IsRequired();
            builder.Property(e => e.Name).HasColumnName("Name").HasColumnType("varchar(80)").IsRequired();
            builder.Property(e => e.ImageUrl).HasColumnName("ImageUrl").HasColumnType("varchar(max)").IsRequired();
            builder.Property(e => e.CEP).HasColumnName("CEP").HasColumnType("varchar(8)").IsRequired();
            builder.Property(e => e.Address).HasColumnName("Address").HasColumnType("varchar(150)").IsRequired();
            builder.Property(e => e.Number).HasColumnName("Number").HasColumnType("int").IsRequired();
            builder.Property(e => e.City).HasColumnName("City").HasColumnType("varchar(30)").IsRequired();
            builder.Property(e => e.State).HasColumnName("State").HasColumnType("varchar(30)").IsRequired();
            builder.Property(e => e.Country).HasColumnName("Country").HasColumnType("varchar(30)").IsRequired();
            builder.Property(e => e.IdCreator).HasColumnName("IdCreator").HasColumnType("uniqueidentifier").IsRequired();

            builder.HasOne(e => e.Creator)
                .WithMany()
                .HasForeignKey(e => e.IdCreator)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
