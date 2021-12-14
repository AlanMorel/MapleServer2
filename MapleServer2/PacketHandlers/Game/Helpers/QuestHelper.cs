using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game.Helpers;

public static class QuestHelper
{
    public static void UpdateExplorationQuest(GameSession session, string code, string type)
    {
        List<QuestStatus> quests = session.Player.QuestList.Where(quest =>
                quest.Basic.QuestType is QuestType.Exploration
                && quest.Condition is not null
                && quest.State is QuestState.Started
                && quest.Condition.Any(condition => condition.Type == type && condition.Codes.Contains(code)))
            .ToList();
        foreach (QuestStatus quest in quests)
        {
            Condition condition = quest.Condition.FirstOrDefault(condition =>
                condition.Type == type
                && condition.Codes.Contains(code)
                && !condition.Completed);
            if (condition == null)
            {
                continue;
            }

            condition.Current++;

            if (condition.Current >= condition.Goal)
            {
                condition.Completed = true;
            }

            session.Send(QuestPacket.UpdateCondition(quest.Basic.Id, quest.Condition));

            if (!condition.Completed)
            {
                return;
            }

            quest.State = QuestState.Finished;
            quest.CompleteTimestamp = TimeInfo.Now();
            DatabaseManager.Quests.Update(quest);

            session.Player.Levels.GainExp(quest.Reward.Exp);
            session.Player.Wallet.Meso.Modify(quest.Reward.Money);
            session.Send(QuestPacket.CompleteQuest(quest.Basic.Id, false));
            return;
        }
    }

    public static void UpdateQuest(GameSession session, string code, string type, string target = "")
    {
        List<QuestStatus> questList = session.Player.QuestList.Where(quest =>
            quest.Condition is not null
            && quest.State is QuestState.Started
            && quest.Condition.Any(condition => condition.Codes is not null
                                                && condition.Target is not null
                                                && condition.Type == type
                                                && condition.Codes.Contains(code)
                                                && (condition.Target.Contains(target) || condition.Target.Count == 0))
        ).ToList();
        foreach (QuestStatus quest in questList)
        {
            Condition condition = quest.Condition.FirstOrDefault(condition =>
                condition.Codes.Contains(code)
                && (condition.Target.Contains(target) || condition.Target.Count == 0)
                && !condition.Completed);
            if (condition == null)
            {
                continue;
            }

            if (condition.Goal != 0)
            {
                if (condition.Goal == condition.Current)
                {
                    return;
                }
            }

            condition.Current++;
            if (condition.Current >= condition.Goal)
            {
                condition.Completed = true;
            }

            session.Send(QuestPacket.UpdateCondition(quest.Basic.Id, quest.Condition));
            DatabaseManager.Quests.Update(quest);
        }
    }

    public static void GetNewQuests(Player player)
    {
        List<QuestMetadata> questList = QuestMetadataStorage.GetAvailableQuests(player.Levels.Level, player.Job);
        foreach (QuestMetadata quest in questList)
        {
            if (player.QuestList.Any(x => x.Basic.Id == quest.Basic.Id))
            {
                continue;
            }

            player.QuestList.Add(new(player, quest));
        }

        player.Session.Send(QuestPacket.SendQuests(player.QuestList));
    }
}
