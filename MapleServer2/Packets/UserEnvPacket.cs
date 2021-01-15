using System.Collections.Generic;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class UserEnvPacket
    {
        // Unlocked Titles
        public static Packet SetTitles(Player player)
        {
            List<int> titleIds = player.Titles;

            PacketWriter pWriter = PacketWriter.Of(SendOp.USER_ENV);
            pWriter.WriteByte(0x02);
            pWriter.WriteInt(titleIds.Count);
            foreach (int titleId in titleIds)
            {
                pWriter.WriteInt(titleId);
            }

            return pWriter;
        }

        public static Packet UpdateTitle(GameSession session, int titleId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.USER_ENV);
            pWriter.WriteByte(01); // Mode update
            pWriter.WriteInt(session.FieldPlayer.ObjectId);
            pWriter.WriteInt(titleId);
            return pWriter;
        }

        public static Packet Send03()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.USER_ENV);
            pWriter.WriteByte(0x03);
            pWriter.WriteInt();
            // Loop: Int + Byte

            return pWriter;
        }

        public static Packet Send04()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.USER_ENV);
            pWriter.WriteByte(0x04);
            pWriter.WriteInt();
            // Loop: Int

            return pWriter;
        }

        public static Packet Send05()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.USER_ENV);
            pWriter.WriteByte(0x05);
            pWriter.WriteInt();

            return pWriter;
        }

        public static Packet Send08()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.USER_ENV);
            pWriter.WriteByte(0x08);
            pWriter.WriteInt();
            // Loop: Int + Int
            pWriter.WriteInt();
            // Loop: Int + Int

            return pWriter;
        }

        public static Packet Send09()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.USER_ENV);
            pWriter.WriteByte(0x09);
            pWriter.WriteInt();
            // Loop: Int + Byte

            return pWriter;
        }

        public static Packet Send10()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.USER_ENV);
            pWriter.WriteByte(0x0A);
            pWriter.WriteInt();
            // Loop: Short + Byte

            return pWriter;
        }

        public static Packet Send11()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.USER_ENV);
            pWriter.WriteByte(0x0B);
            pWriter.WriteShort();
            pWriter.WriteInt();

            return pWriter;
        }

        public static Packet Send12()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.USER_ENV);
            pWriter.WriteByte(0x0C);
            pWriter.WriteInt();
            // Loop: Int + Long

            return pWriter;
        }

        public static Packet Send13()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.USER_ENV);
            pWriter.WriteByte(0x0D);
            pWriter.WriteInt();
            pWriter.WriteLong();

            return pWriter;
        }
    }
}
