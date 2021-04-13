using System;
using System.Collections.Generic;
using System.Linq;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class QuestHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.QUEST;

        public QuestHandler(ILogger<QuestHandler> logger) : base(logger) { }

        private enum QuestMode : byte
        {
            AcceptQuest = 0x02,
            CompleteQuest = 0x04,
            ExplorationQuests = 0x08,
            CompleteNavigator = 0x18,
        }

        public override void Handle(GameSession session, PacketReader packet)
        {
            QuestMode mode = (QuestMode) packet.ReadByte();

            switch (mode)
            {
                case QuestMode.AcceptQuest:
                    HandleAcceptQuest(session, packet);
                    break;
                case QuestMode.CompleteQuest:
                    HandleCompleteQuest(session, packet);
                    break;
                case QuestMode.ExplorationQuests:
                    HandleAddExplorationQuests(session, packet);
                    break;
                case QuestMode.CompleteNavigator:
                    HandleCompleteNavigator(session, packet);
                    break;
                default:
                    IPacketHandler<GameSession>.LogUnknownMode(mode);
                    break;
            }
        }

        private static void HandleAcceptQuest(GameSession session, PacketReader packet)
        {
            int questId = packet.ReadInt();
            int objectId = packet.ReadInt();

            QuestStatus questStatus = session.Player.QuestList.FirstOrDefault(x => x.Basic.Id == questId);
            if (questStatus == null)
            {
                return;
            }

            questStatus.Started = true;
            questStatus.StartTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
            session.Send(QuestPacket.AcceptQuest(questId));
        }

        private static void HandleCompleteQuest(GameSession session, PacketReader packet)
        {
            int questId = packet.ReadInt();
            int objectId = packet.ReadInt();

            QuestStatus questStatus = session.Player.QuestList.FirstOrDefault(x => x.Basic.Id == questId);
            if (questStatus == null)
            {
                return;
            }

            questStatus.Completed = true;
            questStatus.CompleteTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();

            session.Player.Levels.GainExp(questStatus.Reward.Exp);
            session.Player.Wallet.Meso.Modify(questStatus.Reward.Money);

            foreach (QuestRewardItem reward in questStatus.RewardItems)
            {
                Item newItem = new Item(reward.Code)
                {
                    Amount = reward.Count,
                    Rarity = reward.Rank
                };
                if (newItem.RecommendJobs.Contains(session.Player.Job) || newItem.RecommendJobs.Contains(0))
                {
                    InventoryController.Add(session, newItem, true);
                }
            }

            session.Send(QuestPacket.CompleteQuest(questId, true));

            // Add next quest
            IEnumerable<KeyValuePair<int, QuestMetadata>> questList = QuestMetadataStorage.GetAllQuests().Where(x => x.Value.Require.RequiredQuests.Contains(questId));
            foreach (KeyValuePair<int, QuestMetadata> kvp in questList)
            {
                if (session.Player.QuestList.Exists(x => x.Basic.Id == kvp.Value.Basic.Id))
                {
                    continue;
                }
                session.Player.QuestList.Add(new QuestStatus(kvp.Value));
            }
        }

        private static void HandleCompleteNavigator(GameSession session, PacketReader packet)
        {
            int questId = packet.ReadInt();
            QuestStatus questStatus = session.Player.QuestList.FirstOrDefault(x => x.Basic.Id == questId);
            if (questStatus == null)
            {
                return;
            }

            foreach (QuestRewardItem rewardItem in questStatus.RewardItems)
            {
                Item item = new Item(rewardItem.Code)
                {
                    Amount = rewardItem.Count,
                    Rarity = rewardItem.Rank
                };
                InventoryController.Add(session, item, true);
            }
            questStatus.Completed = true;
            questStatus.CompleteTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();

            session.Send(QuestPacket.CompleteQuest(questId, false));
        }

        private static void HandleAddExplorationQuests(GameSession session, PacketReader packet)
        {
            List<QuestStatus> list = new List<QuestStatus>();

            int listSize = packet.ReadInt();
            for (int i = 0; i < listSize; i++)
            {
                int questId = packet.ReadInt();
                if (session.Player.QuestList.Exists(x => x.Basic.Id == questId && x.Started))
                {
                    continue;
                }

                QuestMetadata metadata = QuestMetadataStorage.GetMetadata(questId);
                QuestStatus questStatus = new QuestStatus(metadata)
                {
                    Started = true,
                    StartTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds()
                };
                list.Add(questStatus);
                session.Send(QuestPacket.AcceptQuest(questStatus.Basic.Id));
            }

            session.Player.QuestList.AddRange(list);
        }
    }
}
