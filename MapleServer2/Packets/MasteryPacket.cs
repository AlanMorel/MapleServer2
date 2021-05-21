using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Enums;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class MasteryPacket
    {
        private enum MasteryMode : byte
        {
            SetMasteryExp = 0x00,
            ClaimRewardBox = 0x01,
            GetCraftedItem = 0x02,
            MasteryNotice = 0x03,
        }

        public static Packet SetExp(MasteryType type, long totalExp)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MASTERY);

            pWriter.WriteEnum(MasteryMode.SetMasteryExp);
            pWriter.WriteEnum(type);
            pWriter.WriteLong(totalExp);

            return pWriter;
        }

        public static Packet ClaimReward(int rewardBoxDetails, Item item)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MASTERY);

            pWriter.WriteEnum(MasteryMode.ClaimRewardBox);
            pWriter.WriteInt(rewardBoxDetails);
            pWriter.WriteInt(item.Amount);
            pWriter.WriteInt(item.Id);
            pWriter.WriteShort((short) item.Rarity);

            return pWriter;
        }

        public static Packet GetCraftedItem(MasteryType type, Item item)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MASTERY);

            pWriter.WriteEnum(MasteryMode.GetCraftedItem);
            pWriter.WriteShort((short) type);
            pWriter.WriteInt(item.Amount);
            pWriter.WriteInt(item.Id);
            pWriter.WriteShort((short) item.Rarity);
            return pWriter;
        }

        public static Packet MasteryNotice(short noticeId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MASTERY);

            pWriter.WriteEnum(MasteryMode.MasteryNotice);
            pWriter.WriteShort(noticeId);
            return pWriter;
        }
    }
}
