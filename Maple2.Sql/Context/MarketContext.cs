using Maple2.Sql.Model;
using Microsoft.EntityFrameworkCore;

namespace Maple2.Sql.Context {
    public class MarketContext : DbContext, IItemAccessor {
        public DbSet<BlackMarketListing> BlackMarketListing { get; set; }
        public DbSet<MesoMarketListing> MesoMarketListing { get; set; }
        public DbSet<Item> Item { get; set; }

        public MarketContext(string connectionString)
            : base(new DbContextOptionsBuilder().UseMySql(connectionString).Options) { }
        public MarketContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<BlackMarketListing>(Sql.Model.BlackMarketListing.Configure);
            modelBuilder.Entity<Item>(Sql.Model.Item.Configure);
            modelBuilder.Entity<MesoMarketListing>(Sql.Model.MesoMarketListing.Configure);
        }
    }
}