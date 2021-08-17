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
            StartList = 0x15,
            SendQuests = 0x16, // send the status of every quest
            EndList = 0x17,
            FameMissions = 0x1F, // not sure
            FameMissions2 = 0x20, // not sure
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

        public static Packet UpdateCondition(int questId, int conditionIndex, int value)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.QUEST);
            pWriter.WriteEnum(QuestType.CompleteExplorationGoal);
            pWriter.WriteInt(questId);
            pWriter.WriteInt(conditionIndex);
            pWriter.WriteInt(value);

            return pWriter;
        }

        // Animation: Animates the quest helper
        public static Packet CompleteQuest(int questId, bool animation)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.QUEST);
            pWriter.WriteEnum(QuestType.CompleteQuest);
            pWriter.WriteInt(questId);
            pWriter.WriteInt(animation ? 1 : 0);
            pWriter.WriteLong(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

            return pWriter;
        }

        public static Packet StartList()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.QUEST);
            pWriter.WriteEnum(QuestType.StartList);
            pWriter.WriteLong(); // unknown, sometimes it has an value

            return pWriter;
        }

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
                    pWriter.WriteInt(quest.Condition[j].Current);
                }
            }

            return pWriter;
        }

        public static Packet EndList()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.QUEST);
            pWriter.WriteEnum(QuestType.EndList);
            pWriter.WriteInt();

            return pWriter;
        }

        public static Packet Packet1F()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.QUEST);
            pWriter.WriteEnum(QuestType.FameMissions);
            pWriter.WriteByte();
            pWriter.WriteInt();

            return pWriter;
        }

        public static Packet Packet20()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.QUEST);
            pWriter.WriteEnum(QuestType.FameMissions2);
            pWriter.WriteByte();
            pWriter.WriteInt();

            return pWriter;
        }
    }
}
