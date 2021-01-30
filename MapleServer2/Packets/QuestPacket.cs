using System;
using System.Collections.Generic;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets
{
    public static class QuestPacket
    {
        private enum QuestType : byte
        {
            AcceptQuest = 0x02,
            CompleteExplorationGoal = 0x03,
            CompleteQuest = 0x04,
            SendQuests = 0x16 // send the status of every quest
        }

        // Client has some sort of caching for this packet resulting subsequent packets to not change anything
        // TODO: Find an way to overwrite the cache in the client.
        public static Packet SendQuests(List<QuestMetadata> questList)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.QUEST);
            pWriter.WriteEnum(QuestType.SendQuests);
            pWriter.WriteInt(questList.Count);

            foreach (QuestMetadata item in questList)
            {
                pWriter.WriteInt(item.Basic.QuestID);
                pWriter.WriteInt(0); // 0 and 0 to set to not started, 1 and 0 to started not completed, use 2 and 1 to set to completed
                pWriter.WriteInt(0);
                pWriter.WriteLong(DateTimeOffset.UtcNow.ToUnixTimeSeconds()); // Timestamp of receiving the quest
                pWriter.WriteLong(); // Timestamp of completion
                pWriter.WriteByte(item.Basic.AutoStart); // unsure need more testing
                pWriter.WriteInt(item.Condition.Count);

                for (int j = 0; j < item.Condition.Count; j++)
                {
                    // if or switch check for condition types, for now just 0
                    pWriter.WriteInt(0);
                }
            }

            return pWriter;
        }

        public static Packet AcceptQuest(int questId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.QUEST);
            pWriter.WriteEnum(QuestType.AcceptQuest);
            pWriter.WriteInt(questId);
            pWriter.WriteLong(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            pWriter.WriteByte(1);
            pWriter.WriteInt(0);

            return pWriter;
        }

        public static Packet CompleteQuest(int questId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.QUEST);
            pWriter.WriteEnum(QuestType.CompleteQuest);
            pWriter.WriteInt(questId);
            pWriter.WriteInt(1);
            pWriter.WriteLong(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

            return pWriter;
        }

        public static Packet SendQuestDialog(int objectId, List<QuestMetadata> questList)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.QUEST);
            pWriter.WriteByte(0x1);
            pWriter.WriteInt(objectId);
            pWriter.WriteInt(questList.Count);
            foreach (QuestMetadata quest in questList)
            {
                pWriter.WriteInt(quest.Basic.QuestID);

            }

            return pWriter;

        }

        public static Packet CompleteExplorationGoal(int questId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.QUEST);
            pWriter.WriteEnum(QuestType.CompleteExplorationGoal);
            pWriter.WriteInt(questId);
            pWriter.WriteInt(1);
            pWriter.WriteInt(1);

            return pWriter;
        }
    }
}
