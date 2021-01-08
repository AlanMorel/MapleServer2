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
        public Tuple<string, string> GetAccInfoById(long id)
        {
            using (Ms2DbContext context = new Ms2DbContext())
            {
                Account account = context.Accounts.FirstOrDefault(a => a.AccountId == id);
                return new Tuple<string, string>(account.Username, account.Password);
            }
        }

        public void SetAccInfo(long id, string username, string password)
        {
            using (Ms2DbContext context = new Ms2DbContext())
            {
                Account account = context.Accounts.FirstOrDefault(a => a.AccountId == id);
                if (!string.IsNullOrEmpty(username))
                {
                    account.Username = username;
                }
                if (!string.IsNullOrEmpty(password))
                {
                    account.Password = password;
                }
                context.SaveChanges();
            }
        }

    }
}
