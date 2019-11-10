﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SmartHome.Core.Data;

namespace SmartHome.Core.Data.Migrations
{
    [DbContext(typeof(EntityFrameworkContext))]
    [Migration("20191110092430_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(95)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("varchar(95)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("longtext");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(95)");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(95)");

                    b.Property<string>("Value")
                        .HasColumnType("longtext");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("SmartHome.Core.Entities.DictionaryEntity.Dictionary", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<string>("Metadata")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<bool>("ReadOnly")
                        .HasColumnType("bit");

                    b.Property<byte[]>("RowVersion")
                        .HasColumnType("longblob");

                    b.HasKey("Id");

                    b.ToTable("tbl_dictionary");
                });

            modelBuilder.Entity("SmartHome.Core.Entities.DictionaryEntity.DictionaryValue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("DictionaryId")
                        .HasColumnType("int");

                    b.Property<string>("DisplayValue")
                        .HasColumnType("longtext");

                    b.Property<string>("InternalValue")
                        .HasColumnType("longtext");

                    b.Property<bool?>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<string>("Metadata")
                        .HasColumnType("longtext");

                    b.Property<byte[]>("RowVersion")
                        .HasColumnType("longblob");

                    b.HasKey("Id");

                    b.HasIndex("DictionaryId");

                    b.ToTable("tbl_dictionary_value");
                });

            modelBuilder.Entity("SmartHome.Core.Entities.Entity.AppUserNodeLink", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("NodeId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("NodeId");

                    b.HasIndex("UserId");

                    b.ToTable("tbl_user_node_link");
                });

            modelBuilder.Entity("SmartHome.Core.Entities.Entity.ControlStrategy", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Connector")
                        .IsRequired()
                        .HasColumnType("varchar(250)")
                        .HasMaxLength(250);

                    b.Property<string>("ContractAssembly")
                        .IsRequired()
                        .HasColumnType("varchar(250)")
                        .HasMaxLength(250);

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("CreatedById")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("varchar(250)")
                        .HasMaxLength(250);

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(250)")
                        .HasMaxLength(250);

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("UpdatedById")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("UpdatedById");

                    b.ToTable("tbl_control_strategy");
                });

            modelBuilder.Entity("SmartHome.Core.Entities.Entity.Node", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ApiKey")
                        .HasColumnType("varchar(30)")
                        .HasMaxLength(30);

                    b.Property<string>("BaseTopic")
                        .HasColumnType("varchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("ClientId")
                        .HasColumnType("varchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("ConfigMetadata")
                        .HasColumnType("longtext")
                        .HasMaxLength(2147483647);

                    b.Property<int>("ControlStrategyId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("CreatedById")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("varchar(500)")
                        .HasMaxLength(500);

                    b.Property<string>("GatewayIpAddress")
                        .HasColumnType("varchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("IpAddress")
                        .HasColumnType("varchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("Login")
                        .HasColumnType("varchar(40)")
                        .HasMaxLength(40);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Password")
                        .HasColumnType("varchar(40)")
                        .HasMaxLength(40);

                    b.Property<int>("Port")
                        .HasColumnType("int");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .HasColumnType("longblob");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("UpdatedById")
                        .HasColumnType("int");

                    b.Property<string>("UriSchema")
                        .HasColumnType("varchar(10)")
                        .HasMaxLength(10);

                    b.HasKey("Id");

                    b.HasIndex("ClientId")
                        .IsUnique();

                    b.HasIndex("ControlStrategyId");

                    b.HasIndex("CreatedById");

                    b.HasIndex("UpdatedById");

                    b.ToTable("tbl_node");
                });

            modelBuilder.Entity("SmartHome.Core.Entities.Entity.NodeData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("NodeId")
                        .HasColumnType("int");

                    b.Property<int>("PhysicalPropertyId")
                        .HasColumnType("int");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Value")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("NodeId");

                    b.HasIndex("PhysicalPropertyId");

                    b.ToTable("tbl_node_data");
                });

            modelBuilder.Entity("SmartHome.Core.Entities.Entity.PhysicalProperty", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<bool>("IsComplex")
                        .HasColumnType("bit");

                    b.Property<string>("Magnitude")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<string>("Unit")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("tbl_physical_property");
                });

            modelBuilder.Entity("SmartHome.Core.Entities.Entity.PhysicalPropertyControlStrategyLink", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ControlStrategyId")
                        .HasColumnType("int");

                    b.Property<int>("PhysicalPropertyId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ControlStrategyId");

                    b.HasIndex("PhysicalPropertyId");

                    b.ToTable("tbl_physicalproperty_controlstrategy_link");
                });

            modelBuilder.Entity("SmartHome.Core.Entities.Entity.UiConfiguration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Data")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<int>("ParentId")
                        .HasColumnType("int");

                    b.Property<byte[]>("RowVersion")
                        .HasColumnType("longblob");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("tbl_ui_configuration");
                });

            modelBuilder.Entity("SmartHome.Core.Entities.Role.AppRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("varchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("tbl_role");
                });

            modelBuilder.Entity("SmartHome.Core.Entities.SchedulingEntity.JobStatusEntity", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("tbl_scheduling_job_status");
                });

            modelBuilder.Entity("SmartHome.Core.Entities.SchedulingEntity.JobType", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("AssemblyName")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(255);

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(255);

                    b.Property<string>("FullyQualifiedName")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.ToTable("tbl_scheduling_job_type");
                });

            modelBuilder.Entity("SmartHome.Core.Entities.SchedulingEntity.ScheduleEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("CreatedById")
                        .HasColumnType("int");

                    b.Property<string>("CronExpression")
                        .IsRequired()
                        .HasColumnType("varchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("JobGroup")
                        .HasColumnType("longtext");

                    b.Property<string>("JobName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("JobStatusEntityId")
                        .HasColumnType("int");

                    b.Property<int>("JobTypeId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(255);

                    b.Property<byte[]>("RowVersion")
                        .HasColumnType("longblob");

                    b.Property<int>("ScheduleTypeid")
                        .HasColumnType("int");

                    b.Property<string>("SerializedJobSchedule")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("UpdatedById")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("JobStatusEntityId");

                    b.HasIndex("JobTypeId");

                    b.HasIndex("ScheduleTypeid");

                    b.HasIndex("UpdatedById");

                    b.ToTable("tbl_scheduling_schedules");
                });

            modelBuilder.Entity("SmartHome.Core.Entities.SchedulingEntity.ScheduleType", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("AssemblyName")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(255);

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(255);

                    b.Property<string>("FullyQualifiedName")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.ToTable("tbl_scheduling_schedule_type");
                });

            modelBuilder.Entity("SmartHome.Core.Entities.User.AppUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<int?>("ActivatedById")
                        .HasColumnType("int");

                    b.Property<DateTime>("ActivationDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .HasColumnType("varchar(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("varchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("varchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("varchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("ActivatedById");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("tbl_user");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("SmartHome.Core.Entities.Role.AppRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("SmartHome.Core.Entities.User.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("SmartHome.Core.Entities.User.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.HasOne("SmartHome.Core.Entities.Role.AppRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartHome.Core.Entities.User.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("SmartHome.Core.Entities.User.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SmartHome.Core.Entities.DictionaryEntity.DictionaryValue", b =>
                {
                    b.HasOne("SmartHome.Core.Entities.DictionaryEntity.Dictionary", "Dictionary")
                        .WithMany("Values")
                        .HasForeignKey("DictionaryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SmartHome.Core.Entities.Entity.AppUserNodeLink", b =>
                {
                    b.HasOne("SmartHome.Core.Entities.Entity.Node", "Node")
                        .WithMany("AllowedUsers")
                        .HasForeignKey("NodeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartHome.Core.Entities.User.AppUser", "User")
                        .WithMany("EligibleNodes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SmartHome.Core.Entities.Entity.ControlStrategy", b =>
                {
                    b.HasOne("SmartHome.Core.Entities.User.AppUser", "CreatedBy")
                        .WithMany("CreatedControlStrategies")
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartHome.Core.Entities.User.AppUser", "UpdatedBy")
                        .WithMany()
                        .HasForeignKey("UpdatedById");
                });

            modelBuilder.Entity("SmartHome.Core.Entities.Entity.Node", b =>
                {
                    b.HasOne("SmartHome.Core.Entities.Entity.ControlStrategy", "ControlStrategy")
                        .WithMany("Nodes")
                        .HasForeignKey("ControlStrategyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("SmartHome.Core.Entities.User.AppUser", "CreatedBy")
                        .WithMany("CreatedNodes")
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartHome.Core.Entities.User.AppUser", "UpdatedBy")
                        .WithMany()
                        .HasForeignKey("UpdatedById");
                });

            modelBuilder.Entity("SmartHome.Core.Entities.Entity.NodeData", b =>
                {
                    b.HasOne("SmartHome.Core.Entities.Entity.Node", "Node")
                        .WithMany("NodeData")
                        .HasForeignKey("NodeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartHome.Core.Entities.Entity.PhysicalProperty", "PhysicalProperty")
                        .WithMany()
                        .HasForeignKey("PhysicalPropertyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SmartHome.Core.Entities.Entity.PhysicalPropertyControlStrategyLink", b =>
                {
                    b.HasOne("SmartHome.Core.Entities.Entity.ControlStrategy", "ControlStrategy")
                        .WithMany("PhysicalProperties")
                        .HasForeignKey("ControlStrategyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartHome.Core.Entities.Entity.PhysicalProperty", "PhysicalProperty")
                        .WithMany("ControlStrategies")
                        .HasForeignKey("PhysicalPropertyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SmartHome.Core.Entities.Entity.UiConfiguration", b =>
                {
                    b.HasOne("SmartHome.Core.Entities.User.AppUser", "User")
                        .WithMany("UiConfiguration")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SmartHome.Core.Entities.SchedulingEntity.ScheduleEntity", b =>
                {
                    b.HasOne("SmartHome.Core.Entities.User.AppUser", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartHome.Core.Entities.SchedulingEntity.JobStatusEntity", "JobStatusEntity")
                        .WithMany()
                        .HasForeignKey("JobStatusEntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartHome.Core.Entities.SchedulingEntity.JobType", "JobType")
                        .WithMany()
                        .HasForeignKey("JobTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartHome.Core.Entities.SchedulingEntity.ScheduleType", "ScheduleType")
                        .WithMany()
                        .HasForeignKey("ScheduleTypeid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartHome.Core.Entities.User.AppUser", "UpdatedBy")
                        .WithMany()
                        .HasForeignKey("UpdatedById");
                });

            modelBuilder.Entity("SmartHome.Core.Entities.User.AppUser", b =>
                {
                    b.HasOne("SmartHome.Core.Entities.User.AppUser", "ActivatedBy")
                        .WithMany()
                        .HasForeignKey("ActivatedById");
                });
#pragma warning restore 612, 618
        }
    }
}