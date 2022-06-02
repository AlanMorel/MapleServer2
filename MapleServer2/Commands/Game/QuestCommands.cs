using System.Drawing;
using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MapleServer2.Commands.Core;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Types;

namespace MapleServer2.Commands.Game;

public class CompleteQuestCommand : InGameCommand
{
    public CompleteQuestCommand()
    {
        Aliases = new()
        {
            "completequest"
        };
        Description = "Complete a Quest by id.";
        Parameters = new()
        {
            new Parameter<int>("id", "The id of the Quest.")
        };
        Usage = "/completequest [id]";
    }

    public override void Execute(GameCommandTrigger trigger)
    {
        int questId = trigger.Get<int>("id");
        if (questId == 0)
        {
            trigger.Session.SendNotice("Please type an quest id");
            return;
        }

        if (!QuestMetadataStorage.IsValid(questId))
        {
            trigger.Session.Send(NoticePacket.Notice($"Quest not found with id: {questId.ToString().Color(Color.Aquamarine)}.", NoticeType.Chat));
            return;
        }

        Player player = trigger.Session.Player;
        if (!player.QuestData.TryGetValue(questId, out QuestStatus questStatus))
        {
            questStatus = new(player.CharacterId, questId, QuestState.Started, TimeInfo.Now(), true);
            player.QuestData.Add(questId, questStatus);
            trigger.Session.Send(QuestPacket.AcceptQuest(questStatus));
        }

        questStatus.State = QuestState.Completed;
        questStatus.AmountCompleted++;
        questStatus.Condition.ForEach(x =>
        {
            x.Completed = true;
            x.Current = x.Goal;
        });
        questStatus.StartTimestamp = TimeInfo.Now();
        questStatus.CompleteTimestamp = TimeInfo.Now();
        player.Levels.GainExp(questStatus.Reward.Exp);
        player.Wallet.Meso.Modify(questStatus.Reward.Money);

        foreach (QuestRewardItem reward in questStatus.RewardItems)
        {
            Item newItem = new(reward.Code)
            {
                Amount = reward.Count,
                Rarity = reward.Rank
            };
            if (newItem.RecommendJobs.Contains(player.Job) || newItem.RecommendJobs.Contains(0))
            {
                player.Inventory.AddItem(trigger.Session, newItem, true);
            }
        }

        DatabaseManager.Quests.Update(questStatus);
        trigger.Session.Send(QuestPacket.CompleteQuest(questId, true));

        // Add next quest
        IEnumerable<KeyValuePair<int, QuestMetadata>> questList =
            QuestMetadataStorage.GetAllQuests().Where(x => x.Value.Require.RequiredQuests.Contains(questId));
        foreach ((int id, QuestMetadata quest) in questList)
        {
            if (player.QuestData.ContainsKey(id))
            {
                continue;
            }

            player.QuestData.Add(id, new(player.CharacterId, quest));
        }
    }
}

public class StartQuestCommand : InGameCommand
{
    public StartQuestCommand()
    {
        Aliases = new()
        {
            "startquest"
        };
        Description = "Start a Quest by id.";
        Parameters = new()
        {
            new Parameter<int>("id", "The id of the Quest."),
            new Parameter<bool>("force", "Force the Quest to start.")
        };
        Usage = "/startquest [id] [force start]";
    }

    public override void Execute(GameCommandTrigger trigger)
    {
        int questId = trigger.Get<int>("id");
        bool forceStart = trigger.Get<bool>("force");
        if (questId == 0)
        {
            trigger.Session.SendNotice("Type an quest id.");
            return;
        }

        QuestMetadata quest = QuestMetadataStorage.GetMetadata(questId);
        if (quest == null)
        {
            trigger.Session.Send(NoticePacket.Notice($"Quest not found with id: {questId.ToString().Color(Color.Aquamarine)}.", NoticeType.Chat));
            return;
        }

        Player player = trigger.Session.Player;
        if (player.QuestData.TryGetValue(questId, out QuestStatus questStatus) && !forceStart)
        {
            trigger.Session.Send(NoticePacket.Notice(
                $"You already have quest: {questId.ToString().Color(Color.Aquamarine)}. \r\tUse '/startquest {questId} true' to start it again.",
                NoticeType.Chat));
            return;
        }

        if (questStatus is not null)
        {
            player.QuestData.Remove(questId, out _);
            DatabaseManager.Quests.Delete(questStatus.Uid);
        }

        questStatus = new(player.CharacterId, questId, QuestState.Started, TimeInfo.Now(), accepted: true);
        player.QuestData.Add(questId, questStatus);
        trigger.Session.Send(QuestPacket.AcceptQuest(questStatus));
    }
}
