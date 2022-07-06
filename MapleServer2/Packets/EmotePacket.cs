using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class EmotePacket
{
    private enum Mode : byte
    {
        LoadEmotes = 0x0,
        LearnEmote = 0x1
    }

    public static PacketWriter LoadEmotes(Player player)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Emotion);
        pWriter.Write(Mode.LoadEmotes);
        pWriter.WriteInt(player.Emotes.Count);
        foreach (int emoteId in player.Emotes)
        {
            pWriter.WriteInt(emoteId);
            pWriter.WriteInt(1);
            pWriter.WriteLong();
        }
        return pWriter;
    }

    public static PacketWriter LearnEmote(int emoteId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Emotion);
        pWriter.Write(Mode.LearnEmote);
        pWriter.WriteInt(emoteId);
        pWriter.WriteInt(1); // quantity
        pWriter.WriteLong();
        return pWriter;
    }
}
