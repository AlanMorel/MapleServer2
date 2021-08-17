using Maple2Storage.Types;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;

namespace MapleServer2.Types
{
    public class RegionSkillHandler
    {
        public static void Handle(GameSession session, int sourceId, CoordF coord, SkillCast skillCast)
        {
            session.Send(RegionSkillPacket.Send(sourceId, coord.ToShort(), skillCast));
            Remove(session, skillCast, sourceId);
        }

        private static Task Remove(GameSession session, SkillCast skillCast, int sourceId)
        {
            return Task.Run(async () =>
            {
                // TODO: Get the correct Region Skill Duration when calling chain Skills
                await Task.Delay(skillCast.DurationTick() + 2000);
                session.Send(RegionSkillPacket.Remove(sourceId));
            });
        }
    }
}
