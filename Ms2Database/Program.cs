using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ms2Database.DbClasses;

namespace Ms2Database
{
    public class Program
    {
        static void Main(string[] args)
        {
            using (Ms2DbContext context = new Ms2DbContext())
            {
                Account account = new Account()
                {
                    Username = "localhost",
                    Password = ""
                };
                context.Accounts.Add(account);
                context.SaveChanges();
            }
            Console.WriteLine("Database has been created");
            Console.WriteLine("Account localhost has been added to table");
        }
    }
}
