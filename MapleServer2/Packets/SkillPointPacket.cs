using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Enums;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class SkillPointPacket
{
    public static PacketWriter ExtraSkillPoints(Player character)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.SkillPoint);
        pWriter.WriteInt(character.StatPointDistribution.TotalExtraSkillPoints); // total points
        pWriter.WriteInt(character.StatPointDistribution.ExtraSkillPoints.Count); // source count
        foreach ((SkillPointSource source, ExtraSkillPoints extraPoints) in character.StatPointDistribution.ExtraSkillPoints)
        {
            pWriter.WriteInt((int) source); // source type
            pWriter.WriteInt(extraPoints.ExtraPoints.Count); // count
            foreach (KeyValuePair<short, int> reward in extraPoints.ExtraPoints)
            {
                pWriter.WriteShort(reward.Key); // job rank
                pWriter.WriteInt(reward.Value); // points
            }
        }

        return pWriter;
    }
}
