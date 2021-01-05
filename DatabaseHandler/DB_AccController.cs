using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseHandler
{
    public class DB_AccController
    {
        public Account GetAccountByUser(string username)
        {
            using (Maplestory2DBEntities context = new Maplestory2DBEntities())
            {
                Account accountInfo = context.Accounts.FirstOrDefault(r => r.Username == username);
                return accountInfo;
            }
        }
    }
}
