using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Enums;

namespace MapleServer2.Packets
{
    public static class MasteryPacket
    {
        public static Packet SetMasteryExp(byte masteryType, long totalMasteryExp)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MASTERY);

            pWriter.WriteByte();
            pWriter.WriteEnum((MasteryType) masteryType);
            pWriter.WriteLong(totalMasteryExp);

            return pWriter;
        }
        
        public static Packet ClaimReward(int rewardBoxDetails, int quantity, int itemId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MASTERY);

            pWriter.WriteByte(0x01);
            pWriter.WriteInt(rewardBoxDetails);
            pWriter.WriteInt(quantity);
            pWriter.WriteInt(itemId);
            pWriter.WriteShort(0x01);

            return pWriter;
        }
    }
}
