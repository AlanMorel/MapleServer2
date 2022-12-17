using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class GemEquipmentHandler : GamePacketHandler<GemEquipmentHandler>
{
    public override RecvOp OpCode => RecvOp.RequestGemEquipment;

    private enum Mode : byte
    {
        EquipItem = 0x00,
        UnequipItem = 0x01,
        Transparency = 0x03
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        Mode mode = (Mode) packet.ReadByte();

        switch (mode)
        {
            case Mode.EquipItem:
                HandleEquipItem(session, packet);
                break;
            case Mode.UnequipItem:
                HandleUnequipItem(session, packet);
                break;
            case Mode.Transparency:
                HandleTransparency(session, packet);
                break;
            default:
                LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleEquipItem(GameSession session, PacketReader packet)
    {
        if (session.Player.FieldPlayer is null)
        {
            return;
        }

        long itemUid = packet.ReadLong();

        // Remove from inventory
        bool success = session.Player.Inventory.RemoveItem(session, itemUid, out Item item);

        if (!success)
        {
            return;
        }

        // Unequip existing item in slot
        Item?[] badges = session.Player.Inventory.Badges;
        int index = Array.FindIndex(badges, x => x is not null && x.GemSlot == item.GemSlot);
        if (index >= 0)
        {
            // Add to inventory
            Item? badge = badges[index];
            if (badge is null)
            {
                return;
            }

            badge.IsEquipped = false;
            session.Player.Inventory.AddItem(session, badge, false);

            // Unequip
            badges[index] = null;
            session.FieldManager.BroadcastPacket(GemPacket.UnequipItem(session.Player.FieldPlayer.ObjectId, item.GemSlot));
        }

        // Equip
        item.IsEquipped = true;
        int emptyIndex = Array.FindIndex(badges, x => x is null);
        if (emptyIndex == -1)
        {
            return;
        }

        badges[emptyIndex] = item;
        session.FieldManager.BroadcastPacket(GemPacket.EquipItem(session.Player.FieldPlayer.ObjectId, item, emptyIndex));
    }

    private static void HandleUnequipItem(GameSession session, PacketReader packet)
    {
        byte index = packet.ReadByte();

        Item?[] badges = session.Player.Inventory.Badges;

        Item? item = badges[index];
        if (item is null)
        {
            return;
        }

        if (session.Player.FieldPlayer == null)
        {
            return;
        }

        // Add to inventory
        item.IsEquipped = false;
        session.Player.Inventory.AddItem(session, item, false);

        // Unequip
        badges[index] = null;
        session.FieldManager.BroadcastPacket(GemPacket.UnequipItem(session.Player.FieldPlayer.ObjectId, item.GemSlot));
    }

    private static void HandleTransparency(GameSession session, PacketReader packet)
    {
        byte index = packet.ReadByte();
        byte[] transparencyBools = packet.ReadBytes(10);

        Item? item = session.Player.Inventory.Badges[index];

        if (item is null || session.Player.FieldPlayer is null)
        {
            return;
        }

        item.TransparencyBadgeBools = transparencyBools;

        session.FieldManager.BroadcastPacket(GemPacket.EquipItem(session.Player.FieldPlayer.ObjectId, item, index));
    }
}
