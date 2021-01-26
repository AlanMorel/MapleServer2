using System;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class PrestigeHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.PRESTIGE;

        public PrestigeHandler(ILogger<PrestigeHandler> logger) : base(logger) { }

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

        private void HandleReward(GameSession session, PacketReader packet)
        {
            int rank = packet.ReadInt();

            session.Send(PrestigePacket.Reward(rank));

            // Get reward data
            PrestigeReward reward = PrestigeMetadataStorage.GetReward(rank);

            if (reward.Type.Equals("item"))
            {
                Item item = new Item(reward.Id)
                {
                    CreationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    Rarity = 4
                };

                InventoryController.Add(session, item, true);
            }
            else if (reward.Type.Equals("statPoint"))
            {
                session.Player.StatPointDistribution.AddTotalStatPoints(reward.Value);
            }
        }
    }
}
