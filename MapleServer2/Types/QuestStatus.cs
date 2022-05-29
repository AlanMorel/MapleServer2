using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Database;

namespace MapleServer2.Types;

public class QuestStatus
{
    public long Uid { get; }
    public int Id { get; }
    public long StartTimestamp { get; set; }
    public long CompleteTimestamp { get; set; }
    public bool Accepted { get; set; }
    public List<Condition> Condition { get; }
    public QuestState State { get; set; }
    public int AmountCompleted { get; set; }

    public QuestBasic Basic { get; private set; }
    public long StartNpcId { get; private set; }
    public long CompleteNpcId { get; private set; }
    public QuestReward Reward { get; private set; }

    public List<QuestRewardItem> RewardItems { get; private set; }
    public readonly long CharacterId;

    public QuestStatus(long uid, int id, long characterId, bool accepted, long startTimestamp, long completeTimestamp, List<Condition> conditions,
        QuestState state, int amountCompleted)
    {
        Uid = uid;
        Id = id;
        CharacterId = characterId;
        StartTimestamp = startTimestamp;
        CompleteTimestamp = completeTimestamp;
        Condition = conditions;
        State = state;
        Accepted = accepted;
        AmountCompleted = amountCompleted;
        SetMetadataValues(QuestMetadataStorage.GetMetadata(Id));
    }

    public QuestStatus(long characterId, int questId, QuestState state = QuestState.None, long startTimestamp = 0, bool accepted = false)
        : this(characterId, QuestMetadataStorage.GetMetadata(questId), state, startTimestamp, accepted) { }

    public QuestStatus(long characterId, QuestMetadata metadata, QuestState state = QuestState.None, long startTimestamp = 0, bool accepted = false)
    {
        SetMetadataValues(metadata);

        CharacterId = characterId;
        Id = metadata.Basic.Id;
        Condition = new();
        foreach (QuestCondition condition in metadata.Condition)
        {
            Condition.Add(new(condition.Type, condition.Code, condition.Goal, 0, condition.Target));
        }

        State = state;
        StartTimestamp = startTimestamp;
        Accepted = accepted;
        AmountCompleted = 0;
        Uid = DatabaseManager.Quests.Insert(this);
    }

    private void SetMetadataValues(QuestMetadata metadata)
    {
        Basic = metadata.Basic;
        StartNpcId = metadata.StartNpc;
        CompleteNpcId = metadata.CompleteNpc;
        Reward = metadata.Reward;
        RewardItems = metadata.RewardItem;
    }
}

public class Condition
{
    public string Type { get; set; }
    public string Code { get; set; }
    public int Goal { get; set; }
    public int Current { get; set; }
    public bool Completed { get; set; }
    public readonly string Target;

    public Condition(string type, string code, int goal, int current, string target)
    {
        Type = type;
        Code = code;
        Goal = goal;
        Current = current;
        Target = target;
    }
}
