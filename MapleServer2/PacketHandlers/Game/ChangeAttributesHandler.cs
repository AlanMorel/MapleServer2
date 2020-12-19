using System;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game {
    public class ChangeAttributesHandler : GamePacketHandler {
        public override RecvOp OpCode => RecvOp.CHANGE_ATTRIBUTES;

        private const string NEW_ITEM_KEY = "new_item_key";

        public ChangeAttributesHandler(ILogger<GamePacketHandler> logger) : base(logger) { }

        public override void Handle(GameSession session, PacketReader packet) {
            byte function = packet.ReadByte();

            switch (function) {
                case 0:
                    HandleChangeAttributes(session, packet);
                    break;
                case 2:
                    HandleSelectNewAttributes(session, packet);
                    break;
            }
        }

        private void HandleChangeAttributes(GameSession session, PacketReader packet) {
            short lockIndex = -1;
            long itemUid = packet.ReadLong();
            packet.Skip(8);
            bool useLock = packet.ReadBool();
            if (useLock) {
                packet.Skip(1);
                lockIndex = packet.ReadShort();
            }

            if (session.Player.Inventory.Items.TryGetValue(itemUid, out Item item)) {
                item.TimesAttributesChanged++;
                var newItem = new Item(item);
                int attributeCount = newItem.Stats.BonusAttributes.Count;
                var rng = new Random();
                for (int i = 0; i < attributeCount; i++) {
                    if (i == lockIndex) continue;
                    // TODO: Don't RNG the same attribute twice
                    newItem.Stats.BonusAttributes[i] = ItemStat.Of((ItemAttribute) rng.Next(35), 0.01f);
                }

                session.StateStorage[NEW_ITEM_KEY] = newItem;
                session.Send(ChangeAttributesPacket.PreviewNewItem(newItem));
            }
        }

        private void HandleSelectNewAttributes(GameSession session, PacketReader packet) {
            long itemUid = packet.ReadLong();

            if (session.StateStorage.TryGetValue(NEW_ITEM_KEY, out object obj)) {
                if (!(obj is Item item) || itemUid != item.Uid) {
                    return;
                }

                session.Player.Inventory.Replace(item);
                session.Send(ChangeAttributesPacket.SelectNewItem(item));
            }
        }
    }
}