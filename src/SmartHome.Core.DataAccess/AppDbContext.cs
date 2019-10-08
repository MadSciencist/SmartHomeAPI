﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmartHome.Core.Entities.DictionaryEntity;
using SmartHome.Core.Entities.Entity;
using SmartHome.Core.Entities.Role;
using SmartHome.Core.Entities.User;

namespace SmartHome.Core.DataAccess
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Node> Nodes { get; set; }
        public DbSet<ControlStrategy> ControlStrategies { get; set; }
        public DbSet<RegisteredMagnitude> RegisteredMagnitudes { get; set; }

        public DbSet<NodeData> NodeData { get; set; }
        public DbSet<NodeDataMagnitude> DataMagnitudes { get; set; }
        public DbSet<DataRequestReason> RequestReasons { get; set; }
        public DbSet<UiConfiguration> UiConfigurations { get; set; }

        public DbSet<Dictionary> Dictionaries { get; set; }
        public DbSet<DictionaryValue> DictionaryValues { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>().ToTable("tbl_user");

            builder.Entity<AppUser>()
                .HasMany(x => x.UiConfiguration)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);

            builder.Entity<AppRole>().ToTable("tbl_role");

            builder.Entity<Node>()
                .HasKey(x => x.Id);

            builder.Entity<Node>()
                .HasIndex(x => x.ClientId)
                .IsUnique();

            builder.Entity<Node>()
                .HasOne(x => x.ControlStrategy)
                .WithMany(x => x.Nodes)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);

            builder.Entity<Node>()
                .HasOne(x => x.CreatedBy)
                .WithMany(u => u.CreatedNodes)
                .HasForeignKey(n => n.CreatedById);

            builder.Entity<ControlStrategy>()
                .HasMany(x => x.RegisteredMagnitudes)
                .WithOne(x => x.ControlStrategy)
                .HasForeignKey(x => x.ControlStrategyId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure many-to-many user-node relationship
            builder.Entity<AppUserNodeLink>()
                .HasKey(x => x.Id);

            builder.Entity<AppUserNodeLink>()
                .HasOne(x => x.Node)
                .WithMany(x => x.AllowedUsers)
                .HasForeignKey(x => x.NodeId);

            builder.Entity<AppUserNodeLink>()
                .HasOne(x => x.User)
                .WithMany(x => x.EligibleNodes)
                .HasForeignKey(x => x.UserId);

            // Configure dictionaries one-to-many relationship
            builder.Entity<Dictionary>()
                .HasKey(x => x.Id);

            builder.Entity<DictionaryValue>()
              .HasKey(x => x.Id);

            builder.Entity<DictionaryValue>()
                .Property(x => x.IsActive)
                .HasDefaultValue(true);

            builder.Entity<DictionaryValue>()
                .HasOne(x => x.Dictionary)
                .WithMany(x => x.Values)
                .HasForeignKey(x => x.DictionaryId);

            builder.Entity<UiConfiguration>()
                .HasKey(x => x.Id);
        }
    }
}