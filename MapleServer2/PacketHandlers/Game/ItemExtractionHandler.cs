using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class ItemExtractionHandler : GamePacketHandler
{
    public override RecvOp OpCode => RecvOp.ItemExtraction;

    public override void Handle(GameSession session, PacketReader packet)
    {
        long anvilItemUid = packet.ReadLong();
        long sourceItemUid = packet.ReadLong();

        if (!session.Player.Inventory.HasItem(sourceItemUid) || !session.Player.Inventory.HasItem(anvilItemUid))
        {
            return;
        }

        Item sourceItem = session.Player.Inventory.GetByUid(sourceItemUid);

        ItemExtractionMetadata metadata = ItemExtractionMetadataStorage.GetMetadata(sourceItem.Id);
        if (metadata == null)
        {
            return;
        }

        IReadOnlyCollection<Item> anvils = session.Player.Inventory.GetAllByTag("ItemExtraction");
        int anvilAmount = anvils.Sum(x => x.Amount);

        if (anvilAmount < metadata.ScrollCount)
        {
            session.Send(ItemExtractionPacket.InsufficientAnvils());
            return;
        }

        Item resultItem = new(metadata.ResultItemId)
        {
            Color = sourceItem.Color
        };

        session.Player.Inventory.ConsumeItem(session, anvilItemUid, metadata.ScrollCount);
        session.Player.Inventory.AddItem(session, resultItem, true);
        sourceItem.RemainingGlamorForges -= 1;

        session.Send(ItemExtractionPacket.Extract(sourceItem));
    }
}
