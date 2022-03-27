using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets;

public static class RequestPacket
{
    public static PacketWriter Login()
    {
        return PacketWriter.Of(SendOp.RequestLogin);
    }

    public static PacketWriter Key()
    {
        return PacketWriter.Of(SendOp.RequestKey);
    }

    public static PacketWriter Heartbeat(int key)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.RequestHeartBeat);
        pWriter.WriteInt(key);

        return pWriter;
    }

    public static PacketWriter TickSync()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.RequestClientTickSync);
        pWriter.WriteInt(Environment.TickCount);

        return pWriter;
    }
}
