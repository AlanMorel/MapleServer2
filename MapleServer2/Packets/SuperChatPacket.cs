﻿using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{

    public static class SuperChatPacket
    {
        private enum SuperChatMode : byte
        {
            Select = 0x0,
            Deselect = 0x1,
        }

        public static Packet Select(IFieldObject<Player> player, int itemId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SUPER_WORLDCHAT);
            pWriter.WriteEnum(SuperChatMode.Select);
            pWriter.WriteInt(player.ObjectId);
            pWriter.WriteLong(itemId);
            return pWriter;
        }

        public static Packet Deselect(IFieldObject<Player> player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SUPER_WORLDCHAT);
            pWriter.WriteEnum(SuperChatMode.Deselect);
            pWriter.WriteInt(player.ObjectId);
            return pWriter;
        }
    }
}
