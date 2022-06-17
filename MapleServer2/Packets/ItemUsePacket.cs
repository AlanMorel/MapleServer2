using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.PacketHandlers.Game;

namespace MapleServer2.Packets;

public static class ItemUsePacket
{
    public static PacketWriter Use(int id, int amount, OpenBoxResult openBoxResult)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ItemUse);

        pWriter.WriteInt(id);
        pWriter.WriteInt(amount);
        pWriter.Write(openBoxResult);

        return pWriter;
    }
}
