using Maple2Storage.Types.Metadata;
using MapleServer2.Commands.Core;
using MapleServer2.Data.Static;
using MapleServer2.Packets;
using MapleServer2.Tools;
using MapleServer2.Types;

namespace MapleServer2.Commands.Game
{
    public class QuestCommand : InGameCommand
    {
        public QuestCommand()
        {
            Aliases = new()
            {
                "completequest"
            };
            Description = "Complete a Quest by id.";
            AddParameter<int>("id", "The id of the Quest.");
        }

        public override void Execute(GameCommandTrigger trigger)
        {
            int questId = trigger.Get<int>("id");
            QuestStatus questStatus = trigger.Session.Player.QuestList.FirstOrDefault(x => x.Basic.Id == questId);

            if (questStatus == null)
            {
                trigger.Session.SendNotice($"Quest not found with id: <font color='#93f5eb'>{questId}</font>.");
                return;
            }
            questStatus.Completed = true;
            questStatus.CompleteTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
            trigger.Session.Player.Levels.GainExp(questStatus.Reward.Exp);
            trigger.Session.Player.Wallet.Meso.Modify(questStatus.Reward.Money);

            foreach (QuestRewardItem reward in questStatus.RewardItems)
            {
                Item newItem = new Item(reward.Code)
                {
                    Amount = reward.Count,
                    Rarity = reward.Rank
                };
                if (newItem.RecommendJobs.Contains(trigger.Session.Player.Job) || newItem.RecommendJobs.Contains(0))
                {
                    InventoryController.Add(trigger.Session, newItem, true);
                }
            }
            trigger.Session.Send(QuestPacket.CompleteQuest(questId, true));

            // Add next quest
            IEnumerable<KeyValuePair<int, QuestMetadata>> questList = QuestMetadataStorage.GetAllQuests().Where(x => x.Value.Require.RequiredQuests.Contains(questId));
            foreach (KeyValuePair<int, QuestMetadata> kvp in questList)
            {
                trigger.Session.Player.QuestList.Add(new QuestStatus(trigger.Session.Player, kvp.Value));
            }
        }
    }
}
