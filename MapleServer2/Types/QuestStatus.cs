using System;
using System.Collections.Generic;
using Maple2Storage.Types.Metadata;

namespace MapleServer2.Types
{
    public class QuestStatus
    {
        public QuestBasic Basic { get; set; }
        public bool Started { get; set; }
        public bool Completed { get; set; }
        public long StartTimestamp { get; set; }
        public long CompleteTimestamp { get; set; }
        public long StartNpcId { get; set; }
        public long CompleteNpcId { get; set; }
        public List<QuestCondition> Condition { get; set; }
        public QuestReward Reward { get; set; }
        public List<QuestRewardItem> RewardItems { get; set; }

        public QuestStatus() { }

        public QuestStatus(QuestMetadata metadata)
        {
            Basic = metadata.Basic;
            StartTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            StartNpcId = metadata.StartNpc;
            CompleteNpcId = metadata.CompleteNpc;
            Condition = metadata.Condition;
            Reward = metadata.Reward;
            RewardItems = metadata.RewardItem;
        }
    }
}
