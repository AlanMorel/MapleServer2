using System.Linq;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class ChangeAttributesScrollHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.CHANGE_ATTRIBUTES_SCROLL;

        public ChangeAttributesScrollHandler(ILogger<GamePacketHandler> logger) : base(logger) { }

        private enum ChangeAttributeMode : byte
        {
            Roll = 1,
            Apply = 3,
        }

        public override void Handle(GameSession session, PacketReader packet)
        {
            ChangeAttributeMode mode = (ChangeAttributeMode) packet.ReadByte();
            switch (mode)
            {
                case ChangeAttributeMode.Roll:
                    HandleChangeStats(session, packet);
                    break;
                case ChangeAttributeMode.Apply:
                    HandleSelectNewStats(session, packet);
                    break;
                default:
                    IPacketHandler<GameSession>.LogUnknownMode(mode);
                    break;
            }
        }

        private static void HandleChangeStats(GameSession session, PacketReader packet)
        {
            long scrollUid = packet.ReadLong();
            long gearUid = packet.ReadLong();

            Inventory inventory = session.Player.Inventory;
            Item scroll = inventory.Items.FirstOrDefault(x => x.Key == scrollUid).Value;
            Item gear = inventory.Items.FirstOrDefault(x => x.Key == gearUid).Value;
            if (scroll == null || gear == null)
            {
                return;
            }
            gear.TimesAttributesChanged++;

            Item newItem = new Item(gear);
            ItemStats.RollNewBonusValues(newItem);
            inventory.TemporaryStorage[newItem.Uid] = newItem;

            InventoryController.Consume(session, scrollUid, 1);
            session.Send(ChangeAttributesScrollPacket.PreviewNewItem(newItem));
        }

        private static void HandleSelectNewStats(GameSession session, PacketReader packet)
        {
            long gearUid = packet.ReadLong();

            Inventory inventory = session.Player.Inventory;
            Item gear = inventory.TemporaryStorage.FirstOrDefault(x => x.Key == gearUid).Value;
            if (gear == null)
            {
                return;
            }

            inventory.TemporaryStorage.Remove(gear.Uid);
            inventory.Replace(gear);
            session.Send(ChangeAttributesScrollPacket.AddNewItem(gear));
        }
    }
}
