﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmartHome.Domain.DictionaryEntity;
using SmartHome.Domain.Entity;
using SmartHome.Domain.Role;
using SmartHome.Domain.User;

namespace SmartHome.Core.Persistence
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Node> Nodes { get; set; }
        public DbSet<ControlStrategy> ControlStrategies { get; set; }
        public DbSet<NodeCommand> Commands { get; set; }
        public DbSet<RestTemplate> RestTemplates  { get; set; }
        public DbSet<RestTemplateValues> RestTemplateValues { get; set; }



        public DbSet<Dictionary> Dictionaries { get; set; }
        public DbSet<DictionaryValue> DictionaryValues { get; set; }

        // Navigation properties
        public DbSet<AppUserNodeLink> AppUserNodeLink { get; set; }
        public DbSet<NodeCommandNodeLink> NodeCommandNodeLink { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>().ToTable("appuser");

            builder.Entity<Node>()
                .HasKey(x => x.Id);

            builder.Entity<Node>()
                .HasOne(x => x.CreatedBy)
                .WithMany(u => u.CreatedNodes)
                .HasForeignKey(n => n.CreatedById);

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

            // Configure many-to-many node-nodeCommand relationship
            builder.Entity<NodeCommandNodeLink>()
                .HasKey(x => x.Id);

            builder.Entity<NodeCommandNodeLink>()
                .HasOne(x => x.Node)
                .WithMany(x => x.AllowedCommands)
                .HasForeignKey(x => x.NodeId);

            builder.Entity<NodeCommandNodeLink>()
                .HasOne(x => x.NodeCommand)
                .WithMany(x => x.Nodes)
                .HasForeignKey(x => x.NodeCommandId);

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
        }
    }
}