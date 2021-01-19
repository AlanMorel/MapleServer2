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
            using (Ms2DbContext context = new Ms2DbContext())
            {
                Account account = new Account()
                {
                    Username = username,
                    Password = password
                };

                context.Accounts.Add(account);
                context.SaveChanges();
            }
        }

        public void DeleteAccount(long id)
        {
            using (Ms2DbContext context = new Ms2DbContext())
            {
                Account account = context.Accounts.Find(id);
                context.Remove(account);
                context.SaveChanges();
            }
        }

        public Account GetAccInfoById(long id) // Queries db and retrieves account entry by Id
        {
            using (Ms2DbContext context = new Ms2DbContext())
            {
                Account account = context.Accounts.FirstOrDefault(a => a.AccountId == id); // Retrieve entry by Id
                return account;
            }
        }

        public Account GetAccInfoByUser(string username)
        {
            using (Ms2DbContext context = new Ms2DbContext())
            {
                Account account = context.Accounts.FirstOrDefault(a => a.Username.ToLower() == username.ToLower()); // Retrieve entry by Id
                return account;
            }
        }

        public void SetAccInfo(long id, string username = "", string password = "") // Allows account entry changes
        {
            using (Ms2DbContext context = new Ms2DbContext())
            {
                Account account = context.Accounts.FirstOrDefault(a => a.AccountId == id); // Retrieve entry by Id
                if (account == null) // Checks to see if account Id exists
                {
                    Debug.Assert(account != null, "No entry starting with account ID: " + id);
                    return;
                }
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
