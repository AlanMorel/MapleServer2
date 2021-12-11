using Maple2Storage.Enums;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class QuestPacket
{
    private enum QuestType : byte
    {
        Dialog = 0x01,
        AcceptQuest = 0x02,
        UpdateCondition = 0x03,
        CompleteQuest = 0x04,
        ToggleTracking = 0x09,
        StartList = 0x15,
        SendQuests = 0x16, // send the status of every quest
        EndList = 0x17,
        FameMissions = 0x1F, // not sure
        FameMissions2 = 0x20 // not sure
    }

    public static PacketWriter SendDialogQuest(int objectId, List<QuestStatus> questList)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.QUEST);
        pWriter.Write(QuestType.Dialog);
        pWriter.WriteInt(objectId);
        pWriter.WriteInt(questList.Count);
        foreach (QuestStatus quest in questList)
        {
            pWriter.WriteInt(quest.Basic.Id);
        }

        return pWriter;
    }

    public static PacketWriter AcceptQuest(int questId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.QUEST);
        pWriter.Write(QuestType.AcceptQuest);
        pWriter.WriteInt(questId);
        pWriter.WriteLong(TimeInfo.Now());
        pWriter.WriteByte(1);
        pWriter.WriteInt(0);

        return pWriter;
    }

    public static PacketWriter UpdateCondition(int questId, List<Condition> conditions)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.QUEST);
        pWriter.Write(QuestType.UpdateCondition);
        pWriter.WriteInt(questId);
        pWriter.WriteInt(conditions.Count);
        foreach (Condition condition in conditions)
        {
            pWriter.WriteInt(condition.Current);
        }

        return pWriter;
    }

    // Animation: Animates the quest helper
    public static PacketWriter CompleteQuest(int questId, bool animation)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.QUEST);
        pWriter.Write(QuestType.CompleteQuest);
        pWriter.WriteInt(questId);
        pWriter.WriteInt(animation ? 1 : 0);
        pWriter.WriteLong(TimeInfo.Now());

        return pWriter;
    }

    public static PacketWriter ToggleTracking(int questId, bool tracked)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.QUEST);
        pWriter.Write(QuestType.ToggleTracking);
        pWriter.WriteInt(questId);
        pWriter.WriteBool(tracked);

        return pWriter;
    }

    public static PacketWriter StartList()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.QUEST);
        pWriter.Write(QuestType.StartList);
        pWriter.WriteLong(); // unknown, sometimes it has an value

        return pWriter;
    }

    public static PacketWriter SendQuests(List<QuestStatus> questList)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.QUEST);
        pWriter.Write(QuestType.SendQuests);

        pWriter.WriteInt(questList.Count);
        foreach (QuestStatus quest in questList)
        {
            pWriter.WriteInt(quest.Basic.Id);

            switch (quest.State)
            {
                case QuestState.None:
                    pWriter.WriteInt();
                    pWriter.WriteInt();
                    break;
                case QuestState.Started:
                    pWriter.WriteInt(1);
                    pWriter.WriteInt();
                    break;
                case QuestState.ConditionCompleted:
                    pWriter.WriteInt(2);
                    pWriter.WriteInt();
                    break;
                case QuestState.Finished:
                    pWriter.WriteInt(2);
                    pWriter.WriteInt(1);
                    break;
            }

            pWriter.WriteLong(quest.StartTimestamp);
            pWriter.WriteLong(quest.CompleteTimestamp);
            pWriter.WriteBool(quest.Tracked);
            pWriter.WriteInt(quest.Condition.Count);

            foreach (Condition condition in quest.Condition)
            {
                pWriter.WriteInt(condition.Current);
            }
        }

        return pWriter;
    }

    public static PacketWriter EndList()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.QUEST);
        pWriter.Write(QuestType.EndList);
        pWriter.WriteInt();

        return pWriter;
    }

    public static PacketWriter Packet1F()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.QUEST);
        pWriter.Write(QuestType.FameMissions);
        pWriter.WriteByte();
        pWriter.WriteInt();

        return pWriter;
    }

    public static PacketWriter Packet20()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.QUEST);
        pWriter.Write(QuestType.FameMissions2);
        pWriter.WriteByte();
        pWriter.WriteInt();

        return pWriter;
    }
}
