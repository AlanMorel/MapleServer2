using Maple2Storage.Types;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;

namespace MapleServer2.Types;

public class RegionSkillHandler
{
    public static void Handle(GameSession session, int sourceId, CoordF coords, SkillCast skillCast)
    {
        session.FieldManager.BroadcastPacket(RegionSkillPacket.Send(sourceId, coords.ToShort(), skillCast));
        Remove(session, skillCast, sourceId);
    }

    private static void Remove(GameSession session, SkillCast skillCast, int sourceId)
    {
        Task.Run(async () =>
        {
            // TODO: Get the correct Region Skill Duration when calling chain Skills
            await Task.Delay(skillCast.DurationTick() + 5000);
            session.FieldManager.BroadcastPacket(RegionSkillPacket.Remove(sourceId));
        });
    }
}
