using System;
using System.Collections.Generic;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
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
            ExplorationGoals = 0x08,
            CompleteNavigator = 0x18,
        }

        public override void Handle(GameSession session, PacketReader packet) // TODO: Refactor when DB is implemented
        {
            QuestMode mode = (QuestMode) packet.ReadByte();
            switch (mode)
            {
                case QuestMode.AcceptQuest:
                    {
                        int questid = packet.ReadInt();
                        int objectid = packet.ReadInt();
                        session.Send(QuestPacket.AcceptQuest(questid));
                        break;
                    }
                case QuestMode.CompleteQuest:
                    {
                        int questid = packet.ReadInt();
                        int objectid = packet.ReadInt();
                        session.Send(QuestPacket.CompleteQuest(questid));
                        QuestReward quest = QuestMetadataStorage.GetMetadata(questid).Reward;
                        session.Player.Levels.GainExp(quest.Exp);
                        session.Player.Wallet.Meso.Modify(quest.Money);
                        break;
                    }
                case QuestMode.ExplorationGoals:
                    List<QuestMetadata> list = new List<QuestMetadata>();

                    int listSize = packet.ReadInt();

                    for (int i = 0; i < listSize; i++)
                    {
                        list.Add(QuestMetadataStorage.GetMetadata(packet.ReadInt()));
                    }

                    session.Send(QuestPacket.SendQuests(list));
                    break;
                case QuestMode.CompleteNavigator:
                    int questId = packet.ReadInt();

                    foreach (QuestRewardItem item in QuestMetadataStorage.GetMetadata(questId).RewardItem)
                    {
                        InventoryController.Add(session, new Types.Item(item.Code), true);
                    }

                    session.Send(QuestPacket.CompleteQuest(questId));
                    break;
                default:
                    IPacketHandler<GameSession>.LogUnknownMode(mode);
                    break;
            }
        }
    }
}
