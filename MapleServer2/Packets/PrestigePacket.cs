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

        public static PacketWriter Prestige(Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PRESTIGE);
            pWriter.Write(PrestigePacketMode.Prestige);
            pWriter.WriteLong(player.Levels.PrestigeExp); // PrestigeExp
            pWriter.WriteInt(player.Levels.PrestigeLevel); // PrestigeLevel
            pWriter.WriteLong(player.Levels.PrestigeExp); // Same Prestige Exp??

            List<int> rankRewardsClaimed = player.PrestigeRewardsClaimed;
            pWriter.WriteInt(rankRewardsClaimed.Count);
            foreach (int rank in rankRewardsClaimed)
            {
                pWriter.WriteInt(rank);
            }

            return pWriter;
        }

        public static PacketWriter ExpUp(Player player, long amount)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PRESTIGE);

            pWriter.Write(PrestigePacketMode.PrestigeExp);
            pWriter.WriteLong(player.Levels.PrestigeExp);
            pWriter.WriteLong(amount);

            return pWriter;
        }

        public static PacketWriter LevelUp(IFieldObject<Player> player, int level)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PRESTIGE);

            pWriter.Write(PrestigePacketMode.PrestigeLevel);
            pWriter.WriteInt(player.ObjectId);
            pWriter.WriteInt(level);

            return pWriter;
        }

        public static PacketWriter Reward(int rank)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PRESTIGE);

            pWriter.Write(PrestigePacketMode.Reward);
            pWriter.WriteByte(0x01); // Unknown maybe boolean for whether to accept?
            pWriter.WriteInt(1); // Amount of rewards to accept (multiple ranks)
            pWriter.WriteInt(rank);

            return pWriter;
        }
    }
}
