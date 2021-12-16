using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.PacketHandlers.Game;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class BuildModePacket
{
    public static PacketWriter Use(IFieldObject<Player> fieldPlayer, BuildModeHandler.BuildModeType type, int itemId = 0, long itemUid = 0)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.SET_BUILD_MODE);
        pWriter.WriteInt(fieldPlayer.ObjectId);
        pWriter.Write(type);

        switch (type)
        {
            case BuildModeHandler.BuildModeType.House:
                pWriter.WriteInt(itemId);
                pWriter.WriteLong(itemUid);
                pWriter.WriteLong();
                pWriter.WriteByte();
                pWriter.WriteInt();
                break;
            case BuildModeHandler.BuildModeType.Liftables:
                pWriter.WriteInt(itemId);
                pWriter.WriteLong();
                pWriter.WriteLong();
                pWriter.WriteByte();
                pWriter.WriteInt(2);
                break;
        }

        return pWriter;
    }
}
