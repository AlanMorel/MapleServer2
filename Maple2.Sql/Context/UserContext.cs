using Maple2.Sql.Model;
using Microsoft.EntityFrameworkCore;

namespace Maple2.Sql.Context {
    public class UserContext : DbContext, IItemAccessor {
        public DbSet<Account> Account { get; set; }
        public DbSet<Achieve> Achieve { get; set; }
        public DbSet<Character> Character { get; set; }
        public DbSet<CharacterConfig> CharacterConfig { get; set; }
        public DbSet<CharacterProgress> CharacterProgress { get; set; }
        public DbSet<Guild> Guild { get; set; }
        public DbSet<Item> Item { get; set; }
        public DbSet<Quest> Quest { get; set; }
        public DbSet<SkillTab> SkillTab { get; set; }

        public UserContext(string connectionString)
            : base(new DbContextOptionsBuilder().UseMySql(connectionString).Options) { }
        public UserContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Account>(Sql.Model.Account.Configure);
            modelBuilder.Entity<Achieve>(Sql.Model.Achieve.Configure);
            modelBuilder.Entity<Character>(Sql.Model.Character.Configure);
            modelBuilder.Entity<CharacterConfig>(Sql.Model.CharacterConfig.Configure);
            modelBuilder.Entity<CharacterProgress>(Sql.Model.CharacterProgress.Configure);
            modelBuilder.Entity<Guild>(Sql.Model.Guild.Configure);
            modelBuilder.Entity<Item>(Sql.Model.Item.Configure);
            modelBuilder.Entity<Quest>(Sql.Model.Quest.Configure);
            modelBuilder.Entity<SkillTab>(Sql.Model.SkillTab.Configure);
        }
    }
}