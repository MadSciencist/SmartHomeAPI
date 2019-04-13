﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmartHome.Domain.DictionaryEntity;
using SmartHome.Domain.Entity;
using SmartHome.Domain.Role;
using SmartHome.Domain.User;

namespace SmartHome.API.Persistence.Identity
{
    public class AppIdentityDbContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
        {
        }

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
            builder.Entity<AppUserNode>()
                .HasKey(x => x.Id);

            builder.Entity<AppUserNode>()
                .HasOne(x => x.Node)
                .WithMany(x => x.AllowedUsers)
                .HasForeignKey(x => x.NodeId);

            builder.Entity<AppUserNode>()
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
        }
    }
}