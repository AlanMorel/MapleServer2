using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets;

public class MushkingRoyaleSystemPacket
{
    private enum MushkingRoyaleSystemPacketMode : byte
    {
        MatchFound = 0x2,
        Results = 0x11,
        LastStandingNotice = 0x14,
        LoadStats = 0x17,
        NewSeasonNotice = 0x18,
        KillNotices = 0x19,
        UpdateKills = 0x1A,
        SurvivalSessionStats = 0x1B,
        Poisoned = 0x1D,
        ClaimRewards = 0x23
    }

    public static PacketWriter MatchFound()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MUSHKING_ROYALE_SYSTEM);
        pWriter.Write(MushkingRoyaleSystemPacketMode.MatchFound);
        pWriter.WriteLong(); // match ID
        pWriter.WriteBool(false); // false = 15 second window, true = 5 second window
        return pWriter;
    }

    public static PacketWriter Results()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MUSHKING_ROYALE_SYSTEM);
        pWriter.Write(MushkingRoyaleSystemPacketMode.Results);
        pWriter.WriteInt();
        pWriter.WriteInt();
        pWriter.WriteInt(); // previous points
        pWriter.WriteInt(); // current points
        pWriter.WriteInt(); // total players
        pWriter.WriteInt(); // rank in match
        pWriter.WriteInt();
        pWriter.WriteInt();
        pWriter.WriteInt(); // royale exp
        pWriter.WriteInt();
        pWriter.WriteInt(); // royale level
        pWriter.WriteInt();
        pWriter.WriteInt(); // exp
        pWriter.WriteInt(); // prestige exp
        pWriter.WriteInt(); // survival time
        pWriter.WriteInt(); // kills
        pWriter.WriteInt();
        pWriter.WriteInt();
        pWriter.WriteInt();
        pWriter.WriteInt();
        pWriter.WriteByte(); // item reward count 
        // item rewards loop
        pWriter.WriteInt();
        pWriter.WriteShort();
        pWriter.WriteInt();
        pWriter.WriteByte();
        pWriter.WriteByte();
        pWriter.WriteByte();
        // end loop
        return pWriter;
    }

    public static PacketWriter LastStandingNotice()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MUSHKING_ROYALE_SYSTEM);
        pWriter.Write(MushkingRoyaleSystemPacketMode.Results);
        pWriter.WriteByte();
        pWriter.WriteInt(); // amount of users
        // start loop
        pWriter.WriteLong(); // characterID
        // end loop
        return pWriter;

    }

    public static PacketWriter LoadStats(long accountId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MUSHKING_ROYALE_SYSTEM);
        pWriter.Write(MushkingRoyaleSystemPacketMode.LoadStats);
        pWriter.WriteLong(accountId);
        pWriter.WriteInt();
        pWriter.WriteByte();
        pWriter.WriteLong(); // exp
        pWriter.WriteInt(1); // royal level
        pWriter.WriteInt();
        pWriter.WriteInt();
        pWriter.WriteLong(); // exp gained

        return pWriter;
    }

    public static PacketWriter NewSeasonNotice()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MUSHKING_ROYALE_SYSTEM);
        pWriter.Write(MushkingRoyaleSystemPacketMode.NewSeasonNotice);
        return pWriter;
    }

    public static PacketWriter SurvivalSessionStats()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MUSHKING_ROYALE_SYSTEM);
        pWriter.Write(MushkingRoyaleSystemPacketMode.SurvivalSessionStats);
        pWriter.WriteInt(); // players remaining
        pWriter.WriteInt(); // total players
        pWriter.WriteByte();
        return pWriter;
    }

    public static PacketWriter UpdateKills()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MUSHKING_ROYALE_SYSTEM);
        pWriter.Write(MushkingRoyaleSystemPacketMode.UpdateKills);
        pWriter.WriteInt(); // kill count
        return pWriter;
    }

    public static PacketWriter Poisoned()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MUSHKING_ROYALE_SYSTEM);
        pWriter.Write(MushkingRoyaleSystemPacketMode.Poisoned);
        return pWriter;
    }

    public static PacketWriter ClaimRewards()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MUSHKING_ROYALE_SYSTEM);
        pWriter.Write(MushkingRoyaleSystemPacketMode.ClaimRewards);
        pWriter.WriteInt(); // silver reward claim start level
        pWriter.WriteInt(); // silver reward claim end level
        pWriter.WriteInt(); // gold reward claim start level
        pWriter.WriteInt(); // gold reward claim end level
        return pWriter;

    }
}
