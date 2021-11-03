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

        public static PacketWriter SetExp(MasteryType type, long totalExp)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MASTERY);

            pWriter.Write(MasteryMode.SetMasteryExp);
            pWriter.Write(type);
            pWriter.WriteLong(totalExp);

            return pWriter;
        }

        public static PacketWriter ClaimReward(int rewardBoxDetails, Item item)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MASTERY);

            pWriter.Write(MasteryMode.ClaimRewardBox);
            pWriter.WriteInt(rewardBoxDetails);
            pWriter.WriteInt(item.Amount);
            pWriter.WriteInt(item.Id);
            pWriter.WriteShort((short) item.Rarity);

            return pWriter;
        }

        public static PacketWriter GetCraftedItem(MasteryType type, Item item)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MASTERY);

            pWriter.Write(MasteryMode.GetCraftedItem);
            pWriter.WriteShort((short) type);
            pWriter.WriteInt(item.Amount);
            pWriter.WriteInt(item.Id);
            pWriter.WriteShort((short) item.Rarity);
            return pWriter;
        }

        public static PacketWriter MasteryNotice(short noticeId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MASTERY);

            pWriter.Write(MasteryMode.MasteryNotice);
            pWriter.WriteShort(noticeId);
            return pWriter;
        }
    }
}
