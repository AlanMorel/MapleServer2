using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Enums;

namespace MapleServer2.Packets
{
    public static class MasteryPacket
    {
        private enum MasteryMode : byte
        {
            SetMasteryExp = 0x00,
            ClaimRewardBox = 0x01
        }

        public static Packet SetExp(MasteryType type, long totalExp)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MASTERY);

            pWriter.WriteEnum(MasteryMode.SetMasteryExp);
            pWriter.WriteEnum(type);
            pWriter.WriteLong(totalExp);

            return pWriter;
        }

        public static Packet ClaimReward(int rewardBoxDetails, int quantity, int itemId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MASTERY);

            pWriter.WriteEnum(MasteryMode.ClaimRewardBox);
            pWriter.WriteInt(rewardBoxDetails);
            pWriter.WriteInt(quantity);
            pWriter.WriteInt(itemId);
            pWriter.WriteShort(1);

            return pWriter;
        }
    }
}
