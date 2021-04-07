using System;
using System.Collections.Generic;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class QuestPacket
    {
        private enum QuestType : byte
        {
            Dialog = 0x01,
            AcceptQuest = 0x02,
            CompleteExplorationGoal = 0x03,
            CompleteQuest = 0x04,
            SendQuests = 0x16 // send the status of every quest
        }

        // Client has some sort of caching for this packet resulting subsequent packets to not change anything
        // TODO: Find an way to overwrite the cache in the client.
        public static Packet SendQuests(List<QuestStatus> questList)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.QUEST);
            pWriter.WriteEnum(QuestType.SendQuests);

            pWriter.WriteInt(questList.Count);
            foreach (QuestStatus quest in questList)
            {
                pWriter.WriteInt(quest.Basic.Id);

                if (quest.Completed)
                {
                    pWriter.WriteInt(2);
                    pWriter.WriteInt(1);
                }
                else if (quest.Started)
                {
                    pWriter.WriteInt(1);
                    pWriter.WriteInt(0);
                }
                else
                {
                    pWriter.WriteInt(0);
                    pWriter.WriteInt(0);
                }

                pWriter.WriteLong(quest.StartTimestamp);
                pWriter.WriteLong(quest.CompleteTimestamp);
                pWriter.WriteByte(quest.Basic.AutoStart); // unsure need more testing
                pWriter.WriteInt(quest.Condition.Count);

                for (int j = 0; j < quest.Condition.Count; j++)
                {
                    // This int is the value of each condition. 
                    // Ex: 'Kill x/20 monsters'
                    pWriter.WriteInt();
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

        public static Packet SendDialogQuest(int objectId, List<QuestStatus> questList)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.QUEST);
            pWriter.WriteEnum(QuestType.Dialog);
            pWriter.WriteInt(objectId);
            pWriter.WriteInt(questList.Count);
            foreach (QuestStatus quest in questList)
            {
                pWriter.WriteInt(quest.Basic.Id);
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
