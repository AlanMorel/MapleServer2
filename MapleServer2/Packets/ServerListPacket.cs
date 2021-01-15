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
            pWriter.Write(01, 01, 00, 00, 00);
            pWriter.WriteUnicodeString(serverName);
            pWriter.WriteByte(4); // IPv4?
            pWriter.WriteUShort((ushort) serverIps.Count);
            foreach (IPEndPoint endpoint in serverIps)
            {
                pWriter.WriteUnicodeString(endpoint.Address.ToString());
                pWriter.WriteUShort((ushort) endpoint.Port);
            }
            pWriter.WriteInt(100); // Const
            // Looks like length 9, then 1-9 in scrambled order
            pWriter.Write(09, 00, 01, 00, 04, 00, 07, 00, 02, 00, 05, 00, 08, 00, 03, 00, 06, 00, 09, 00);

            return pWriter;
        }
    }
}
