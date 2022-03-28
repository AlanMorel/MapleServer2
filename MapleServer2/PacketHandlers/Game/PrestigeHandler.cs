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
        Reward = 0x03
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        PrestigeMode mode = (PrestigeMode) packet.ReadByte();
        switch (mode)
        {
            case PrestigeMode.Reward: // Receive reward
                HandleReward(session, packet);
                break;
            default:
                IPacketHandler<GameSession>.LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleReward(GameSession session, PacketReader packet)
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
}
