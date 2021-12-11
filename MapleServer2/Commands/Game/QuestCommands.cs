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
        QuestStatus questStatus = player.QuestList.FirstOrDefault(x => x.Basic.Id == questId);
        if (questStatus == null)
        {
            questStatus = new(player, QuestMetadataStorage.GetMetadata(questId));
            player.QuestList.Add(questStatus);
        }
        questStatus.State = QuestState.Finished;
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
        IEnumerable<KeyValuePair<int, QuestMetadata>> questList = QuestMetadataStorage.GetAllQuests().Where(x => x.Value.Require.RequiredQuests.Contains(questId));
        foreach (KeyValuePair<int, QuestMetadata> kvp in questList)
        {
            player.QuestList.Add(new(player, kvp.Value));
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
            new Parameter<int>("id", "The id of the Quest.")
        };
        Usage = "/startquest [id]";
    }

    public override void Execute(GameCommandTrigger trigger)
    {
        int questId = trigger.Get<int>("id");
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
        if (trigger.Session.Player.QuestList.Any(x => x.Basic.Id == questId))
        {
            trigger.Session.Send(NoticePacket.Notice($"You already have quest: {questId.ToString().Color(Color.Aquamarine)}.", NoticeType.Chat));
            return;
        }
        trigger.Session.Player.QuestList.Add(new(trigger.Session.Player, quest, QuestState.Started, TimeInfo.Now()));
        trigger.Session.Send(QuestPacket.AcceptQuest(questId));
    }
}
