using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class PrestigePacket
    {
        private enum PrestigePacketMode : byte
        {
            Prestige = 0x00,
            PrestigeExp = 0x01,
            PrestigeLevel = 0x02,
            Reward = 0x04
        }

        public static Packet Prestige(Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PRESTIGE);
            pWriter.WriteMode(PrestigePacketMode.Prestige);
            pWriter.WriteLong(player.Exp.PrestigeExp); // PrestigeExp
            pWriter.WriteInt(player.Levels.PrestigeLevel); // PrestigeLevel
            pWriter.WriteLong(player.Exp.PrestigeExp); // Same Prestige Exp??

            // Ranks: 2, 4, 6, 8, 10, 12, 20, 30, 40, 50, 60, 70, 80, 90
            int[] rankRewardsClaimed = { };
            pWriter.WriteInt(rankRewardsClaimed.Length);
            foreach (int rank in rankRewardsClaimed)
            {
                pWriter.WriteInt(rank);
            }

            return pWriter;
        }

        public static Packet ExpUp(Player player, long amount)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PRESTIGE);

            pWriter.WriteMode(PrestigePacketMode.PrestigeExp);
            pWriter.WriteLong(player.Exp.PrestigeExp);
            pWriter.WriteLong(amount);

            return pWriter;
        }

        public static Packet LevelUp(IFieldObject<Player> player, int level)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PRESTIGE);

            pWriter.WriteMode(PrestigePacketMode.PrestigeLevel);
            pWriter.WriteInt(player.ObjectId);
            pWriter.WriteInt(level);

            return pWriter;
        }

        public static Packet Reward(int rank)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PRESTIGE);

            pWriter.WriteMode(PrestigePacketMode.Reward);
            pWriter.WriteByte(0x01); // Unknown maybe boolean for whether to accept?
            pWriter.WriteInt(1); // Amount of rewards to accept (multiple ranks)
            pWriter.WriteInt(rank);

            return pWriter;
        }
    }
}
