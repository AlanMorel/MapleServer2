using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Enums;

namespace MapleServer2.Packets;

public static class RockPaperScissorsPacket
{
    private enum MicroGamePacketMode : short
    {
        Open = 0x0,
        RequestMatch = 0x1,
        ConfirmMatch = 0x2,
        DenyMatch = 0x3,
        BeginMatch = 0x4,
        CancelRequestMatch = 0x5,
        Notice = 0x6,
        MatchResults = 0xB,
        ConfirmMatch2 = 0xC
    }

    public static PacketWriter Open()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ROCK_PAPER_SCISSORS);
        pWriter.Write(MicroGamePacketMode.Open);
        return pWriter;
    }

    public static PacketWriter RequestMatch(long characterId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ROCK_PAPER_SCISSORS);
        pWriter.Write(MicroGamePacketMode.RequestMatch);
        pWriter.WriteLong(characterId);
        return pWriter;
    }

    public static PacketWriter ConfirmMatch(long characterId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ROCK_PAPER_SCISSORS);
        pWriter.Write(MicroGamePacketMode.ConfirmMatch);
        pWriter.WriteLong(characterId);
        return pWriter;
    }

    public static PacketWriter DenyMatch(long characterId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ROCK_PAPER_SCISSORS);
        pWriter.Write(MicroGamePacketMode.DenyMatch);
        pWriter.WriteLong(characterId);
        return pWriter;
    }

    public static PacketWriter BeginMatch()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ROCK_PAPER_SCISSORS);
        pWriter.Write(MicroGamePacketMode.BeginMatch);
        return pWriter;
    }

    public static PacketWriter CancelRequestMatch(long characterId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ROCK_PAPER_SCISSORS);
        pWriter.Write(MicroGamePacketMode.CancelRequestMatch);
        pWriter.WriteLong(characterId);
        return pWriter;
    }

    public static PacketWriter Notice(byte noticeId, long characterId = 0)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ROCK_PAPER_SCISSORS);
        pWriter.Write(MicroGamePacketMode.CancelRequestMatch);
        pWriter.WriteLong(characterId);
        pWriter.WriteByte(noticeId);
        return pWriter;
    }

    public static PacketWriter MatchResults(RPSResult result, RockPaperScissorsChoice playerChoice, RockPaperScissorsChoice opponentChoice)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ROCK_PAPER_SCISSORS);
        pWriter.Write(MicroGamePacketMode.MatchResults);
        pWriter.Write(result);
        pWriter.Write(playerChoice);
        pWriter.Write(opponentChoice);
        return pWriter;
    }

    public static PacketWriter ConfirmMatch2(long characterId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ROCK_PAPER_SCISSORS);
        pWriter.Write(MicroGamePacketMode.ConfirmMatch2);
        pWriter.WriteLong(characterId);
        return pWriter;
    }
}
