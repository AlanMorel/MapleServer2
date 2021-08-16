using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Database.Classes;

namespace MapleServer2.Types
{
    public class QuestStatus
    {
        public long Uid { get; private set; }
        public int Id { get; private set; }
        public QuestBasic Basic { get; private set; }
        public bool Started { get; set; }
        public bool Completed { get; set; }
        public long StartTimestamp { get; set; }
        public long CompleteTimestamp { get; set; }
        public long StartNpcId { get; private set; }
        public long CompleteNpcId { get; private set; }
        public List<Condition> Condition { get; set; }
        public QuestReward Reward { get; private set; }
        public List<QuestRewardItem> RewardItems { get; private set; }

        public readonly long CharacterId;

        public QuestStatus(long uid, int id, long characterId, bool started, bool completed, long startTimestamp, long completeTimestamp, List<Condition> conditions)
        {
            Uid = uid;
            Id = id;
            CharacterId = characterId;
            Started = started;
            Completed = completed;
            StartTimestamp = startTimestamp;
            CompleteTimestamp = completeTimestamp;
            Condition = conditions;
            SetMetadataValues();
        }

        public QuestStatus(Player player, QuestMetadata metadata, bool started = false, long startTimestamp = 0)
        {
            CharacterId = player.CharacterId;
            Id = metadata.Basic.Id;
            Basic = metadata.Basic;
            StartNpcId = metadata.StartNpc;
            CompleteNpcId = metadata.CompleteNpc;
            Condition = new List<Condition>();
            foreach (QuestCondition condition in metadata.Condition)
            {
                Condition.Add(new Condition(condition.Type, condition.Codes, condition.Goal, 0, condition.Target));
            }
            Reward = metadata.Reward;
            RewardItems = metadata.RewardItem;
            Started = started;
            StartTimestamp = startTimestamp;
            Uid = DatabaseQuest.CreateQuest(this);
        }

        public void SetMetadataValues()
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
        public List<string> Target;

        public Condition(string type, string[] codes, int goal, int current, List<string> target)
        {
            Type = type;
            Codes = codes;
            Goal = goal;
            Current = current;
            Target = target;
        }
    }
}
