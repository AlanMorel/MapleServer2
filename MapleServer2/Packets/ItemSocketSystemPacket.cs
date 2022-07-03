using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class ItemSocketSystemPacket
{
    private enum ItemSocketSystemPacketMode : byte
    {
        UnlockSocket = 0x1,
        SelectUnlockSocketEquip = 0x3,
        UpgradeGem = 0x5,
        SelectGemUpgrade = 0x7,
        MountGem = 0x9,
        ExtractGem = 0xB,
        Notice = 0x12
    }

    public static PacketWriter UnlockSocket(Item item, byte slot, List<GemSocket> unlockedSockets, bool success = true)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ItemSocketSystem);
        pWriter.Write(ItemSocketSystemPacketMode.UnlockSocket);
        pWriter.WriteBool(success);
        pWriter.WriteLong(item.Uid);
        pWriter.WriteByte(slot);
        pWriter.WriteByte(item.GemSockets.Count);
        pWriter.WriteByte((byte) unlockedSockets.Count);
        foreach (GemSocket socket in unlockedSockets)
        {
            pWriter.WriteBool(socket.Gemstone != null);
            if (socket.Gemstone != null)
            {
                pWriter.WriteInt(socket.Gemstone.Id);
                pWriter.WriteLong(socket.Gemstone.OwnerId);
                pWriter.WriteUnicodeString(socket.Gemstone.OwnerName);
                if (socket.Gemstone.IsLocked)
                {
                    pWriter.WriteBool(socket.Gemstone.IsLocked);
                    pWriter.WriteLong(socket.Gemstone.UnlockTime);
                }
            }
        }
        pWriter.WriteByte();
        return pWriter;
    }

    public static PacketWriter SelectGemUpgrade(long unkUid, byte slot, long equipUid)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ItemSocketSystem);
        pWriter.Write(ItemSocketSystemPacketMode.SelectGemUpgrade);
        pWriter.WriteLong(unkUid);
        pWriter.WriteByte(slot);
        pWriter.WriteLong(equipUid);
        pWriter.WriteFloat(100); // Success Rate. Hardcoding the success rate to 100% because the game no longer calculates a rate. it's always 100%
        return pWriter;
    }

    public static PacketWriter SelectUnlockSocketEquip(long equipUid, byte slot, long itemUid)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ItemSocketSystem);
        pWriter.Write(ItemSocketSystemPacketMode.SelectUnlockSocketEquip);
        pWriter.WriteLong(equipUid);
        pWriter.WriteByte(slot);
        pWriter.WriteLong(itemUid);
        pWriter.WriteFloat(100); // Success Rate. Hardcoding the success rate to 100% because the game no longer calculates a rate. it's always 100%
        return pWriter;
    }

    public static PacketWriter UpgradeGem(long equipUid, byte slot, Item item, bool success = true)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ItemSocketSystem);
        pWriter.Write(ItemSocketSystemPacketMode.UpgradeGem);
        pWriter.WriteLong(equipUid);
        pWriter.WriteByte(slot);
        pWriter.WriteBool(success);
        pWriter.WriteByte((byte) item.Rarity);
        pWriter.WriteLong(item.Uid);
        pWriter.WriteByte(1);
        pWriter.WriteInt(item.Id);
        bool isCharBound = item.OwnerCharacterId != 0;
        pWriter.WriteBool(isCharBound);
        if (isCharBound)
        {
            pWriter.WriteLong(item.OwnerCharacterId);
            pWriter.WriteUnicodeString(item.OwnerCharacterName);
        }
        pWriter.WriteBool(item.IsLocked);
        if (item.IsLocked)
        {
            pWriter.WriteBool(item.IsLocked);
            pWriter.WriteLong(item.UnlockTime);
        }
        return pWriter;
    }

    public static PacketWriter MountGem(long equipItemUid, Gemstone gem, byte slot)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ItemSocketSystem);
        pWriter.Write(ItemSocketSystemPacketMode.MountGem);
        pWriter.WriteLong(equipItemUid);
        pWriter.WriteByte(slot);
        pWriter.WriteByte(1);
        pWriter.WriteInt(gem.Id);
        pWriter.WriteBool(gem.OwnerId != 0);
        if (gem.OwnerId != 0)
        {
            pWriter.WriteLong(gem.OwnerId);
            pWriter.WriteUnicodeString(gem.OwnerName);
        }
        pWriter.WriteBool(gem.IsLocked);
        if (gem.IsLocked)
        {
            pWriter.WriteBool(gem.IsLocked);
            pWriter.WriteLong(gem.UnlockTime);
        }
        return pWriter;
    }

    public static PacketWriter ExtractGem(long equipItemUid, long gemItemUid, byte slot)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ItemSocketSystem);
        pWriter.Write(ItemSocketSystemPacketMode.ExtractGem);
        pWriter.WriteLong(equipItemUid);
        pWriter.WriteByte(slot);
        pWriter.WriteLong(gemItemUid);
        return pWriter;
    }

    public static PacketWriter Notice(int noticeId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ItemSocketSystem);
        pWriter.Write(ItemSocketSystemPacketMode.Notice);
        pWriter.WriteByte(); // category?
        pWriter.WriteInt(noticeId);
        return pWriter;
    }
}
