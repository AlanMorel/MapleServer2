using System;
using System.Collections.Generic;
using System.Linq;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class ChangeAttributesHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.CHANGE_ATTRIBUTES;

        public ChangeAttributesHandler(ILogger<GamePacketHandler> logger) : base(logger) { }

        public override void Handle(GameSession session, PacketReader packet)
        {
            byte function = packet.ReadByte();

            switch (function)
            {
                case 0:
                    HandleChangeStats(session, packet);
                    break;
                case 2:
                    HandleSelectNewStats(session, packet);
                    break;
            }
        }

        private static void HandleChangeStats(GameSession session, PacketReader packet)
        {
            short lockStatId = -1;
            long itemUid = packet.ReadLong();
            packet.Skip(8);
            bool useLock = packet.ReadBool();
            if (useLock)
            {
                packet.Skip(1);
                lockStatId = packet.ReadShort();
            }

            Inventory inventory = session.Player.Inventory;
            Item gear = inventory.Items.FirstOrDefault(x => x.Key == itemUid).Value;
            if (gear == null)
            {
                return;
            }

            gear.TimesAttributesChanged++;

            Random random = new Random();
            Item newItem = new Item(gear);

            // Get random stats ignoring stat that is locked
            List<ItemStat> randomList = ItemStats.RollBonusStats(newItem.Id, newItem.Rarity, newItem.Stats.BonusStats.Count, lockStatId);

            for (int i = 0; i < newItem.Stats.BonusStats.Count; i++)
            {
                if (newItem.Stats.BonusStats[i].GetId() == lockStatId)
                {
                    continue;
                }

                newItem.Stats.BonusStats[i] = randomList[i];
            }

            inventory.TemporaryStorage[newItem.Uid] = newItem;
            session.Send(ChangeAttributesPacket.PreviewNewItem(newItem));
        }

        private static void HandleSelectNewStats(GameSession session, PacketReader packet)
        {
            long itemUid = packet.ReadLong();

            Inventory inventory = session.Player.Inventory;
            Item gear = inventory.TemporaryStorage.FirstOrDefault(x => x.Key == itemUid).Value;
            if (gear == null)
            {
                return;
            }

            inventory.TemporaryStorage.Remove(itemUid);
            inventory.Replace(gear);
            session.Send(ChangeAttributesPacket.AddNewItem(gear));
        }
    }
}
