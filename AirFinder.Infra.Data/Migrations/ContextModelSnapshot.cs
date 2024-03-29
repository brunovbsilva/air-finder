﻿// <auto-generated />
using System;
using AirFinder.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AirFinder.Infra.Data.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AirFinder.Domain.Battlegrounds.Battleground", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("Id")
                        .HasDefaultValueSql("newid()");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("varchar(150)")
                        .HasColumnName("Address");

                    b.Property<string>("CEP")
                        .IsRequired()
                        .HasColumnType("varchar(8)")
                        .HasColumnName("CEP");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("varchar(30)")
                        .HasColumnName("City");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("varchar(30)")
                        .HasColumnName("Country");

                    b.Property<Guid>("IdCreator")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("IdCreator");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("varchar(max)")
                        .HasColumnName("ImageUrl");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(80)")
                        .HasColumnName("Name");

                    b.Property<int>("Number")
                        .HasColumnType("int")
                        .HasColumnName("Number");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("varchar(30)")
                        .HasColumnName("State");

                    b.HasKey("Id");

                    b.HasIndex("IdCreator");

                    b.ToTable("Battlegrounds", (string)null);
                });

            modelBuilder.Entity("AirFinder.Domain.GameLogs.GameLog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("Id")
                        .HasDefaultValueSql("newid()");

                    b.Property<Guid>("GameId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("GameId");

                    b.Property<long>("JoinDate")
                        .HasColumnType("bigint")
                        .HasColumnName("JoinDate");

                    b.Property<long?>("PaymentDate")
                        .HasColumnType("bigint")
                        .HasColumnName("PaymentDate");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("UserId");

                    b.Property<long?>("ValidateDate")
                        .HasColumnType("bigint")
                        .HasColumnName("ValidateDate");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.HasIndex("UserId");

                    b.ToTable("GameLogs", (string)null);
                });

            modelBuilder.Entity("AirFinder.Domain.Games.Game", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("Id")
                        .HasDefaultValueSql("newid()");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("varchar(max)")
                        .HasColumnName("Description");

                    b.Property<Guid>("IdBattleground")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("IdBattleground");

                    b.Property<Guid>("IdCreator")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("IdCreator");

                    b.Property<int>("MaxPlayers")
                        .HasColumnType("int")
                        .HasColumnName("MaxPlayers");

                    b.Property<long>("MillisDateFrom")
                        .HasColumnType("bigint")
                        .HasColumnName("DateFrom");

                    b.Property<long>("MillisDateUpTo")
                        .HasColumnType("bigint")
                        .HasColumnName("DateUpTo");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(80)")
                        .HasColumnName("Name");

                    b.HasKey("Id");

                    b.HasIndex("IdBattleground");

                    b.HasIndex("IdCreator");

                    b.ToTable("Games", (string)null);
                });

            modelBuilder.Entity("AirFinder.Domain.People.Person", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("Id")
                        .HasDefaultValueSql("newid()");

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

                    b.ToTable("People", (string)null);
                });

            modelBuilder.Entity("AirFinder.Domain.Tokens.TokenControl", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("Id")
                        .HasDefaultValueSql("newid()");

                    b.Property<long?>("ExpirationDate")
                        .HasColumnType("bigint")
                        .HasColumnName("ExpirationDate");

                    b.Property<Guid>("IdUser")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("IdUser");

                    b.Property<long>("SentDate")
                        .HasColumnType("bigint")
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
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("Id")
                        .HasDefaultValueSql("newid()");

                    b.Property<Guid>("IdPerson")
                        .HasColumnType("uniqueidentifier")
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

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("AirFinder.Domain.Battlegrounds.Battleground", b =>
                {
                    b.HasOne("AirFinder.Domain.Users.User", "Creator")
                        .WithMany()
                        .HasForeignKey("IdCreator")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("AirFinder.Domain.GameLogs.GameLog", b =>
                {
                    b.HasOne("AirFinder.Domain.Games.Game", "Game")
                        .WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AirFinder.Domain.Users.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Game");

                    b.Navigation("User");
                });

            modelBuilder.Entity("AirFinder.Domain.Games.Game", b =>
                {
                    b.HasOne("AirFinder.Domain.Battlegrounds.Battleground", "BattleGroud")
                        .WithMany()
                        .HasForeignKey("IdBattleground")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AirFinder.Domain.Users.User", "Creator")
                        .WithMany()
                        .HasForeignKey("IdCreator")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BattleGroud");

                    b.Navigation("Creator");
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
