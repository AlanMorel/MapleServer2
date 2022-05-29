using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Database.Types;

namespace MapleServer2.Packets;

public static class RouletteGamePacket
{
    private enum BonusGameMode : byte
    {
        OpenWheel = 0x01,
        SpinWheel = 0x02
    }

    public static PacketWriter OpenWheel(List<RouletteGameItem> items)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.RouletteGame);
        pWriter.Write(BonusGameMode.OpenWheel);
        pWriter.WriteByte();
        pWriter.WriteInt(items.Count);
        foreach (RouletteGameItem item in items)
        {
            pWriter.WriteInt(item.ItemId);
            pWriter.WriteByte(item.ItemRarity);
            pWriter.WriteInt(item.ItemAmount);
        }
        pWriter.WriteInt();
        return pWriter;
    }

    public static PacketWriter SpinWheel(List<int> indexes, List<RouletteGameItem> items)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.RouletteGame);
        pWriter.Write(BonusGameMode.SpinWheel);
        pWriter.WriteInt(indexes.Count);
        for(int i = 0; i < indexes.Count; i++)
        {
            pWriter.WriteInt(indexes[i]);
            pWriter.WriteInt(items[indexes[i]].ItemId);
            pWriter.WriteInt(items[indexes[i]].ItemAmount);
            pWriter.WriteShort(items[indexes[i]].ItemRarity);
        }
        return pWriter;
    }
}
