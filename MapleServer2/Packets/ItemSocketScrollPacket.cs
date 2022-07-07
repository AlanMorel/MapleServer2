using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets.Helpers;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class ItemSocketScrollPacket
{
    private enum Mode : byte
    {
        OpenWindow = 0x0,
        UseScroll = 0x2,
        Error = 0x3
    }

    public static PacketWriter OpenWindow(long itemUid, int successRate, byte socketCount)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ItemSocketScroll);
        pWriter.Write(Mode.OpenWindow);
        pWriter.WriteLong(itemUid);
        pWriter.WriteInt(successRate);
        pWriter.WriteByte(socketCount);
        return pWriter;
    }

    public static PacketWriter UseScroll(Item item, int successRate, bool success)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ItemSocketScroll);
        pWriter.Write(Mode.UseScroll);
        pWriter.WriteBool(success);
        pWriter.WriteLong(item.Uid);
        pWriter.WriteByte();
        pWriter.WriteInt(successRate);
        pWriter.WriteSockets(item.Stats, item.GemSockets.Sockets);
        return pWriter;
    }

    public static PacketWriter Error(int errorId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ItemSocketScroll);
        pWriter.Write(Mode.Error);
        pWriter.WriteByte();
        pWriter.WriteInt(errorId);
        return pWriter;
    }
}
