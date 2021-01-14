using MapleServer2.Packets;

namespace MapleServer2.Types
{
    public class Level
    {
        private readonly Player Player;
        public short PlayerLevel { get; private set; }
        public int PrestigeLevel { get; private set; }

        public Level(Player player, short playerLevel, int prestigeLevel)
        {
            Player = player;
            PlayerLevel = playerLevel;
            PrestigeLevel = prestigeLevel;
        }

        public void SetPrestigeLevel(int level)
        {
            PrestigeLevel = level;
            Player.Exp.PrestigeExp = 0;
            Player.Session.Send(PrestigePacket.ExpUp(Player, 0));
            Player.Session.Send(PrestigePacket.LevelUp(Player.Session.FieldPlayer, PrestigeLevel));
        }

        public void SetLevel(short level)
        {
            PlayerLevel = level;
            Player.Exp.PlayerExp = 0;
            Player.Session.Send(ExperiencePacket.ExpUp(0, Player.Exp.PlayerExp, 0));
            Player.Session.Send(ExperiencePacket.LevelUp(Player.Session.FieldPlayer, PlayerLevel));
        }

        public void PlayerLevelUp()
        {
            PlayerLevel++;
            // TODO: Gain max HP and heal to max hp
            Player.StatPointDistribution.AddTotalStatPoints(5);
            Player.Session.Send(StatPointPacket.WriteTotalStatPoints(Player));
            Player.Session.Send(ExperiencePacket.LevelUp(Player.Session.FieldPlayer, PlayerLevel));
        }

        public void PrestigeLevelUp()
        {
            PrestigeLevel++;
            Player.Session.Send(PrestigePacket.LevelUp(Player.Session.FieldPlayer, PrestigeLevel));
        }

    }
}
