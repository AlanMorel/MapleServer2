﻿using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class LapenshardPacket
{
    private enum LapenshardMode : byte
    {
        Load = 0,
        Equip = 1,
        Unequip = 2,
        Select = 4,
        Upgrade = 5,
    }

    public static PacketWriter LapenshardLoad(Item[] items)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM_LAPENSHARD);
        pWriter.Write(LapenshardMode.Load);
        pWriter.WriteInt(items.Count(x => x is not null));
        foreach (Item item in items.Where(x => x is not null))
        {
            pWriter.WriteInt(item.Slot);
            pWriter.WriteInt(item.Id);
        }
        return pWriter;
    }

    public static PacketWriter LapenshardEquip(int slotId, int itemId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM_LAPENSHARD);
        pWriter.Write(LapenshardMode.Equip);
        pWriter.WriteInt(slotId);
        pWriter.WriteInt(itemId);
        return pWriter;
    }

    public static PacketWriter LapenshardUnequip(int slotId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM_LAPENSHARD);
        pWriter.Write(LapenshardMode.Unequip);
        pWriter.WriteInt(slotId);
        return pWriter;
    }

    public static PacketWriter LapenshardSelect(int successRate)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM_LAPENSHARD);
        pWriter.Write(LapenshardMode.Select);
        pWriter.WriteInt(successRate);
        return pWriter;
    }

    public static PacketWriter LapenshardUpgrade(int itemId, bool result)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM_LAPENSHARD);
        pWriter.Write(LapenshardMode.Upgrade);
        pWriter.WriteLong();
        pWriter.WriteInt(itemId);
        pWriter.WriteInt();
        pWriter.WriteBool(result);
        return pWriter;
    }
}
