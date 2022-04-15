using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class PrestigeHandler : GamePacketHandler
{
    public override RecvOp OpCode => RecvOp.Prestige;

    private enum PrestigeMode : byte
    {
        ClaimPerk = 0x03,
        ClaimMissionReward = 0x05
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        PrestigeMode mode = (PrestigeMode) packet.ReadByte();
        switch (mode)
        {
            case PrestigeMode.ClaimPerk:
                HandleClaimPerk(session, packet);
                break;
            case PrestigeMode.ClaimMissionReward:
                HandleClaimMissionReward(session, packet);
                break;
            default:
                IPacketHandler<GameSession>.LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleClaimPerk(GameSession session, PacketReader packet)
    {
        int rank = packet.ReadInt();

        if (session.Player.PrestigeRewardsClaimed.Contains(rank))
        {
            return;
        }

        // Get reward data
        PrestigeReward reward = PrestigeMetadataStorage.GetReward(rank);

        switch (reward.Type)
        {
            case "item":
                Item item = new(reward.Id)
                {
                    Rarity = 4
                };

                session.Player.Inventory.AddItem(session, item, true);
                break;
            case "statPoint":
                session.Player.AddStatPoint(reward.Value, OtherStatsIndex.Trophy);
                break;
        }

        session.Send(PrestigePacket.Reward(rank));
        session.Player.PrestigeRewardsClaimed.Add(rank);
    }

    private static void HandleClaimMissionReward(GameSession session, PacketReader packet)
    {
        int missionId = packet.ReadInt();
        PrestigeLevelMissionMetadata metadata = PrestigeLevelMissionMetadataStorage.GetMetadata(missionId);
        if (metadata is null)
        {
            return;
        }

        PrestigeMission mission = session.Player.PrestigeMissions.FirstOrDefault(x => x.Id == missionId);
        if (mission is null || mission.Claimed || mission.LevelCount < metadata.MissionCount)
        {
            return;
        }

        mission.Claimed = true;
        Item reward = new(metadata.RewardItemId, metadata.RewardItemAmount, metadata.RewardItemRarity);

        session.Player.Inventory.AddItem(session, reward, true);
        session.Send(PrestigePacket.UpdateMissions(session.Player.PrestigeMissions));
    }
}
