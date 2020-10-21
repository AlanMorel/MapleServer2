using Maple2.Sql.Model;
using Microsoft.EntityFrameworkCore;

namespace Maple2.Sql.Context {
    // The purpose of this context is only for database definition and initialization
    public class InitializationContext : DbContext {
        public DbSet<Account> Account { get; set; }
        public DbSet<Achieve> Achieve { get; set; }
        public DbSet<BlackMarketListing> BlackMarketListing { get; set; }
        public DbSet<Character> Character { get; set; }
        public DbSet<CharacterConfig> CharacterConfig { get; set; }
        public DbSet<CharacterProgress> CharacterProgress { get; set; }
        public DbSet<Guild> Guild { get; set; }
        public DbSet<Item> Item { get; set; }
        public DbSet<Mail> Mail { get; set; }
        public DbSet<MesoMarketListing> MesoMarketListing { get; set; }
        public DbSet<Quest> Quest { get; set; }
        public DbSet<SkillTab> SkillTab { get; set; }

        public InitializationContext(string connectionString)
            : base(new DbContextOptionsBuilder().UseMySql(connectionString).Options) { }

        public InitializationContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Account>(Sql.Model.Account.Configure);
            modelBuilder.Entity<Achieve>(Sql.Model.Achieve.Configure);
            modelBuilder.Entity<BlackMarketListing>(Sql.Model.BlackMarketListing.Configure);
            modelBuilder.Entity<Character>(Sql.Model.Character.Configure);
            modelBuilder.Entity<CharacterConfig>(Sql.Model.CharacterConfig.Configure);
            modelBuilder.Entity<CharacterProgress>(Sql.Model.CharacterProgress.Configure);
            modelBuilder.Entity<Guild>(Sql.Model.Guild.Configure);
            modelBuilder.Entity<Item>(Sql.Model.Item.Configure);
            modelBuilder.Entity<Mail>(Sql.Model.Mail.Configure);
            modelBuilder.Entity<MesoMarketListing>(Sql.Model.MesoMarketListing.Configure);
            modelBuilder.Entity<Quest>(Sql.Model.Quest.Configure);
            modelBuilder.Entity<SkillTab>(Sql.Model.SkillTab.Configure);
        }

        public bool Initialize() {
            bool created = Database.EnsureCreated();
            if (!created) {
                return false;
            }

            Database.ExecuteSqlRaw("ALTER TABLE account AUTO_INCREMENT = 100000000000");
            Database.ExecuteSqlRaw("ALTER TABLE character AUTO_INCREMENT = 120000000000");
            Database.ExecuteSqlRaw("ALTER TABLE item AUTO_INCREMENT = 200000000000");
            Database.ExecuteSqlRaw("ALTER TABLE mail AUTO_INCREMENT = 300000000000");
            Database.ExecuteSqlRaw("ALTER TABLE bm_listing AUTO_INCREMENT = 400000000000");
            Database.ExecuteSqlRaw("ALTER TABLE mm_listing AUTO_INCREMENT = 420000000000");
            Database.ExecuteSqlRaw("ALTER TABLE skilltab AUTO_INCREMENT = 440000000000");

            return true;
        }
    }
}