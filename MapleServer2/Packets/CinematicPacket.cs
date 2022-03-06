using Maple2.Trigger.Enum;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets;

public static class CinematicPacket
{
    private enum CinematicPacketMode : byte
    {
        HideUi = 0x1,
        Mode02 = 0x2,
        View = 0x3,
        SetSceneSkip = 0x4,
        StartSceneSkip = 0x5,
        Conversation = 0x6,
        BalloonTalk = 0x8,
        Caption = 0xA,
        SystemMsg = 0xB
    }

    public static PacketWriter HideUi(bool hide)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.CINEMATIC);
        pWriter.Write(CinematicPacketMode.HideUi);
        pWriter.WriteBool(hide);
        return pWriter;
    }

    public static PacketWriter Mode02()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.CINEMATIC);
        pWriter.Write(CinematicPacketMode.Mode02);
        return pWriter;
    }

    public static PacketWriter View(int type)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.CINEMATIC);
        pWriter.Write(CinematicPacketMode.View);
        pWriter.WriteInt(type);
        pWriter.WriteUnicodeString();
        pWriter.WriteUnicodeString();
        return pWriter;
    }

    public static PacketWriter SetSceneSkip(string skipState = "")
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.CINEMATIC);
        pWriter.Write(CinematicPacketMode.SetSceneSkip);
        pWriter.WriteBool(skipState.Length != 0);
        pWriter.WriteString(skipState);
        return pWriter;
    }

    public static PacketWriter StartSceneSkip()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.CINEMATIC);
        pWriter.Write(CinematicPacketMode.StartSceneSkip);
        return pWriter;
    }

    public static PacketWriter Conversation(int npcId, string illustrationId, string stringId, int delay, Align align)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.CINEMATIC);
        pWriter.Write(CinematicPacketMode.Conversation);
        pWriter.WriteInt(npcId);
        pWriter.WriteString(illustrationId);
        pWriter.WriteUnicodeString(stringId);
        pWriter.WriteInt(delay);
        pWriter.Write(align);
        return pWriter;
    }

    public static PacketWriter BalloonTalk(int objectId, bool isNpcId, string msg, int duration, int delayTick)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.CINEMATIC);
        pWriter.Write(CinematicPacketMode.BalloonTalk);
        pWriter.WriteBool(isNpcId);
        pWriter.WriteInt(objectId);
        pWriter.WriteUnicodeString(msg);
        pWriter.WriteInt(duration);
        pWriter.WriteInt(delayTick);
        return pWriter;
    }

    public static PacketWriter Caption(CaptionType type, string title, string script, string align, float offsetRateX, float offsetRateY, int duration,
        float scale)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.CINEMATIC);
        pWriter.Write(CinematicPacketMode.Caption);
        pWriter.WriteUnicodeString(type + "Caption");
        pWriter.WriteUnicodeString(title);
        pWriter.WriteUnicodeString(script);
        pWriter.WriteUnicodeString(align);
        pWriter.WriteInt(duration);
        pWriter.WriteFloat(offsetRateX);
        pWriter.WriteFloat(offsetRateY);
        pWriter.WriteFloat(scale);
        return pWriter;
    }

    public static PacketWriter SystemMessage(string script)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.CINEMATIC);
        pWriter.Write(CinematicPacketMode.SystemMsg);
        pWriter.WriteUnicodeString(script);
        pWriter.WriteByte();

        return pWriter;
    }
}
