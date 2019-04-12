using Microsoft.EntityFrameworkCore;
using SmartHome.Domain.Entity;

namespace SmartHome.API.Persistence.App
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Node> Nodes { get; set; }
        public DbSet<NodeType> NodeTypes { get; set; }
        public DbSet<ControllableNode> ControllableNodes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Node>(node =>
            {
                node.HasOne(x => x.CreatedBy)
                    .WithMany(u => u.EligibleNodes);
            });
        }
    }
}
