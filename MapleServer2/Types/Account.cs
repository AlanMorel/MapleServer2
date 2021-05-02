using System;
using System.Collections.Generic;
using MapleServer2.Database;

namespace MapleServer2.Types
{
    public class Account
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public long CreationTime { get; set; }

        public virtual ICollection<Player> Players { get; set; }

        public Account() { }

        public Account(string username, string password)
        {
            Username = username;
            Password = password;
            CreationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + Environment.TickCount;
            Id = DatabaseManager.CreateAccount(this);
        }
    }
}
