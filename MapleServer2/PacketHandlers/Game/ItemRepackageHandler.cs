using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class ItemRepackageHandler : GamePacketHandler<ItemRepackageHandler>
{
    public override RecvOp OpCode => RecvOp.ItemRepackage;

    private enum ItemRepackageMode : byte
    {
        Repackage = 0x1
    }

    private enum ItemRepackageNotice
    {
        CannotBePackaged = 0x1,
        ItemInvalid = 0x2,
        CannotRepackageRightNow = 0x3,
        InvalidRarity = 0x4,
        InvalidLevel = 0x5
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        ItemRepackageMode mode = (ItemRepackageMode) packet.ReadByte();

        switch (mode)
        {
            case ItemRepackageMode.Repackage:
                HandleRepackage(session, packet);
                break;
            default:
                LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleRepackage(GameSession session, PacketReader packet)
    {
        long ribbonUid = packet.ReadLong();
        long repackingItemUid = packet.ReadLong();

        Item ribbon = session.Player.Inventory.GetByUid(ribbonUid);
        Item repackingItem = session.Player.Inventory.GetByUid(repackingItemUid);
        if (repackingItem == null || ribbon == null)
        {
            session.Send(ItemRepackagePacket.Notice((int) ItemRepackageNotice.ItemInvalid));
            return;
        }

        if (repackingItem.RemainingTrades != 0 || repackingItem.IsBound())
        {
            session.Send(ItemRepackagePacket.Notice((int) ItemRepackageNotice.CannotBePackaged));
        }

        int ribbonRequirementAmount = ItemMetadataStorage.GetPropertyMetadata(ribbon.Id).RepackageItemConsumeCount;
        if (ribbonRequirementAmount > ribbon.Amount)
        {
            session.Send(ItemRepackagePacket.Notice((int) ItemRepackageNotice.CannotBePackaged));
            return;
        }

        if (!ItemRepackageMetadataStorage.ItemCanRepackage(ribbon.Function.Id, repackingItem.Level, repackingItem.Rarity))
        {
            session.Send(ItemRepackagePacket.Notice((int) ItemRepackageNotice.ItemInvalid));
            return;
        }

        repackingItem.RemainingRepackageCount -= 1;
        repackingItem.RemainingTrades++;

        session.Player.Inventory.ConsumeItem(session, ribbon.Uid, ribbonRequirementAmount);

        session.Send(ItemRepackagePacket.Repackage(repackingItem));
    }
}
