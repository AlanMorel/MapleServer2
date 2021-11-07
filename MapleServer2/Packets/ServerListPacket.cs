using System.Collections.Immutable;
using System.Net;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets;

public static class ServerListPacket
{
    public static PacketWriter SetServers(string serverName, ImmutableList<IPEndPoint> serverIps)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.SERVER_LIST);

        pWriter.WriteByte(1); // If false packet isn't processed
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

        short nMultiServerChannel = 10; // Doesn't seems to go past 9 channels ingame ??
        pWriter.WriteShort(nMultiServerChannel);
        for (short i = 1; i <= nMultiServerChannel; ++i)
        {
            pWriter.WriteShort(i);
        }

        return pWriter;
    }
}
