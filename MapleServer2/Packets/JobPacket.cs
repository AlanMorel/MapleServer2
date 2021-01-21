using System.Collections.Generic;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;
using System;
using MapleServer2.Enums;
using System.Linq;

namespace MapleServer2.Packets
{
    public static class JobPacket
    {
        // Close skill tree
        public static Packet Close()
        {
            // After a save the close sends the same save data again, but this doesn't seem to do anything so instead just write 0 int
            PacketWriter pWriter = PacketWriter.Of(SendOp.JOB);

            pWriter.WriteInt();

            return pWriter;
        }

        // Save skill tree
        public static Packet Save(Player character, int objectId = 0)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.JOB);

            // Identifier info
            pWriter.WriteInt(objectId);
            pWriter.WriteByte(0x09); // Unknown, changes to 08 when closing skill tree after saving
            pWriter.WriteInt((int) character.JobCode);
            pWriter.WriteByte(1); // Possibly always 01 byte
            pWriter.WriteInt((int) character.Job);

            // Skill info
            WriteSkills(pWriter, character);

            return pWriter;
        }

        public static Packet WriteSkills(PacketWriter pWriter, Player character)
        {
            // Get skills
            // Get first skill tab skills only for now, uncertain of how to have multiple skill tabs
            Dictionary<int, SkillMetadata> skills = character.SkillTabs[0].SkillJob;

            // Ordered list of skill ids (must be sent in this order)
            List<int> ids = character.SkillTabs[0].Order;
            byte split = (byte) Enum.Parse<JobSkillSplit>(Enum.GetName(character.Job));
            int countId = ids[ids.Count - split]; // Split to last skill id
            pWriter.WriteByte((byte) (ids.Count - split)); // Skill count minus split

            // List of skills for given tab in format (byte zero) (byte learned) (int skill_id) (int skill_level) (byte zero)
            foreach (int id in ids)
            {
                if (id == countId)
                {
                    pWriter.WriteByte(split); // Write that there are (split) skills left
                }
                pWriter.WriteByte();
                pWriter.WriteByte(skills[id].Learned);
                pWriter.WriteInt(id);
                pWriter.WriteInt(skills[id].SkillLevel.Select(x => x.Level).FirstOrDefault());
                pWriter.WriteByte();
            }
            pWriter.WriteShort(); // Ends with zero short

            return pWriter;
        }
    }
}
