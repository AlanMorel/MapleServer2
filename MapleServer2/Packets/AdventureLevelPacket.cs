using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class AdventureLevelPacket
    {
        public static Packet Prestige(Player player)
        {
            var pWriter = PacketWriter.Of(SendOp.ADVENTURE_LEVEL)
                .WriteByte(0x00)
                .WriteLong(player.PrestigeExperience) // PrestigeExp
                .WriteInt(player.PrestigeLevel) // PrestigeLevel
                .WriteLong(player.PrestigeExperience); // Same Prestige Exp??

            // Ranks: 2, 4, 6, 8, 10, 12, 20, 30, 40, 50, 60, 70, 80, 90
            int[] rankRewardsClaimed = { };
            pWriter.WriteInt(rankRewardsClaimed.Length);
            foreach (int rank in rankRewardsClaimed)
            {
                pWriter.WriteInt(rank);
            }

            return pWriter;
        }
    }
}
