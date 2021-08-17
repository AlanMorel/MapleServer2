using Maple2Storage.Enums;
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

        public Currency Meret { get; private set; }
        public Currency GameMeret { get; private set; }
        public Currency EventMeret { get; private set; }

        public Home Home;

        public virtual ICollection<Player> Players { get; set; }

        public Account() { }

        public Account(string username, string passwordHash)
        {
            Username = username;
            PasswordHash = passwordHash;
            CreationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + Environment.TickCount;
            LastLoginTime = CreationTime;
            CharacterSlots = 7;
            Meret = new Currency(CurrencyType.Meret, 0);
            GameMeret = new Currency(CurrencyType.GameMeret, 0);
            EventMeret = new Currency(CurrencyType.EventMeret, 0);

            Id = DatabaseManager.CreateAccount(this);
        }

        public bool RemoveMerets(long amount)
        {
            if (Meret.Modify(-amount) || GameMeret.Modify(-amount) || EventMeret.Modify(-amount))
            {
                return true;
            }

            if (Meret.Amount + GameMeret.Amount + EventMeret.Amount >= amount)
            {
                long rest = Meret.Amount + GameMeret.Amount + EventMeret.Amount - amount;
                Meret.SetAmount(rest);
                GameMeret.SetAmount(0);
                EventMeret.SetAmount(0);

                return true;
            }

            return false;
        }
    }
}
