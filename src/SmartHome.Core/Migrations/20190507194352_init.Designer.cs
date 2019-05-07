﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SmartHome.Core.Persistence;

namespace SmartHome.Core.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20190507194352_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<int>("UserId");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("SmartHome.Domain.DictionaryEntity.Dictionary", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("dictionary");
                });

            modelBuilder.Entity("SmartHome.Domain.DictionaryEntity.DictionaryValue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DictionaryId");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(true);

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.HasIndex("DictionaryId");

                    b.ToTable("dictionary_value");
                });

            modelBuilder.Entity("SmartHome.Domain.Entity.AppUserNodeLink", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("NodeId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("NodeId");

                    b.HasIndex("UserId");

                    b.ToTable("appuser_node_link");
                });

            modelBuilder.Entity("SmartHome.Domain.Entity.Command", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ExecutorClassName")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("command");
                });

            modelBuilder.Entity("SmartHome.Domain.Entity.ControlStrategy", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .HasMaxLength(250);

                    b.Property<string>("ExecutorClassNamespace")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<bool>("IsActive");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<byte>("Type")
                        .HasMaxLength(20);

                    b.HasKey("Id");

                    b.ToTable("control_strategy");
                });

            modelBuilder.Entity("SmartHome.Domain.Entity.ControlStrategyCommandLink", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CommandId");

                    b.Property<int>("ControlStrategyId");

                    b.HasKey("Id");

                    b.HasIndex("CommandId");

                    b.HasIndex("ControlStrategyId");

                    b.ToTable("strategy_command_link");
                });

            modelBuilder.Entity("SmartHome.Domain.Entity.Node", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ApiKey")
                        .HasMaxLength(30);

                    b.Property<int>("ControlStrategyId");

                    b.Property<DateTime>("Created");

                    b.Property<int>("CreatedById");

                    b.Property<string>("Description")
                        .HasMaxLength(500);

                    b.Property<string>("GatewayIpAddress")
                        .HasMaxLength(20);

                    b.Property<string>("IpAddress")
                        .HasMaxLength(20);

                    b.Property<string>("Name")
                        .HasMaxLength(50);

                    b.Property<int>("Port");

                    b.HasKey("Id");

                    b.HasIndex("ControlStrategyId");

                    b.HasIndex("CreatedById");

                    b.ToTable("node");
                });

            modelBuilder.Entity("SmartHome.Domain.Role.AppRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("SmartHome.Domain.User.AppUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<int?>("ActivatedById");

                    b.Property<DateTime>("ActivationDate");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("ActivatedById");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("appuser");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("SmartHome.Domain.Role.AppRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("SmartHome.Domain.User.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("SmartHome.Domain.User.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.HasOne("SmartHome.Domain.Role.AppRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SmartHome.Domain.User.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("SmartHome.Domain.User.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SmartHome.Domain.DictionaryEntity.DictionaryValue", b =>
                {
                    b.HasOne("SmartHome.Domain.DictionaryEntity.Dictionary", "Dictionary")
                        .WithMany("Values")
                        .HasForeignKey("DictionaryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SmartHome.Domain.Entity.AppUserNodeLink", b =>
                {
                    b.HasOne("SmartHome.Domain.Entity.Node", "Node")
                        .WithMany("AllowedUsers")
                        .HasForeignKey("NodeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SmartHome.Domain.User.AppUser", "User")
                        .WithMany("EligibleNodes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SmartHome.Domain.Entity.ControlStrategyCommandLink", b =>
                {
                    b.HasOne("SmartHome.Domain.Entity.Command", "Command")
                        .WithMany("Nodes")
                        .HasForeignKey("CommandId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SmartHome.Domain.Entity.ControlStrategy", "ControlStrategy")
                        .WithMany("AllowedCommands")
                        .HasForeignKey("ControlStrategyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SmartHome.Domain.Entity.Node", b =>
                {
                    b.HasOne("SmartHome.Domain.Entity.ControlStrategy", "ControlStrategy")
                        .WithMany()
                        .HasForeignKey("ControlStrategyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SmartHome.Domain.User.AppUser", "CreatedBy")
                        .WithMany("CreatedNodes")
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SmartHome.Domain.User.AppUser", b =>
                {
                    b.HasOne("SmartHome.Domain.User.AppUser", "ActivatedBy")
                        .WithMany()
                        .HasForeignKey("ActivatedById");
                });
#pragma warning restore 612, 618
        }
    }
}