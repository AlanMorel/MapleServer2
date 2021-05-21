﻿using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class ItemExtraDataPacket
    {
        public static Packet Update(IFieldObject<Player> player, Item item)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM_EXTRA_DATA);
            pWriter.WriteInt(player.ObjectId);
            pWriter.WriteLong(item.Uid);
            return pWriter;
        }
    }
}
