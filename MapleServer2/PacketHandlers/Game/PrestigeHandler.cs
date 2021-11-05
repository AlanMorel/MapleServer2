using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class PrestigeHandler : GamePacketHandler
{
    public override RecvOp OpCode => RecvOp.PRESTIGE;

    public PrestigeHandler() : base() { }

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

        if (reward.Type.Equals("item"))
        {
            Item item = new(reward.Id)
            {
                CreationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(), Rarity = 4
            };

            session.Player.Inventory.AddItem(session, item, true);
        }
        else if (reward.Type.Equals("statPoint"))
        {
            session.Player.StatPointDistribution.AddTotalStatPoints(reward.Value);
        }

        session.Send(PrestigePacket.Reward(rank));
        session.Player.PrestigeRewardsClaimed.Add(rank);
    }
}
