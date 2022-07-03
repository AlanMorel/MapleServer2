using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.PacketHandlers.Game.Helpers;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class ItemSocketScrollHandler : GamePacketHandler<ItemSocketScrollHandler>
{
    public override RecvOp OpCode => RecvOp.ItemSocketScroll;

    private enum ItemSocketScrollMode : byte
    {
        UseScroll = 0x1
    }

    private enum ItemSocketScrollError
    {
        NotEligibleItem = 0x1,
        SelectedItemsNoLongerValid = 0x2,
        SelectedItemCannotUseScroll = 0x3,
        SelectedItemCannotHaveMoreSockets = 0x4,
        ItemRarityNotValid = 0x6,
        ItemLevelNotValid = 0x7,
        FailedToUseScroll = 0x8,
        SocketActivationFailed = 0x9
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        ItemSocketScrollMode mode = (ItemSocketScrollMode) packet.ReadByte();

        switch (mode)
        {
            case ItemSocketScrollMode.UseScroll:
                HandleUseScroll(session, packet);
                break;
            default:
                LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleUseScroll(GameSession session, PacketReader packet)
    {
        long equipUid = packet.ReadLong();
        long scrollUid = packet.ReadLong();

        if (!session.Player.Inventory.HasItem(equipUid) && !session.Player.Inventory.HasItem(scrollUid))
        {
            session.Send(ItemSocketScrollPacket.Error((int) ItemSocketScrollError.SelectedItemsNoLongerValid));
            return;
        }

        Item scroll = session.Player.Inventory.GetByUid(scrollUid);
        Item equip = session.Player.Inventory.GetByUid(equipUid);
        ItemSocketScrollMetadata metadata = ItemSocketScrollMetadataStorage.GetMetadata(scroll.Function.Id);
        if (metadata is null)
        {
            session.Send(ItemSocketScrollPacket.Error((int) ItemSocketScrollError.SelectedItemCannotUseScroll));
            return;
        }

        if (metadata.Rarity != equip.Rarity)
        {
            session.Send(ItemSocketScrollPacket.Error((int) ItemSocketScrollError.ItemRarityNotValid));
            return;
        }

        if (metadata.MinLevel > equip.Level || metadata.MaxLevel < equip.Level)
        {
            session.Send(ItemSocketScrollPacket.Error((int) ItemSocketScrollError.ItemLevelNotValid));
            return;
        }

        if (!metadata.ItemTypes.Contains(equip.Type))
        {
            session.Send(ItemSocketScrollPacket.Error((int) ItemSocketScrollError.NotEligibleItem));
            return;
        }

        byte socketCount = ItemSocketScrollHelper.GetSocketCount(metadata.Id);
        int successRate = (int) ItemSocketScrollHelper.GetSuccessRate(metadata.Id) * 10000;

        if (equip.GemSockets.Sockets.Count(x => x.IsUnlocked) >= socketCount)
        {
            session.Send(ItemSocketScrollPacket.Error((int) ItemSocketScrollError.SelectedItemCannotHaveMoreSockets));
            return;
        }

        Random random = Random.Shared;
        int randomValue = random.Next(0, 10000 + 1);
        bool scrollSuccess = successRate >= randomValue;

        if (scrollSuccess)
        {
            for (int i = 0; i < socketCount; i++)
            {
                if (equip.GemSockets[i].IsUnlocked)
                {
                    continue;
                }
                equip.GemSockets[i].IsUnlocked = true;
            }

            if (metadata.MakeUntradeable)
            {
                equip.RemainingTrades = 0;
            }
        }

        session.Player.Inventory.ConsumeItem(session, scrollUid, 1);
        session.Send(ItemSocketScrollPacket.UseScroll(equip, successRate, scrollSuccess));
    }
}
