using System.Collections.Immutable;
using System.Net;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets;

public static class ServerListPacket
{
    public static PacketWriter SetServers(string serverName, ImmutableList<IPEndPoint> serverIps, short channelCount)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ServerList);

        pWriter.WriteBool(true); // If false packet isn't processed
        pWriter.WriteInt(1); // Unk.
        pWriter.WriteUnicodeString(serverName);
        pWriter.WriteByte(); // Unk.

        pWriter.Write((ushort) serverIps.Count);
        foreach (IPEndPoint endpoint in serverIps)
        {
            pWriter.WriteUnicodeString(endpoint.Address.ToString());
            pWriter.Write((ushort) endpoint.Port);
        }

        pWriter.WriteInt(100); // Unk.

        pWriter.WriteShort(channelCount);
        for (short i = 1; i <= channelCount; ++i)
        {
            pWriter.WriteShort(i);
        }

        return pWriter;
    }
}
