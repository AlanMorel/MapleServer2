using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Database;

namespace MapleServer2.Types;

public class QuestStatus
{
    public long Uid { get; private set; }
    public int Id { get; private set; }
    public QuestBasic Basic { get; private set; }
    public long StartTimestamp { get; set; }
    public long CompleteTimestamp { get; set; }
    public long StartNpcId { get; private set; }
    public long CompleteNpcId { get; private set; }
    public bool Tracked { get; set; }
    public List<Condition> Condition { get; set; }
    public QuestReward Reward { get; private set; }
    public List<QuestRewardItem> RewardItems { get; private set; }
    public QuestState State;

    public readonly long CharacterId;

    public QuestStatus(long uid, int id, long characterId, bool tracked, long startTimestamp, long completeTimestamp, List<Condition> conditions, QuestState state)
    {
        Uid = uid;
        Id = id;
        CharacterId = characterId;
        StartTimestamp = startTimestamp;
        CompleteTimestamp = completeTimestamp;
        Condition = conditions;
        State = state;
        Tracked = tracked;
        SetMetadataValues();
    }

    public QuestStatus(Player player, QuestMetadata metadata, QuestState state = QuestState.None, long startTimestamp = 0)
    {
        CharacterId = player.CharacterId;
        Id = metadata.Basic.Id;
        Basic = metadata.Basic;
        StartNpcId = metadata.StartNpc;
        CompleteNpcId = metadata.CompleteNpc;
        Condition = new();
        foreach (QuestCondition condition in metadata.Condition)
        {
            Condition.Add(new(condition.Type, condition.Codes, condition.Goal, 0, condition.Target));
        }

        Reward = metadata.Reward;
        RewardItems = metadata.RewardItem;
        State = state;
        StartTimestamp = startTimestamp;
        Tracked = true;
        Uid = DatabaseManager.Quests.Insert(this);
    }

    private void SetMetadataValues()
    {
        QuestMetadata metadata = QuestMetadataStorage.GetMetadata(Id);
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
    public string[] Codes { get; set; }
    public int Goal { get; set; }
    public int Current { get; set; }
    public bool Completed { get; set; }
    public readonly List<string> Target;

    public Condition(string type, string[] codes, int goal, int current, List<string> target)
    {
        Type = type;
        Codes = codes;
        Goal = goal;
        Current = current;
        Target = target;
    }
}
