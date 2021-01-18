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
            CompleteExplorationGoal = 0x03,
            CompleteQuest = 0x04,
            SendQuests = 0x16 // send the status of every quest
        }

        // Client has some sort of caching for this packet resulting subsequent packets to not change anything
        // TODO: Find an way to overwrite the cache in the client.
        public static Packet SendQuests(List<QuestMetadata> questList)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.QUEST);
            pWriter.WriteMode(QuestType.SendQuests);
            pWriter.WriteInt(questList.Count);

            foreach (QuestMetadata item in questList)
            {
                pWriter.WriteInt(item.Basic.QuestID);
                pWriter.WriteInt(1); // 1 and 0 to set to not started, use 2 and 1 to set to completed
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

        public static Packet CompleteQuest(int questId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.QUEST);
            pWriter.WriteMode(QuestType.CompleteQuest);
            pWriter.WriteInt (questId);
            pWriter.WriteInt(1);
            pWriter.WriteLong(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

            return pWriter;
        }

        public static Packet CompleteExplorationGoal(int questId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.QUEST);
            pWriter.WriteMode(QuestType.CompleteExplorationGoal);
            pWriter.WriteInt (questId);
            pWriter.WriteInt(1);
            pWriter.WriteInt(1);

            return pWriter;
        }
    }
}
