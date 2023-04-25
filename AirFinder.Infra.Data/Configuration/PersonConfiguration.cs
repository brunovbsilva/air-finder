using AirFinder.Domain.People;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirFinder.Infra.Data.Configuration
{
    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.ToTable("Person");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("Id").HasColumnType("int").UseIdentityColumn().IsRequired();
            builder.Property(e => e.Name).HasColumnName("Name").HasColumnType("varchar(80)").IsRequired();
            builder.Property(e => e.Email).HasColumnName("Email").HasColumnType("varchar(80)").IsRequired();
            builder.Property(e => e.Birthday).HasColumnName("Birthday").HasColumnType("date").IsRequired();
            builder.Property(e => e.CPF).HasColumnName("CPF").HasColumnType("varchar(11)").IsRequired();
            builder.Property(e => e.Gender).HasColumnName("Gender").HasColumnType("int").IsRequired();
            builder.Property(e => e.Phone).HasColumnName("Phone").HasColumnType("varchar(11)").IsRequired();
        }
    }
}
