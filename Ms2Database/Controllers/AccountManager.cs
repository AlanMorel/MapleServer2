using System;
using Ms2Database.DbClasses;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ms2Database.Controllers
{
    public class AccountManager
    {
        public Tuple<string, string> GetAccInfoById(long id) // Queries db and retrieves account entry by Id
        {
            using (Ms2DbContext context = new Ms2DbContext())
            {
                Account account = context.Accounts.FirstOrDefault(a => a.AccountId == id); // Retrieve entry by Id
                return new Tuple<string, string>(account.Username, account.Password);
            }
        }

        public void SetAccInfo(long id, string username, string password) // Allows account entry changes
        {
            using (Ms2DbContext context = new Ms2DbContext())
            {
                Account account = context.Accounts.FirstOrDefault(a => a.AccountId == id); // Retrieve entry by Id
                if (!string.IsNullOrEmpty(username)) // Prevents empty entries
                {
                    account.Username = username;
                }
                if (!string.IsNullOrEmpty(password))
                {
                    account.Password = password;
                }
                context.SaveChanges(); // Updates database with changes.
            }
        }

    }
}
