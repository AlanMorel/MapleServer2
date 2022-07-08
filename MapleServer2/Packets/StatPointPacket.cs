using Maple2Storage.Enums;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Enums;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class StatPointPacket
{
    private enum Mode : byte
    {
        TotalPoints = 0x0,
        StatDistribution = 0x1
    }

    // packet which represents the total number of stat points gained and
    // how each stat point was obtained (ie quest, trophy, exploration, prestige)
    public static PacketWriter WriteTotalStatPoints(Player character)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.StatPoint);

        pWriter.Write(Mode.TotalPoints);
        pWriter.WriteInt(character.StatPointDistribution.TotalStatPoints);
        pWriter.WriteInt(character.StatPointDistribution.OtherStats.Count);

        foreach ((OtherStatsIndex statIndex, int value) in character.StatPointDistribution.OtherStats)
        {
            pWriter.WriteInt((int) statIndex);
            pWriter.WriteInt(value);
        }

        return pWriter;
    }

    // packet that updates or loads the character's current stat distribution
    public static PacketWriter WriteStatPointDistribution(Player character)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.StatPoint);

        pWriter.Write(Mode.StatDistribution);
        pWriter.WriteInt(character.StatPointDistribution.TotalStatPoints);
        pWriter.WriteInt(character.StatPointDistribution.GetStatTypeCount());

        foreach ((StatAttribute statType, int amount) in character.StatPointDistribution.AllocatedStats)
        {
            pWriter.WriteByte((byte) statType);
            pWriter.WriteInt(amount);
        }

        return pWriter;
    }
}
