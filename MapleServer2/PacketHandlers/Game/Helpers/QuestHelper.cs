using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game.Helpers
{
    public class QuestHelper
    {
        public static void UpdateExplorationQuest(GameSession session, string code, string type)
        {
            List<QuestStatus> quests = session.Player.QuestList.Where(quest => quest.Basic.QuestType == QuestType.Exploration
                && quest.Condition != null
                && !quest.Completed
                && quest.Started
                && quest.Condition.Any(condition => condition.Type == type && condition.Codes.Contains(code)))
                .ToList();
            foreach (QuestStatus quest in quests)
            {
                Condition condition = quest.Condition.FirstOrDefault(condition => condition.Type == type
                && condition.Codes.Length != 0
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

                quest.Completed = true;
                quest.CompleteTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                DatabaseManager.Quests.Update(quest);

                session.Player.Levels.GainExp(quest.Reward.Exp);
                session.Player.Wallet.Meso.Modify(quest.Reward.Money);
                session.Send(QuestPacket.CompleteQuest(quest.Basic.Id, false));
                return;
            }
        }

        public static void UpdateQuest(GameSession session, string code, string type, string target = "")
        {
            List<QuestStatus> questList = session.Player.QuestList.Where(quest => quest.Condition != null
            && quest.Condition.Any(condition => condition.Type == type && condition.Codes.Contains(code) && condition.Target.Contains(target))
            && quest.Started
            && !quest.Completed).ToList();
            foreach (QuestStatus quest in questList)
            {
                Condition condition = quest.Condition.FirstOrDefault(condition => condition.Codes != null
                    && condition.Codes.Length != 0
                    && condition.Codes.Contains(code)
                    && condition.Target.Contains(target)
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
                return;
            }
        }

        public static void GetNewQuests(GameSession session, int level)
        {
            List<QuestMetadata> questList = QuestMetadataStorage.GetAvailableQuests(level);
            foreach (QuestMetadata quest in questList)
            {
                if (session.Player.QuestList.Exists(x => x.Basic.Id == quest.Basic.Id))
                {
                    continue;
                }
                session.Player.QuestList.Add(new QuestStatus(session.Player, quest));
            }

            session.Send(QuestPacket.SendQuests(session.Player.QuestList));
        }
    }
}
