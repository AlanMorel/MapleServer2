using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets.Helpers;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class ItemLockPacket
{
    private enum ItemLockMode : byte
    {
        Add = 0x01,
        Remove = 0x02,
        Update = 0x03
    }

    public static PacketWriter Add(long uid, short slot)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ItemLock);
        pWriter.Write(ItemLockMode.Add);
        pWriter.WriteLong(uid);
        pWriter.WriteShort(slot);

        return pWriter;
    }

    public static PacketWriter Remove(long uid)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ItemLock);
        pWriter.Write(ItemLockMode.Remove);
        pWriter.WriteLong(uid);

        return pWriter;
    }

    public static PacketWriter UpdateItems(List<Item> items)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ItemLock);
        pWriter.Write(ItemLockMode.Update);
        pWriter.WriteByte((byte) items.Count);
        foreach (Item item in items)
        {
            pWriter.WriteLong(item.Uid);
            pWriter.WriteItem(item);
        }

        return pWriter;
    }
}
