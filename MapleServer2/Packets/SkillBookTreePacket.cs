using System.Collections.Generic;
using System.Linq;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class SkillBookTreePacket
    {
        public static Packet Open(Player character)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SKILL_BOOK_TREE);

            // Writes only skills that are learned and for the job rank tab that is opened also doesn't write default passive skills
            pWriter.WriteByte(0);   // Mode (0 = open) (1 = save)
            pWriter.WriteInt(1);    // Possibly always 1
            pWriter.WriteLong(character.SkillTabs[0].Id); // Skill tab id
            pWriter.WriteInt(1);    // Repeat of last int and long
            pWriter.WriteLong(character.SkillTabs[0].Id); // Same as previous identifier
            pWriter.WriteUnicodeString("Build 1"); // Page name

            // Get first skill tab skills only for now, uncertain of how to have multiple skill tabs
            Dictionary<int, int> skills = character.SkillTabs[0].SkillLevels.Where(x => x.Value > 0).ToDictionary(x => x.Key, x => x.Value);
            pWriter.WriteInt(skills.Count); // Skill count
            foreach (KeyValuePair<int, int> p in skills)
            {
                pWriter.WriteInt(p.Key);
                pWriter.WriteInt(p.Value);
            }

            return pWriter;
        }

        public static Packet Save(Player character)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SKILL_BOOK_TREE);

            pWriter.WriteByte(1);   // Mode (0 = open) (1 = save)
            pWriter.WriteLong(character.SkillTabs[0].Id); // Skill tab id
            pWriter.WriteLong(character.SkillTabs[0].Id); // Skill tab id
            pWriter.WriteInt(2);    // Set Client Mode (1 = unsaved points, 2 = no unsaved points)

            return pWriter;
        }

        public static Packet AddTab(Player character)
        {
            PacketWriter pWriter = new PacketWriter();
            return pWriter;
        }
    }
}
