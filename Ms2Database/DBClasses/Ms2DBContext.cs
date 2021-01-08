using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ms2Database.DBClasses
{
    public class Ms2DBContext : DbContext
    {
        public Ms2DBContext() : base("name=DatabaseConnectionString")
        {
            //Database.SetInitializer<Ms2DBContext>(new DropCreateDatabaseAlways<Ms2DBContext>());
        }
        public DbSet<Account> Accounts { get; set; }
    }
}
