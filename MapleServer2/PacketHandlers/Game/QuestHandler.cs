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

            QuestStatus quest = session.Player.QuestList.FirstOrDefault(x => x.Basic.QuestID == questId);
            if (quest == null)
            {
                return;
            }

            quest.Started = true;
            session.Send(QuestPacket.AcceptQuest(questId));
        }

        private static void HandleCompleteQuest(GameSession session, PacketReader packet)
        {
            int questId = packet.ReadInt();
            int objectId = packet.ReadInt();

            QuestStatus quest = session.Player.QuestList.FirstOrDefault(x => x.Basic.QuestID == questId);
            if (quest == null)
            {
                return;
            }

            quest.Completed = true;
            session.Send(QuestPacket.CompleteQuest(questId));

            session.Player.Levels.GainExp(quest.Reward.Exp);
            session.Player.Wallet.Meso.Modify(quest.Reward.Money);
        }

        private static void HandleCompleteNavigator(GameSession session, PacketReader packet)
        {
            int questId = packet.ReadInt();
            QuestStatus quest = session.Player.QuestList.FirstOrDefault(x => x.Basic.QuestID == questId);
            if (quest == null)
            {
                return;
            }

            foreach (QuestRewardItem rewardItem in quest.RewardItems)
            {
                Item item = new Item(rewardItem.Code)
                {
                    Amount = rewardItem.Count
                };
                InventoryController.Add(session, item, true);
            }

            session.Send(QuestPacket.CompleteQuest(questId));
        }

        private static void HandleAddExplorationQuests(GameSession session, PacketReader packet)
        {
            List<QuestStatus> list = new List<QuestStatus>();

            int listSize = packet.ReadInt();
            for (int i = 0; i < listSize; i++)
            {
                int questId = packet.ReadInt();
                if (session.Player.QuestList.Exists(x => x.Basic.QuestID == questId))
                {
                    continue;
                }

                QuestMetadata metadata = QuestMetadataStorage.GetMetadata(questId);
                QuestStatus questStatus = new QuestStatus()
                {
                    Basic = metadata.Basic,
                    Started = true,
                    StartTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    Condition = metadata.Condition,
                    Reward = metadata.Reward,
                    RewardItems = metadata.RewardItem
                };

                list.Add(questStatus);
            }

            session.Player.QuestList.AddRange(list);
            session.Send(QuestPacket.SendQuests(list));
        }
    }
}
