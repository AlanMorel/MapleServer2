using ICSharpCode.SharpZipLib.Core;
using Maple2Storage.Enums;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class ItemEquipHandler : GamePacketHandler<ItemEquipHandler>
{
    public override RecvOp OpCode => RecvOp.ItemEquip;

    private enum ItemEquipMode : byte
    {
        Equip = 0,
        Unequip = 1
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        ItemEquipMode function = (ItemEquipMode) packet.ReadByte();

        switch (function)
        {
            case ItemEquipMode.Equip:
                HandleEquipItem(session, packet);
                break;
            case ItemEquipMode.Unequip:
                HandleUnequipItem(session, packet);
                break;
            default:
                LogUnknownMode(function);
                break;
        }
    }

    private static void HandleEquipItem(GameSession session, PacketReader packet)
    {
        long itemUid = packet.ReadLong();
        string equipSlotStr = packet.ReadUnicodeString();
        if (!Enum.TryParse(equipSlotStr, out ItemSlot equipSlot))
        {
            Logger.Warning("Unknown equip slot: {equipSlotStr}", equipSlotStr);
            return;
        }

        session.Player.Inventory.TryEquip(session, itemUid, equipSlot);
    }

    private static void HandleUnequipItem(GameSession session, PacketReader packet)
    {
        long itemUid = packet.ReadLong();

        // TODO: do something on false
        session.Player.Inventory.TryUnequip(session, itemUid);
    }
}
