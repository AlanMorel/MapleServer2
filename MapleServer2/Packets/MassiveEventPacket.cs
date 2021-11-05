using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Enums;

namespace MapleServer2.Packets;

public static class MassiveEventPacket
{
    private enum MassiveEventPacketMode : byte
    {
        RoundBar = 0x0,
        RoundBanner = 0x1,
        TextBanner = 0x2,
        RoundHeader = 0x7,
        PrepareCountdown = 0x8
    }

    public static PacketWriter RoundBar(int currentRound, int lastRound, int startFromRound)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MASSIVE_EVENT);
        pWriter.Write(MassiveEventPacketMode.RoundBar);
        pWriter.WriteInt(currentRound);
        pWriter.WriteInt(lastRound);
        pWriter.WriteInt(startFromRound);
        pWriter.WriteInt();
        return pWriter;
    }

    public static PacketWriter Round(string text, int round, int countFrom, int soundType = 0)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MASSIVE_EVENT);
        pWriter.Write(MassiveEventPacketMode.RoundBanner);
        pWriter.WriteUnicodeString(text);
        pWriter.WriteInt(round);
        pWriter.WriteInt(countFrom);
        pWriter.WriteInt(soundType);
        return pWriter;
    }

    public static PacketWriter TextBanner(EventBannerType type, string script, int duration)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MASSIVE_EVENT);
        pWriter.Write(MassiveEventPacketMode.TextBanner);
        pWriter.Write(type);
        pWriter.WriteUnicodeString(script);
        pWriter.WriteInt(duration);
        return pWriter;
    }

    public static PacketWriter RoundHeader(int round, bool finalRound, int duration)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MASSIVE_EVENT);
        pWriter.Write(MassiveEventPacketMode.RoundHeader);
        pWriter.WriteInt(round);
        pWriter.WriteBool(finalRound);
        pWriter.WriteInt(duration);
        return pWriter;
    }

    public static PacketWriter PrepareCountdown(int countFrom)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MASSIVE_EVENT);
        pWriter.Write(MassiveEventPacketMode.PrepareCountdown);
        pWriter.WriteInt(countFrom);
        return pWriter;
    }
}
