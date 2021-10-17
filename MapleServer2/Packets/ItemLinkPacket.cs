using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets.Helpers;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class ItemLinkPacket
    {
        public static Packet SendLinkItem(List<Item> items)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.USER_CHAT_ITEM_LINK);
            pWriter.WriteInt(items.Count);
            foreach (Item item in items)
            {
                pWriter.WriteLong(item.Uid);
                pWriter.WriteInt(item.Id);
                pWriter.WriteInt(item.Rarity);
                pWriter.WriteItem(item);
            }
            return pWriter;
        }
    }
}
