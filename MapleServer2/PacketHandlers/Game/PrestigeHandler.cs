using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class PrestigeHandler : GamePacketHandler<PrestigeHandler>
{
    public override RecvOp OpCode => RecvOp.Prestige;

    private enum Mode : byte
    {
        ClaimPerk = 0x03,
        ClaimMissionReward = 0x05
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        Mode mode = (Mode) packet.ReadByte();
        switch (mode)
        {
            case Mode.ClaimPerk:
                HandleClaimPerk(session, packet);
                break;
            case Mode.ClaimMissionReward:
                HandleClaimMissionReward(session, packet);
                break;
            default:
                LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleClaimPerk(GameSession session, PacketReader packet)
    {
        int rank = packet.ReadInt();

        if (session.Player.Account.PrestigeRewardsClaimed.Contains(rank))
        {
            return;
        }

        // Get reward data
        PrestigeRewardMetadata reward = PrestigeRewardMetadataStorage.GetReward(rank);

        switch (reward.Type)
        {
            case "item":
                Item item = new(reward.Id, reward.Amount,reward.Rarity);
                session.Player.Inventory.AddItem(session, item, true);
                break;
            case "statPoint":
                session.Player.AddStatPoint(reward.Amount, OtherStatsIndex.Prestige);
                //TODO: Give stat points to all characters.
                break;
        }

        session.Send(PrestigePacket.Reward(rank));
        session.Player.Account.PrestigeRewardsClaimed.Add(rank);
    }

    private static void HandleClaimMissionReward(GameSession session, PacketReader packet)
    {
        int missionId = packet.ReadInt();
        PrestigeLevelMissionMetadata metadata = PrestigeLevelMissionMetadataStorage.GetMetadata(missionId);
        if (metadata is null)
        {
            return;
        }

        PrestigeMission mission = session.Player.Account.PrestigeMissions.FirstOrDefault(x => x.Id == missionId);
        if (mission is null || mission.Claimed || mission.LevelCount < metadata.MissionCount)
        {
            return;
        }

        mission.Claimed = true;
        Item reward = new(metadata.RewardItemId, metadata.RewardItemAmount, metadata.RewardItemRarity);

        session.Player.Inventory.AddItem(session, reward, true);
        session.Send(PrestigePacket.UpdateMissions(session.Player.Account.PrestigeMissions));
    }
}
