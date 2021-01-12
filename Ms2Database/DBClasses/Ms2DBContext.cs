using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ms2Database.DbClasses
{
    public class Ms2DbContext : DbContext
    {
        public Ms2DbContext() : base("name=DatabaseConnectionString") // Creates context for database via connection string
        {
            //Database.SetInitializer<Ms2DbContext>(new DropCreateDatabaseAlways<Ms2DbContext>());
        }
        public DbSet<Account> Accounts { get; set; } // Creates account table during database generation

        public DbSet<Character> Characters { get; set; } // Create character table during database generation
    }
}
