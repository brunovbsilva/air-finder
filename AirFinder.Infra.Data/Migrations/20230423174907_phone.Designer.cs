﻿// <auto-generated />
using System;
using AirFinder.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AirFinder.Infra.Data.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20230423174907_phone")]
    partial class phone
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AirFinder.Domain.People.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Birthday")
                        .HasColumnType("date")
                        .HasColumnName("Birthday");

                    b.Property<string>("CPF")
                        .IsRequired()
                        .HasColumnType("varchar(11)")
                        .HasColumnName("CPF");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(80)")
                        .HasColumnName("Email");

                    b.Property<int>("Gender")
                        .HasColumnType("int")
                        .HasColumnName("Gender");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(80)")
                        .HasColumnName("Name");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("varchar(11)")
                        .HasColumnName("Phone");

                    b.HasKey("Id");

                    b.ToTable("Person", (string)null);
                });

            modelBuilder.Entity("AirFinder.Domain.Tokens.TokenControl", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("ExpirationDate")
                        .HasColumnType("datetime")
                        .HasColumnName("ExpirationDate");

                    b.Property<int>("IdUser")
                        .HasColumnType("int")
                        .HasColumnName("IdUser");

                    b.Property<DateTime>("SentDate")
                        .HasColumnType("datetime")
                        .HasColumnName("SentDate");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("varchar(6)")
                        .HasColumnName("Token");

                    b.Property<bool>("Valid")
                        .HasColumnType("bit")
                        .HasColumnName("Valid");

                    b.HasKey("Id");

                    b.ToTable("TokenControl", (string)null);
                });

            modelBuilder.Entity("AirFinder.Domain.Users.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("IdPerson")
                        .HasColumnType("int")
                        .HasColumnName("IdPerson");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("varchar(20)")
                        .HasColumnName("Login");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("varchar(20)")
                        .HasColumnName("Password");

                    b.Property<int>("Roll")
                        .HasColumnType("int")
                        .HasColumnName("Roll");

                    b.HasKey("Id");

                    b.HasIndex("IdPerson");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("AirFinder.Domain.Users.User", b =>
                {
                    b.HasOne("AirFinder.Domain.People.Person", "Person")
                        .WithMany()
                        .HasForeignKey("IdPerson")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");
                });
#pragma warning restore 612, 618
        }
    }
}
