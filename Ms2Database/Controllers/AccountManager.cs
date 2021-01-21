using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Ms2Database.DbClasses;

namespace Ms2Database.Controllers
{
    public class AccountManager
    {
        public void CreateAccount(string username, string password)
        {
            using (Ms2DbContext Context = new Ms2DbContext())
            {
                Account account = new Account()
                {
                    Username = username,
                    Password = password
                };

                Context.Accounts.Add(account);
                Context.SaveChanges();
            }
        }

        public void DeleteAccount(long id)
        {
            using (Ms2DbContext Context = new Ms2DbContext())
            {
                Account Account = Context.Accounts.Find(id);
                Context.Remove(Account);
                Context.SaveChanges();
            }
        }

        public Account GetAccInfoById(long id) // Queries db and retrieves account entry by Id
        {
            using (Ms2DbContext Context = new Ms2DbContext())
            {
                Account Account = Context.Accounts.FirstOrDefault(a => a.AccountId == id); // Retrieve entry by Id
                return Account;
            }
        }

        public Account GetAccInfoByUser(string username)
        {
            using (Ms2DbContext Context = new Ms2DbContext())
            {
                Account Account = Context.Accounts.FirstOrDefault(a => a.Username.ToLower() == username.ToLower()); // Retrieve entry by username
                return Account;
            }
        }

        public void EditAccInfo(long id, string username = "", string password = "") // Allows account entry changes
        {
            using (Ms2DbContext Context = new Ms2DbContext())
            {
                Account Account = Context.Accounts.FirstOrDefault(a => a.AccountId == id); // Retrieve entry by Id
                if (Account == null) // Checks to see if account Id exists
                {
                    Debug.Assert(Account != null, "No entry starting with account ID: " + id);
                    return;
                }
                if (!string.IsNullOrEmpty(username)) // Prevents empty entries
                {
                    Account.Username = username;
                }
                if (!string.IsNullOrEmpty(password))
                {
                    Account.Password = password;
                }
                Context.SaveChanges(); // Updates database with changes.
            }
        }
    }
}
