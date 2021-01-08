using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ms2Database.DB_Classes;

namespace Ms2Database
{
    public class Program
    {
        static void Main(string[] args)
        {
            using (Ms2DBContext context = new Ms2DBContext())
            {
                Account account = new Account()
                {
                    Username = "localhost",
                    Password = ""
                };
                context.Accounts.Add(account);
                context.SaveChanges();
            }
        }
    }
}
