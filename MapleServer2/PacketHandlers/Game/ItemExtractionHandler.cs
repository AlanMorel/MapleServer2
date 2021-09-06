using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game
{
    public class ItemExtractionHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.ITEM_EXTRACTION;

        public ItemExtractionHandler() : base() { }

        public override void Handle(GameSession session, PacketReader packet)
        {
            long anvilItemUid = packet.ReadLong();
            long sourceItemUid = packet.ReadLong();

            if (!session.Player.Inventory.Items.ContainsKey(sourceItemUid) || !session.Player.Inventory.Items.ContainsKey(anvilItemUid))
            {
                return;
            }

            Item sourceItem = session.Player.Inventory.Items[sourceItemUid];

            ItemExtractionMetadata metadata = ItemExtractionMetadataStorage.GetMetadata(sourceItem.Id);
            if (metadata == null)
            {
                return;
            }

            int anvilAmount = 0;
            List<KeyValuePair<long, Item>> anvils = session.Player.Inventory.Items.Where(x => x.Value.Tag == "ItemExtraction").ToList();
            anvils.ForEach(x => anvilAmount += x.Value.Amount);

            if (anvilAmount < metadata.ScrollCount)
            {
                session.Send(ItemExtractionPacket.InsufficientAnvils());
                return;
            }

            Item resultItem = new Item(metadata.ResultItemId)
            {
                Color = sourceItem.Color,
            };

            InventoryController.Consume(session, anvilItemUid, metadata.ScrollCount);
            InventoryController.Add(session, resultItem, true);
            sourceItem.RemainingGlamorForges -= 1;

            session.Send(ItemExtractionPacket.Extract(sourceItem));
        }
    }
}
