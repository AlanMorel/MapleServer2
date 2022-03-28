using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class GameEventUserValuePacket
{
    private enum GameEventUserValuePacketMode : byte
    {
        LoadValues = 0x0,
        UpdateValue = 0x1,
        Notice = 0x9,
    }

    public static PacketWriter LoadValues(List<GameEventUserValue> userValues)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.GameEventUserValue);
        pWriter.Write(GameEventUserValuePacketMode.LoadValues);
        pWriter.WriteByte();
        pWriter.WriteInt(userValues.Count);

        foreach (GameEventUserValue userValue in userValues)
        {
            WriteUserValue(pWriter, userValue);
        }
        return pWriter;
    }

    public static PacketWriter UpdateValue(GameEventUserValue userValue)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.GameEventUserValue);
        pWriter.Write(GameEventUserValuePacketMode.UpdateValue);
        pWriter.WriteByte();
        WriteUserValue(pWriter, userValue);
        return pWriter;

    }

    private static void WriteUserValue(PacketWriter pWriter, GameEventUserValue userValue)
    {
        pWriter.Write(userValue.EventType);
        pWriter.WriteInt(userValue.EventId);
        pWriter.WriteUnicodeString(userValue.EventValue);
        pWriter.WriteLong(userValue.ExpirationTimestamp);
    }
}
