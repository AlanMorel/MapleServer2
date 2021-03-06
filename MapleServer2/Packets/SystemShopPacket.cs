﻿using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets
{
    public static class SystemShopPacket
    {
        private enum SystemShopPacketMode : byte
        {
            Open = 0x0A,
        }

        public static Packet Open()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SYSTEM_SHOP);
            pWriter.WriteEnum(SystemShopPacketMode.Open);
            pWriter.WriteByte(0x1);
            return pWriter;
        }
    }
}
