using System.Collections.Immutable;
using System.Net;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets
{
    public static class ServerListPacket
    {
        public static Packet SetServers(string serverName, ImmutableList<IPEndPoint> serverIps)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SERVER_LIST);

            pWriter.WriteByte(1); // Required
            pWriter.WriteInt(0); // Unk.
            pWriter.WriteUnicodeString(serverName);
            pWriter.WriteByte(0); // Unk.

            pWriter.WriteUShort((ushort) serverIps.Count);
            foreach (IPEndPoint endpoint in serverIps)
            {
                pWriter.WriteUnicodeString(endpoint.Address.ToString());
                pWriter.WriteUShort((ushort) endpoint.Port);
            }

            pWriter.WriteInt(0); // 100 in sniff

            short nMultiServerChannel = 1; // Need at least 1
            for (short i = 0; i < nMultiServerChannel; ++i)
            {
                pWriter.WriteShort(i);
            }

            return pWriter;
        }
    }
}
