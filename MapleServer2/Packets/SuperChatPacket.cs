using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class SuperChatPacket
{
    private enum SuperChatMode : byte
    {
        Select = 0x0,
        Deselect = 0x1
    }

    public static PacketWriter Select(IFieldObject<Player> player, int itemId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.SuperWorldChat);
        pWriter.Write(SuperChatMode.Select);
        pWriter.WriteInt(player.ObjectId);
        pWriter.WriteLong(itemId);
        return pWriter;
    }

    public static PacketWriter Deselect(IFieldObject<Player> player)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.SuperWorldChat);
        pWriter.Write(SuperChatMode.Deselect);
        pWriter.WriteInt(player.ObjectId);
        return pWriter;
    }
}
