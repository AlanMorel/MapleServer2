using Maple2Storage.Enums;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public class MushkingRoyaleSystemPacket
{
    private enum MushkingRoyaleSystemPacketMode : byte
    {
        JoinSoloQueue = 0x0,
        WithdrawSoloQueue = 0x1,
        MatchFound = 0x2,
        Results = 0x11,
        LastStandingNotice = 0x14,
        Unk16 = 0x16,
        LoadStats = 0x17,
        NewSeasonNotice = 0x18,
        KillNotices = 0x19,
        UpdateKills = 0x1A,
        SurvivalSessionStats = 0x1B,
        Poisoned = 0x1D,
        LoadMedals = 0x1E,
        ClaimRewards = 0x23
    }

    public static PacketWriter JoinSoloQueue()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MUSHKING_ROYALE_SYSTEM);
        pWriter.Write(MushkingRoyaleSystemPacketMode.JoinSoloQueue);
        return pWriter;
    }

    public static PacketWriter WithdrawSoloQueue()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MUSHKING_ROYALE_SYSTEM);
        pWriter.Write(MushkingRoyaleSystemPacketMode.WithdrawSoloQueue);
        return pWriter;
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

    public static PacketWriter Unk16()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MUSHKING_ROYALE_SYSTEM);
        pWriter.Write(MushkingRoyaleSystemPacketMode.Unk16);
        pWriter.WriteInt();
        pWriter.WriteInt(1);
        pWriter.WriteLong();
        pWriter.WriteUnicodeString();
        pWriter.WriteInt();
        pWriter.WriteInt();
        pWriter.WriteInt();
        pWriter.WriteInt();
        pWriter.WriteInt();
        pWriter.WriteInt();
        pWriter.WriteInt();
        return pWriter;
    }

    public static PacketWriter LoadStats(MushkingRoyaleStats stats, long expGained = 0)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MUSHKING_ROYALE_SYSTEM);
        pWriter.Write(MushkingRoyaleSystemPacketMode.LoadStats);
        pWriter.WriteLong(stats.Id);
        pWriter.WriteInt();
        pWriter.WriteBool(stats.IsGoldPassActive);
        pWriter.WriteLong(stats.Exp);
        pWriter.WriteInt(stats.Level);
        pWriter.WriteInt(stats.SilverLevelClaimedRewards);
        pWriter.WriteInt(stats.GoldLevelClaimedRewards);
        pWriter.WriteLong(expGained);

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

    public static PacketWriter LoadMedals(Account account)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MUSHKING_ROYALE_SYSTEM);
        pWriter.Write(MushkingRoyaleSystemPacketMode.LoadMedals);
        pWriter.WriteByte((byte) Enum.GetNames(typeof(MedalSlot)).Length);
        foreach (MedalSlot slot in Enum.GetValues(typeof(MedalSlot)))
        {
            pWriter.WriteInt(account.EquippedMedals[slot]?.EffectId ?? 0);
            List<Medal> medals = account.Medals.Where(x => x.Slot == slot).ToList();
            pWriter.WriteInt(medals.Count);
            foreach (Medal medal in medals)
            {
                pWriter.WriteInt(medal.EffectId);
                pWriter.WriteLong(medal.ExpirationTimeStamp);
            }
        }
        return pWriter;
    }

    public static PacketWriter ClaimRewards(int silverStartLevel, int silverEndLevel, int goldStartLevel, int goldEndLevel)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MUSHKING_ROYALE_SYSTEM);
        pWriter.Write(MushkingRoyaleSystemPacketMode.ClaimRewards);
        pWriter.WriteInt(silverStartLevel);
        pWriter.WriteInt(silverEndLevel);
        pWriter.WriteInt(goldStartLevel);
        pWriter.WriteInt(goldEndLevel);
        return pWriter;
    }
}
