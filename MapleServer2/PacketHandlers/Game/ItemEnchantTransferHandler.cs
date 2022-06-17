using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class ItemEnchantTransferHandler : GamePacketHandler<ItemEnchantTransferHandler>
{
    public override RecvOp OpCode => RecvOp.ItemEnchantTransform;

    private enum ItemEnchantTransferMode : byte
    {
        Convert = 0x1,
        UseSticker = 0x3,
        GroupChatSticker = 0x4,
        Favorite = 0x5,
        Unfavorite = 0x6
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        long itemUid = packet.ReadLong();

        Item item = session.Player.Inventory.GetByUid(itemUid);
        if (item is null)
        {
            return;
        }

        ItemEnchantTransferMetadata metadata = ItemEnchantTransferMetadataStorage.GetMetadata(item.Rarity, item.Id, item.EnchantLevel);
        if (metadata is null)
        {
            return;
        }

        // Remove Enchant
        item.EnchantLevel = 0;
        item.Stats.Enchants = new();

        Item scroll = new(metadata.OutputItemId, metadata.OutputAmount, metadata.OutputRarity);

        session.Player.Inventory.AddItem(session, scroll, true);
        session.Send(ItemInventoryPacket.UpdateItem(item));
        session.Send(ItemEnchantTransferPacket.Transfer());
    }
}
