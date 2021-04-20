using System;
using System.Collections.Generic;

namespace MapleServer2.Types
{
    public class Account
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public long CreationTime { get; set; }

        public virtual ICollection<Player> Players { get; set; }

        public Account(long id, string username, string password)
        {
            Id = id;
            Username = username;
            Password = password;
            CreationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + Environment.TickCount;
        }
    }
}
