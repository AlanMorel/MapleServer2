using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class ItemEnchantHandler : GamePacketHandler
{
    public override RecvOp OpCode => RecvOp.REQUEST_ITEM_ENCHANT;

    private enum ItemEnchantMode : byte
    {
        None = 0,
        BeginEnchant = 0x01,
        Ophelia = 0x04,
        Peachy = 0x06,
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        ItemEnchantMode mode = (ItemEnchantMode) packet.ReadByte();

        switch (mode)
        {
            case ItemEnchantMode.None: // Sent when opening up enchant ui
                break;
            case ItemEnchantMode.BeginEnchant:
                HandleBeginEnchant(session, packet);
                break;
            case ItemEnchantMode.Ophelia:
                HandleOpheliaEnchant(session, packet);
                break;
            case ItemEnchantMode.Peachy:
                HandlePeachyEnchant(session, packet);
                break;
            default:
                IPacketHandler<GameSession>.LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleBeginEnchant(GameSession session, PacketReader packet)
    {
        byte type = packet.ReadByte();
        long itemUid = packet.ReadLong();

        if (session.Player.Inventory.Items.TryGetValue(itemUid, out Item item))
        {
            session.Send(ItemEnchantPacket.BeginEnchant(type, item));
        }
    }

    private static void HandleOpheliaEnchant(GameSession session, PacketReader packet)
    {
        long itemUid = packet.ReadLong();

        if (session.Player.Inventory.Items.TryGetValue(itemUid, out Item item))
        {
            item.Enchants += 5;
            item.Charges += 10;
            session.Send(ItemEnchantPacket.EnchantResult(item));
        }
    }

    private static void HandlePeachyEnchant(GameSession session, PacketReader packet)
    {
        long itemUid = packet.ReadLong();

        if (session.Player.Inventory.Items.TryGetValue(itemUid, out Item item))
        {
            item.EnchantExp += 5000;
            if (item.EnchantExp >= 10000)
            {
                item.EnchantExp %= 10000;
                item.Enchants++;
            }

            session.Send(ItemEnchantPacket.EnchantResult(item));
            session.Send(ItemEnchantPacket.UpdateCharges(item));
        }
    }
}
