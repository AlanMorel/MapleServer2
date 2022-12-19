using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class LapenshardPacket
{
    private enum Mode : byte
    {
        Load = 0,
        Equip = 1,
        Unequip = 2,
        Select = 4,
        Upgrade = 5
    }

    public static PacketWriter Load(Item?[] items)
    {
        List<Item> itemList = items.Where(x => x is not null).ToList()!;

        PacketWriter pWriter = PacketWriter.Of(SendOp.ItemLapenshard);
        pWriter.Write(Mode.Load);
        pWriter.WriteInt(itemList.Count);
        foreach (Item item in itemList)
        {
            pWriter.WriteInt(item.Slot);
            pWriter.WriteInt(item.Id);
        }
        return pWriter;
    }

    public static PacketWriter Equip(int slotId, int itemId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ItemLapenshard);
        pWriter.Write(Mode.Equip);
        pWriter.WriteInt(slotId);
        pWriter.WriteInt(itemId);
        return pWriter;
    }

    public static PacketWriter Unequip(int slotId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ItemLapenshard);
        pWriter.Write(Mode.Unequip);
        pWriter.WriteInt(slotId);
        return pWriter;
    }

    public static PacketWriter Select(int successRate)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ItemLapenshard);
        pWriter.Write(Mode.Select);
        pWriter.WriteInt(successRate);
        return pWriter;
    }

    public static PacketWriter Upgrade(int itemId, bool result)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ItemLapenshard);
        pWriter.Write(Mode.Upgrade);
        pWriter.WriteLong();
        pWriter.WriteInt(itemId);
        pWriter.WriteInt();
        pWriter.WriteBool(result);
        return pWriter;
    }
}
