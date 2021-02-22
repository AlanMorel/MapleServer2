using System.Collections.Generic;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets
{
    public static class ItemBreakPacket
    {
        private enum ItemBreakMode : byte
        {
            Add = 0x01,
            Remove = 0x02,
            ShowRewards = 0x03,
            Results = 0x05,
        }

        public static Packet Add(long uid, short slot, int amount)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM_BREAK);
            pWriter.WriteEnum(ItemBreakMode.Add);
            pWriter.WriteLong(uid);
            pWriter.WriteShort(slot);
            pWriter.WriteInt(amount);

            return pWriter;
        }

        public static Packet Remove(long uid)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM_BREAK);
            pWriter.WriteEnum(ItemBreakMode.Remove);
            pWriter.WriteLong(uid);

            return pWriter;
        }

        public static Packet Results(Dictionary<int, int> rewards)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM_BREAK);
            pWriter.WriteEnum(ItemBreakMode.Results);
            pWriter.WriteInt(rewards.Count);
            foreach (KeyValuePair<int, int> item in rewards)
            {
                pWriter.WriteInt(item.Key);
                pWriter.WriteInt(item.Value);
                pWriter.WriteInt(item.Value);
            }

            return pWriter;
        }

        public static Packet ShowRewards(Dictionary<int, int> rewards)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM_BREAK);
            pWriter.WriteEnum(ItemBreakMode.ShowRewards);
            pWriter.WriteByte(1); // unknown
            pWriter.WriteInt(rewards.Count);
            foreach (KeyValuePair<int, int> item in rewards)
            {
                pWriter.WriteInt(item.Key);
                pWriter.WriteInt(item.Value);
            }

            return pWriter;
        }
    }
}
