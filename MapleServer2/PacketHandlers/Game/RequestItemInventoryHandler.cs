using Maple2Storage.Enums;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class RequestItemInventoryHandler : GamePacketHandler
{
    public override RecvOp OpCode => RecvOp.REQUEST_ITEM_INVENTORY;

    private enum RequestItemInventoryMode : byte
    {
        Move = 0x3,
        Drop = 0x4,
        DropBound = 0x5,
        Sort = 0xA,
        Expand = 0xB
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        RequestItemInventoryMode mode = (RequestItemInventoryMode) packet.ReadByte();

        switch (mode)
        {
            case RequestItemInventoryMode.Move:
                HandleMove(session, packet);
                break;
            case RequestItemInventoryMode.Drop:
                HandleDrop(session, packet);
                break;
            case RequestItemInventoryMode.DropBound:
                HandleDropBound(session, packet);
                break;
            case RequestItemInventoryMode.Sort:
                HandleSort(session, packet);
                break;
            case RequestItemInventoryMode.Expand:
                HandleExpand(session, packet);
                break;
            default:
                IPacketHandler<GameSession>.LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleMove(GameSession session, PacketReader packet)
    {
        long uid = packet.ReadLong(); // Grabs incoming item packet uid
        short dstSlot = packet.ReadShort(); // Grabs incoming item packet slot
        session.Player.Inventory.MoveItem(session, uid, dstSlot);
    }

    private static void HandleDrop(GameSession session, PacketReader packet)
    {
        long uid = packet.ReadLong();
        int amount = packet.ReadInt(); // Grabs incoming item packet amount
        if (!session.Player.Inventory.HasItem(uid))
        {
            return;
        }
        
        Item item = session.Player.Inventory.GetByUid(uid);
        session.Player.Inventory.DropItem(session, item, amount);
    }

    private static void HandleDropBound(GameSession session, PacketReader packet)
    {
        long uid = packet.ReadLong();
        if (!session.Player.Inventory.HasItem(uid))
        {
            return;
        }
        
        Item item = session.Player.Inventory.GetByUid(uid);
        session.Player.Inventory.DropItem(session, item, item.Amount);
    }

    private static void HandleSort(GameSession session, PacketReader packet)
    {
        InventoryTab tab = (InventoryTab) packet.ReadShort();
        session.Player.Inventory.SortInventory(session, tab);
    }

    private static void HandleExpand(GameSession session, PacketReader packet)
    {
        InventoryTab tab = (InventoryTab) packet.ReadByte();
        session.Player.Inventory.ExpandInventory(session, tab);
    }
}
