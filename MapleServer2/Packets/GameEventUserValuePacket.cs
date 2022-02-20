using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Enums;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class GameEventUserValuePacket
{
    private enum GameEventUserValuePacketMode : byte
    {
        LoadValues = 0x0,
        UpdateValue = 0x1,
    }

    public static PacketWriter LoadValues(List<GameEventUserValue> userValues)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.GAME_EVENT_USER_VALUE);
        pWriter.Write(GameEventUserValuePacketMode.LoadValues);
        pWriter.WriteByte();
        pWriter.WriteInt(userValues.Count); // loop count

        foreach (GameEventUserValue userValue in userValues)
        {
            pWriter.Write(userValue.Type);
            pWriter.WriteInt(userValue.EventId);
            pWriter.WriteUnicodeString(userValue.EventValue);
            pWriter.WriteLong(userValue.ExpirationTimestamp);
        }

        return pWriter;
    }
}
