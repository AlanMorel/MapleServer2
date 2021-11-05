using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Enums;

namespace MapleServer2.Packets;

public class HandshakePacket
{
    public static PacketWriter Handshake(uint version, uint riv, uint siv, uint blockIv, PatchType patchType, int handshakeSize)
    {
        PacketWriter handshake = PacketWriter.Of(SendOp.REQUEST_VERSION, handshakeSize);
        handshake.Write(version);
        handshake.Write(riv);
        handshake.Write(siv);
        handshake.Write(blockIv);
        handshake.Write(patchType);

        return handshake;
    }
}
