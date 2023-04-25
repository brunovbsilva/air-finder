﻿using AirFinder.Domain.People;
using AirFinder.Domain.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirFinder.Infra.Data.Configuration
{
    public class TokenControlConfiguration : IEntityTypeConfiguration<TokenControl>
    {
        public void Configure(EntityTypeBuilder<TokenControl> builder)
        {
            builder.ToTable("TokenControl");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("Id").HasColumnType("int").UseIdentityColumn().IsRequired();
            builder.Property(e => e.IdUser).HasColumnName("IdUser").HasColumnType("int").IsRequired();
            builder.Property(e => e.Token).HasColumnName("Token").HasColumnType("varchar(6)").IsRequired();
            builder.Property(e => e.Valid).HasColumnName("Valid").HasColumnType("bit").IsRequired();
            builder.Property(e => e.SentDate).HasColumnName("SentDate").HasColumnType("datetime").IsRequired();
            builder.Property(e => e.ExpirationDate).HasColumnName("ExpirationDate").HasColumnType("datetime");
        }
    }
}