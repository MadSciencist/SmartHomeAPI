using Microsoft.EntityFrameworkCore;
using SmartHome.Domain.DictionaryEntity;
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
        public DbSet<ControlStrategy> ControlStrategies { get; set; }
        public DbSet<ControllableNode> ControllableNodes { get; set; }
        public DbSet<Dictionary> Dictionaries { get; set; }
        public DbSet<DictionaryValue> DictionaryValues { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
