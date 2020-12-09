using System.Collections.Generic;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Constants.Skills;
using MapleServer2.Types;

namespace MapleServer2.Packets {
    public static class JobPacket {
        // Close skill tree
        public static Packet Close() {
            // After a save the close sends the same save data again, but this doesn't seem to do anything so instead just write 0 int
            var pWriter = PacketWriter.Of(SendOp.JOB);

            pWriter.WriteInt();

            return pWriter;
        }

        // Save skill tree
        public static Packet Save(Player character, int objectId = 0) {
            var pWriter = PacketWriter.Of(SendOp.JOB);

            // Identifier info
            pWriter.WriteInt(objectId);
            pWriter.WriteByte(0x09); // Unknown, changes to 08 when closing skill tree after saving
            pWriter.WriteInt(character.JobId);
            pWriter.WriteByte(1); // Possibly always 01 byte
            pWriter.WriteInt(character.JobGroupId);

            // Skill info
            WriteSkills(pWriter, character);

            return pWriter;
        }

        public static Packet WriteSkills(PacketWriter pWriter, Player character) {
            // Get skills
            Dictionary<int, Skill> skills = character.SkillTabs[0].GetSkills(); // Get first skill tab skills only for now, uncertain of how to have multiple skill tabs

            // Ordered list of skill ids (must be sent in this order)
            int[] ids = JobSkillOrder.Order[character.JobGroupId];
            int countId = ids[ids.Length - 8]; // 8th to last skill id

            pWriter.WriteByte((byte)(ids.Length - 8)); // Skill count minus 8

            // List of skills for given tab in format (byte zero) (byte learned) (int skill_id) (int skill_level) (byte zero)
            foreach (int id in ids) {
                if (id == countId) pWriter.WriteByte(8); // Write that there are 8 skills left
                pWriter.WriteByte();
                pWriter.WriteByte(skills[id].Learned);
                pWriter.WriteInt(id);
                pWriter.WriteInt((int)skills[id].Level);
                pWriter.WriteByte();
            }

            pWriter.WriteShort(); // Ends with zero short

            return pWriter;
        }
    }
}