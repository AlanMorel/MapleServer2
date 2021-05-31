using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets
{
    public static class ItemSocketSystemPacket
    {
        private enum ItemSocketSystemPacketMode : byte
        {
            UpgradeGem = 0x5,
            SelectGemUpgrade = 0x7,
        }

        public static Packet SelectGemUpgrade(long itemUid)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM_SOCKET_SYSTEM);
            pWriter.WriteEnum(ItemSocketSystemPacketMode.SelectGemUpgrade);
            pWriter.WriteLong();
            pWriter.WriteByte(0xFF);
            pWriter.WriteLong(itemUid);
            pWriter.WriteFloat(100); // Success Rate. Hardcoding the success rate to 100% because the game no longer calculates a rate. it's always 100%
            return pWriter;
        }

        public static Packet UpgradeGem(bool success)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM_SOCKET_SYSTEM);
            pWriter.WriteEnum(ItemSocketSystemPacketMode.UpgradeGem);
            pWriter.WriteLong();
            pWriter.Write(0xFF);
            pWriter.WriteBool(success);
            return pWriter;
        }
    }
}
