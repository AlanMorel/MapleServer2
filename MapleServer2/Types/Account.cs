using System;
using System.Collections.Generic;
using MapleServer2.Database;

namespace MapleServer2.Types
{
    public class Account
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public long CreationTime { get; set; }
        public long LastLoginTime { get; set; }
        public int CharacterSlots { get; set; }

        public virtual ICollection<Player> Players { get; set; }

        public Account() { }

        public Account(string username, string passwordHash)
        {
            Username = username;
            PasswordHash = passwordHash;
            CreationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + Environment.TickCount;
            LastLoginTime = CreationTime;
            CharacterSlots = 7;
            Id = DatabaseManager.CreateAccount(this);
        }
    }
}
