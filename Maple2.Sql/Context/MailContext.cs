using Maple2.Sql.Model;
using Microsoft.EntityFrameworkCore;

namespace Maple2.Sql.Context {
    public class MailContext : DbContext, IItemAccessor {
        public DbSet<Character> Character { get; set; }
        public DbSet<Item> Item { get; set; }
        public DbSet<Mail> Mail { get; set; }

        public MailContext(string connectionString)
            : base(new DbContextOptionsBuilder().UseMySql(connectionString).Options) { }
        public MailContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Character>(Sql.Model.Character.Configure);
            modelBuilder.Entity<Item>(Sql.Model.Item.Configure);
            modelBuilder.Entity<Mail>(Sql.Model.Mail.Configure);
        }
    }
}