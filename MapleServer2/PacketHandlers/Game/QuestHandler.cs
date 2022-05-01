using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Managers;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class QuestHandler : GamePacketHandler<QuestHandler>
{
    public override RecvOp OpCode => RecvOp.Quest;

    private enum QuestMode : byte
    {
        AcceptQuest = 0x02,
        CompleteQuest = 0x04,
        ExplorationQuests = 0x08,
        ToggleTracking = 0x09,
        SkyFortress = 0x0E,
        ResumeDungeon = 0x13,
        DispatchMode = 0x14,
        CompleteNavigator = 0x18
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
            case QuestMode.ResumeDungeon:
                HandleResumeDungeon(session, packet);
                break;
            case QuestMode.DispatchMode:
                HandleDispatchMode(session, packet);
                break;
            case QuestMode.ToggleTracking:
                HandleToggleTracking(session, packet);
                break;
            case QuestMode.SkyFortress:
                HandleSkyFortressTeleport(session);
                break;
            default:
                LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleSkyFortressTeleport(GameSession session)
    {
        session.Player.Warp(Map.SkyFortressBridge);
    }

    private static void HandleAcceptQuest(GameSession session, PacketReader packet)
    {
        int questId = packet.ReadInt();
        int objectId = packet.ReadInt();

        if (!session.Player.QuestData.TryGetValue(questId, out QuestStatus questStatus))
        {
            return;
        }

        questStatus.State = QuestState.Started;
        questStatus.StartTimestamp = TimeInfo.Now();
        DatabaseManager.Quests.Update(questStatus);
        session.Send(QuestPacket.AcceptQuest(questStatus));
        TrophyManager.OnAcceptQuest(session.Player, questId);
    }

    private static void HandleCompleteQuest(GameSession session, PacketReader packet)
    {
        int questId = packet.ReadInt();
        int objectId = packet.ReadInt();

        if (!session.Player.QuestData.TryGetValue(questId, out QuestStatus questStatus) || questStatus.State is QuestState.Completed)
        {
            return;
        }

        questStatus.State = QuestState.Completed;
        questStatus.AmountCompleted++;
        questStatus.CompleteTimestamp = TimeInfo.Now();

        session.Player.Levels.GainExp(questStatus.Reward.Exp);
        session.Player.Wallet.Meso.Modify(questStatus.Reward.Money);

        foreach (QuestRewardItem reward in questStatus.RewardItems)
        {
            Item newItem = new(reward.Code)
            {
                Amount = reward.Count,
                Rarity = reward.Rank
            };
            if (newItem.RecommendJobs.Contains(session.Player.Job) || newItem.RecommendJobs.Contains(0))
            {
                session.Player.Inventory.AddItem(session, newItem, true);
            }
        }

        DatabaseManager.Quests.Update(questStatus);
        session.Send(QuestPacket.CompleteQuest(questId, true));

        // Add next quest
        IEnumerable<QuestMetadata> questList = QuestMetadataStorage.GetAllQuests().Values
            .Where(x => x.Require.RequiredQuests.Contains(questId));
        foreach (QuestMetadata questMetadata in questList)
        {
            if (session.Player.QuestData.ContainsKey(questMetadata.Basic.Id))
            {
                continue;
            }

            session.Player.QuestData.Add(questMetadata.Basic.Id, new(session.Player.CharacterId, questMetadata));
        }
    }

    private static void HandleCompleteNavigator(GameSession session, PacketReader packet)
    {
        int questId = packet.ReadInt();

        if (!session.Player.QuestData.TryGetValue(questId, out QuestStatus questStatus) || questStatus.State is QuestState.Completed)
        {
            return;
        }

        foreach (QuestRewardItem rewardItem in questStatus.RewardItems)
        {
            Item item = new(rewardItem.Code)
            {
                Amount = rewardItem.Count,
                Rarity = rewardItem.Rank
            };
            session.Player.Inventory.AddItem(session, item, true);
        }

        Condition firstCondition = questStatus.Condition.First();
        firstCondition.Current++;
        firstCondition.Completed = true;

        questStatus.State = QuestState.Completed;
        questStatus.AmountCompleted++;
        questStatus.CompleteTimestamp = TimeInfo.Now();
        DatabaseManager.Quests.Update(questStatus);
        session.Send(QuestPacket.CompleteQuest(questId, false));
    }

    private static void HandleAddExplorationQuests(GameSession session, PacketReader packet)
    {
        int listSize = packet.ReadInt();
        for (int i = 0; i < listSize; i++)
        {
            int questId = packet.ReadInt();
            session.Player.QuestData.TryGetValue(questId, out QuestStatus questStatus);

            if (questStatus is null)
            {
                questStatus = new(session.Player.CharacterId, questId, QuestState.Started, TimeInfo.Now());
                session.Player.QuestData.Add(questId, questStatus);
                session.Send(QuestPacket.AcceptQuest(questStatus));
                continue;
            }

            session.Send(QuestPacket.AcceptQuest(questStatus));
            questStatus.State = QuestState.Started;
            DatabaseManager.Quests.Update(questStatus);
        }
    }

    private static void HandleResumeDungeon(GameSession session, PacketReader packet)
    {
        int questId = packet.ReadInt();

        if (!session.Player.QuestData.TryGetValue(questId, out QuestStatus questStatus) || questStatus.State is QuestState.Completed)
        {
            return;
        }

        QuestMetadata questMetadata = QuestMetadataStorage.GetMetadata(questId);
        session.Player.Warp(questMetadata.ProgressMap.First());
    }

    private static void HandleDispatchMode(GameSession session, PacketReader packet)
    {
        int questId = packet.ReadInt();
        short mode = packet.ReadShort();

        if (!session.Player.QuestData.TryGetValue(questId, out QuestStatus questStatus) || questStatus.State is QuestState.Completed)
        {
            return;
        }

        QuestMetadata questMetadata = QuestMetadataStorage.GetMetadata(questId);
        session.Player.Warp(questMetadata.Dispatch.FieldId);
    }

    private static void HandleToggleTracking(GameSession session, PacketReader packet)
    {
        int questId = packet.ReadInt();
        bool tracked = packet.ReadBool();

        if (!session.Player.QuestData.TryGetValue(questId, out QuestStatus questStatus))
        {
            return;
        }

        questStatus.Tracked = tracked;
        DatabaseManager.Quests.Update(questStatus);
        session.Send(QuestPacket.ToggleTracking(questId, tracked));
    }
}
