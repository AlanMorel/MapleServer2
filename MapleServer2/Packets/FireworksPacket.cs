using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class FireWorksPacket
{
    private enum FireworksPacketMode : byte
    {
        TreasureChest = 0x4,
        Gacha = 0x5
    }

    public static PacketWriter TreasureChest(int itemId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Fireworks);
        pWriter.Write(FireworksPacketMode.TreasureChest);
        pWriter.WriteInt(itemId);
        return pWriter;
    }

    public static PacketWriter Gacha(List<Item> items)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Fireworks);
        pWriter.Write(FireworksPacketMode.Gacha);
        pWriter.WriteInt(items.Count);
        foreach (Item item in items)
        {
            pWriter.WriteInt(item.Id);
            pWriter.WriteInt(item.Amount);
            pWriter.WriteInt(item.Rarity);
            pWriter.WriteByte();
        }
        return pWriter;
    }
}
