using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class SkillBookTreePacket
    {
        private enum SkillBookTreeMode : byte
        {
            Open = 0x00,
            Save = 0x01,
            Rename = 0x02,
            AddTab = 0x04
        }

        public static PacketWriter Open(Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SKILL_BOOK_TREE);

            // Writes only skills that are learned and for the job rank tab that is opened also doesn't write default passive skills
            pWriter.Write(SkillBookTreeMode.Open);
            pWriter.WriteInt(player.SkillTabs.Count);
            pWriter.WriteLong(player.ActiveSkillTabId);
            pWriter.WriteInt(player.SkillTabs.Count);
            foreach (SkillTab skillTab in player.SkillTabs)
            {
                pWriter.WriteLong(skillTab.TabId);
                pWriter.WriteUnicodeString(skillTab.Name);

                Dictionary<int, short> skills = skillTab.SkillLevels.Where(x => x.Value > 0).ToDictionary(x => x.Key, x => x.Value);
                pWriter.WriteInt(skills.Count);
                foreach (KeyValuePair<int, short> p in skills)
                {
                    pWriter.WriteInt(p.Key);
                    pWriter.WriteInt(p.Value);
                }
            }

            return pWriter;
        }

        public static PacketWriter Save(Player player, long selectedTab)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SKILL_BOOK_TREE);

            pWriter.Write(SkillBookTreeMode.Save);
            pWriter.WriteLong(player.ActiveSkillTabId);
            pWriter.WriteLong(selectedTab);
            pWriter.WriteInt(2); // Set Client Mode (1 = unsaved points, 2 = no unsaved points)

            return pWriter;
        }

        public static PacketWriter Rename(long id, string name)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SKILL_BOOK_TREE);
            pWriter.Write(SkillBookTreeMode.Rename);
            pWriter.WriteLong(id);
            pWriter.WriteUnicodeString(name);
            pWriter.WriteByte();

            return pWriter;
        }

        public static PacketWriter AddTab(Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SKILL_BOOK_TREE);

            pWriter.Write(SkillBookTreeMode.AddTab);
            pWriter.WriteInt(2);
            pWriter.WriteLong(player.ActiveSkillTabId);

            return pWriter;
        }
    }
}
