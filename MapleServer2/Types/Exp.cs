using MapleServer2.Data.Static;
using MapleServer2.Packets;

namespace MapleServer2.Types
{
    public class Exp
    {
        private readonly Player Player;
        public long PlayerExp { get; set; }
        public long RestExp { get; set; }
        public long PrestigeExp { get; set; }

        public Exp(Player player, long playerExp, long restExp, long prestigeExp)
        {
            Player = player;
            PlayerExp = playerExp;
            RestExp = restExp;
            PrestigeExp = prestigeExp;
        }

        public void GainExp(int amount)
        {
            if (amount <= 0 || Player.Levels.PlayerLevel >= Player.MAX_LEVEL)
            {
                return;
            }

            long newExp = PlayerExp + amount + RestExp;

            if (RestExp > 0)
            {
                RestExp -= amount;

            }
            else
            {
                RestExp = 0;
            }

            while (newExp >= ExpMetadataStorage.GetExpToLevel(Player.Levels.PlayerLevel))
            {
                newExp -= ExpMetadataStorage.GetExpToLevel(Player.Levels.PlayerLevel);
                Player.Levels.PlayerLevelUp();
                if (Player.Levels.PlayerLevel >= Player.MAX_LEVEL)
                {
                    newExp = 0;
                    break;
                }
            }

            PlayerExp = newExp;
            Player.Session.Send(ExperiencePacket.ExpUp(amount, newExp, RestExp));
        }

        public void GainPrestigeExp(long amount)
        {
            if (Player.Levels.PlayerLevel < 50) // Can only gain prestige exp after level 50.
            {
                return;
            }
            // Prestige exp can only be earned 1M exp per day. 
            // TODO: After 1M exp, reduce the gain and reset the exp gained every midnight.

            long newPrestigeExp = PrestigeExp + amount;

            if (newPrestigeExp >= 1000000)
            {
                newPrestigeExp -= 1000000;
                Player.Levels.PrestigeLevelUp();
            }

            PrestigeExp = newPrestigeExp;
            Player.Session.Send(PrestigePacket.ExpUp(Player, amount));
        }
    }
}
