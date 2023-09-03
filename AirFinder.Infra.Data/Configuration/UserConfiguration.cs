using AirFinder.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirFinder.Infra.Data.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("Id").HasColumnType("uniqueidentifier").HasDefaultValueSql("newid()").IsRequired();
            builder.Property(e => e.Login).HasColumnName("Login").HasColumnType("varchar(20)").IsRequired();
            builder.Property(e => e.Password).HasColumnName("Password").HasColumnType("varchar(20)").IsRequired();
            builder.Property(e => e.IdPerson).HasColumnName("IdPerson").HasColumnType("uniqueidentifier").IsRequired();
            builder.Property(e => e.Role).HasColumnName("Role").HasColumnType("int").IsRequired();

            builder.HasOne(e => e.Person)
                .WithMany()
                .HasForeignKey(e => e.IdPerson);
        }
    }
}
