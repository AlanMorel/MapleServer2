﻿using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets
{
    public static class BreakablePacket
    {
        public static Packet Break(string objectHash, byte flag)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.BREAKABLE)
                .WriteByte(1)
                .WriteMapleString(objectHash)
                .WriteByte(flag)
                .WriteInt()
                .WriteInt() //Unk, trigger id maybe
                .WriteByte();
            return pWriter;
        }
    }
}
