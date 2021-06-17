using System.Collections.Generic;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
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

        public static Packet UnlockSocket(Item item, byte slot, List<GemSocket> unlockedSockets, bool success = true)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM_SOCKET_SYSTEM);
            pWriter.WriteEnum(ItemSocketSystemPacketMode.UnlockSocket);
            pWriter.WriteBool(success);
            pWriter.WriteLong(item.Uid);
            pWriter.WriteByte(slot);
            pWriter.WriteByte((byte) item.Stats.GemSockets.Count);
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

        public static Packet SelectGemUpgrade(long unkUid, byte slot, long equipUid)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM_SOCKET_SYSTEM);
            pWriter.WriteEnum(ItemSocketSystemPacketMode.SelectGemUpgrade);
            pWriter.WriteLong(unkUid);
            pWriter.WriteByte(slot);
            pWriter.WriteLong(equipUid);
            pWriter.WriteFloat(100); // Success Rate. Hardcoding the success rate to 100% because the game no longer calculates a rate. it's always 100%
            return pWriter;
        }

        public static Packet SelectUnlockSocketEquip(long equipUid, byte slot, long itemUid)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM_SOCKET_SYSTEM);
            pWriter.WriteEnum(ItemSocketSystemPacketMode.SelectUnlockSocketEquip);
            pWriter.WriteLong(equipUid);
            pWriter.WriteByte(slot);
            pWriter.WriteLong(itemUid);
            pWriter.WriteFloat(100); // Success Rate. Hardcoding the success rate to 100% because the game no longer calculates a rate. it's always 100%
            return pWriter;
        }

        public static Packet UpgradeGem(long equipUid, byte slot, Item item, bool success = true)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM_SOCKET_SYSTEM);
            pWriter.WriteEnum(ItemSocketSystemPacketMode.UpgradeGem);
            pWriter.WriteLong(equipUid);
            pWriter.WriteByte(slot);
            pWriter.WriteBool(success);
            pWriter.WriteByte((byte) item.Rarity);
            pWriter.WriteLong(item.Uid);
            pWriter.WriteByte(1);
            pWriter.WriteInt(item.Id);
            pWriter.WriteBool(item.Owner != null);
            if (item.Owner != null)
            {
                pWriter.WriteLong(item.Owner.CharacterId);
                pWriter.WriteUnicodeString(item.Owner.Name);
            }
            pWriter.WriteBool(item.IsLocked);
            if (item.IsLocked)
            {
                pWriter.WriteBool(item.IsLocked);
                pWriter.WriteLong(item.UnlockTime);
            }
            return pWriter;
        }

        public static Packet MountGem(long equipItemUid, Gemstone gem, byte slot)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM_SOCKET_SYSTEM);
            pWriter.WriteEnum(ItemSocketSystemPacketMode.MountGem);
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

        public static Packet ExtractGem(long equipItemUid, long gemItemUid, byte slot)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM_SOCKET_SYSTEM);
            pWriter.WriteEnum(ItemSocketSystemPacketMode.ExtractGem);
            pWriter.WriteLong(equipItemUid);
            pWriter.WriteByte(slot);
            pWriter.WriteLong(gemItemUid);
            return pWriter;
        }

        public static Packet Notice(int noticeId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM_SOCKET_SYSTEM);
            pWriter.WriteEnum(ItemSocketSystemPacketMode.Notice);
            pWriter.WriteByte(); // category?
            pWriter.WriteInt(noticeId);
            return pWriter;
        }
    }
}
