﻿using Microsoft.EntityFrameworkCore;

namespace Ms2Database.DbClasses
{
    public class Ms2DbContext : DbContext // Create Database Schema
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<SkillTree> SkillTrees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB; Database=Maplestory2DB; Trusted_Connection=True;"); // Connection String
        }
    }
}
