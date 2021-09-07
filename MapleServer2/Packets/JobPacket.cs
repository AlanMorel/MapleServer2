using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Enums;
using MapleServer2.Types;

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
            pWriter.WriteEnum(character.JobCode);
            pWriter.WriteByte(1); // Possibly always 01 byte
            pWriter.WriteEnum(character.Job);

            // Skill info
            pWriter.WriteSkills(character);

            return pWriter;
        }

        public static Packet WriteSkills(this PacketWriter pWriter, Player player)
        {
            SkillTab skillTab = player.SkillTabs.First(x => x.TabId == player.ActiveSkillTabId);
            Dictionary<int, SkillMetadata> skillData = skillTab.SkillJob;
            Dictionary<int, int> skills = skillTab.SkillLevels;

            // Ordered list of skill ids (must be sent in this order)
            List<int> ids = skillTab.Order;

            byte split = (byte) Enum.Parse<JobSkillSplit>(Enum.GetName(player.Job));
            int countId = ids[ids.Count - split]; // Split to last skill id

            if (player.Job == Job.GameMaster)
            {
                pWriter.WriteByte((byte) ids.Count);
            }
            else
            {
                pWriter.WriteByte((byte) (ids.Count - split)); // Skill count minus split
            }

            // List of Skills to display on the client
            foreach (int id in ids)
            {
                if (id == countId)
                {
                    if (player.Job == Job.GameMaster)
                    {
                        pWriter.WriteByte(11);
                    }
                    else
                    {
                        pWriter.WriteByte(split); // Write that there are (split) skills left
                    }
                    
                }
                pWriter.WriteByte();
                try
                {
                    pWriter.WriteBool(skills[id] > 0);  // Is it learned?
                }
                catch {
                    pWriter.WriteBool(false);
                }
                pWriter.WriteInt(id);               // Skill to display
                try
                {
                    pWriter.WriteInt(Math.Clamp(skills[id], skillData[id].SkillLevels.Select(x => x.Level).FirstOrDefault(), int.MaxValue));    // Level to display
                }
                catch
                {
                    pWriter.WriteInt(1);
                }

                pWriter.WriteByte();
            }
            pWriter.WriteShort(); // Ends with zero short

            return pWriter;
        }

        public static Packet WritePassiveSkills(PacketWriter pWriter, IFieldObject<Player> character)
        {
            // The x.Value.Learned == 1 is to filter for now, the skills by level 1 until player can be save on db.
            List<SkillMetadata> passiveSkillList = character.Value.SkillTabs[0].SkillJob.Where(x => x.Value.Type == 1 && x.Value.Learned == 1).Select(x => x.Value).ToList();

            pWriter.WriteShort((short) passiveSkillList.Count); // Passive skills learned count, has to be retrieve from player db.
            // foreach passive skill learned, add it to the player
            for (int i = 0; i < passiveSkillList.Count; i++)
            {
                pWriter.WriteInt(character.ObjectId);
                pWriter.WriteInt(); // unk int
                pWriter.WriteInt(character.ObjectId);
                pWriter.WriteInt(); // unk int 2
                pWriter.WriteInt(); // same as the unk int 2
                pWriter.WriteInt(passiveSkillList[i].SkillId); // Passive skill id
                pWriter.WriteShort((short) passiveSkillList[i].SkillLevels.Select(x => x.Level).FirstOrDefault()); // skill level
                pWriter.WriteInt(1); // unk int = 1
                pWriter.WriteByte(1); // unk byte = 1
                pWriter.WriteLong();
            }
            return pWriter;
        }

        public static Packet SendJob(IFieldObject<Player> character)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.JOB);
            pWriter.WriteInt(character.ObjectId);
            if (character.Value.Job != Job.GameMaster)
            {
                pWriter.WriteByte(2); //2 = second job?
                pWriter.WriteEnum(character.Value.JobCode);
            }

            pWriter.WriteByte(1); //1 = first job?
            pWriter.WriteEnum(character.Value.Job);
            pWriter.WriteSkills(character.Value);

            return pWriter;
        }
    }
}
