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
        public Ms2DbContext() : base("name=DatabaseConnectionString")
        {
            //Database.SetInitializer<Ms2DBContext>(new DropCreateDatabaseAlways<Ms2DBContext>());
        }
        public DbSet<Account> Accounts { get; set; }
    }
}
