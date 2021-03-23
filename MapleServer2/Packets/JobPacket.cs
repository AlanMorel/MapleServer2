﻿using System;
using System.Collections.Generic;
using System.Linq;
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

        public static Packet WriteSkills(this PacketWriter pWriter, Player character)
        {
            // Get first skill tab skills only for now, uncertain of how to have multiple skill tabs
            Dictionary<int, SkillMetadata> skillData = character.SkillTabs[0].SkillJob;
            Dictionary<int, int> skills = character.SkillTabs[0].SkillLevels;

            // Ordered list of skill ids (must be sent in this order)
            List<int> ids = character.SkillTabs[0].Order;
            byte split = (byte) Enum.Parse<JobSkillSplit>(Enum.GetName(character.Job));
            int countId = ids[ids.Count - split]; // Split to last skill id
            pWriter.WriteByte((byte) (ids.Count - split)); // Skill count minus split

            // List of Skills to display on the client
            foreach (int id in ids)
            {
                if (id == countId)
                {
                    pWriter.WriteByte(split); // Write that there are (split) skills left
                }
                pWriter.WriteByte();
                pWriter.WriteBool(skills[id] > 0);  // Is it learned?
                pWriter.WriteInt(id);               // Skill to display
                pWriter.WriteInt(Math.Clamp(skills[id], skillData[id].SkillLevels.Select(x => x.Level).FirstOrDefault(), int.MaxValue));    // Level to display
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
    }
}
