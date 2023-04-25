using AirFinder.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirFinder.Infra.Data.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("Id").HasColumnType("int").UseIdentityColumn().IsRequired();
            builder.Property(e => e.Login).HasColumnName("Login").HasColumnType("varchar(20)").IsRequired();
            builder.Property(e => e.Password).HasColumnName("Password").HasColumnType("varchar(20)").IsRequired();
            builder.Property(e => e.IdPerson).HasColumnName("IdPerson").HasColumnType("int").IsRequired();
            builder.Property(e => e.Roll).HasColumnName("Roll").HasColumnType("int").IsRequired();

            builder.HasOne(e => e.Person)
                .WithMany()
                .HasForeignKey(e => e.IdPerson);
        }
    }
}
