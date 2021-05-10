using System;
using System.Collections.Generic;
using System.Linq;
using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game.Helpers
{
    public class QuestHelper
    {
        public static void UpdateExplorationQuest(GameSession session, string code, string type)
        {
            List<QuestStatus> questList = session.Player.QuestList;
            foreach (QuestStatus quest in questList.Where(x => x.Basic.QuestType == QuestType.Exploration && x.Condition != null))
            {
                Condition condition = quest.Condition.Where(x => x.Type == type)
                    .FirstOrDefault(x => x.Codes.Length != 0 && x.Codes.Contains(code));
                if (condition == null)
                {
                    continue;
                }

                if (condition.Goal != condition.Current)
                {
                    condition.Current++;
                }

                session.Send(QuestPacket.UpdateCondition(quest.Basic.Id, quest.Condition.IndexOf(condition) + 1, condition.Current));

                if (condition.Goal != condition.Current) // Quest completed
                {
                    return;
                }
                quest.Completed = true;
                quest.CompleteTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                session.Player.Levels.GainExp(quest.Reward.Exp);
                session.Player.Wallet.Meso.Modify(quest.Reward.Money);
                session.Send(QuestPacket.CompleteQuest(quest.Basic.Id, false));
                return;
            }
        }

        public static void UpdateQuest(GameSession session, string code, string type)
        {
            List<QuestStatus> questList = session.Player.QuestList;
            foreach (QuestStatus quest in questList.Where(x => x.Condition != null))
            {
                Condition condition = quest.Condition.Where(x => x.Type == type).FirstOrDefault(x => x.Codes != null && x.Codes.Length != 0 && x.Codes.Contains(code));
                if (condition == null)
                {
                    continue;
                }

                if (condition.Goal == condition.Current)
                {
                    return;
                }
                condition.Current++;
                session.Send(QuestPacket.UpdateCondition(quest.Basic.Id, quest.Condition.IndexOf(condition) + 1, condition.Current));
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
