using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Database;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class RequestItemStorage : GamePacketHandler
{
    public override RecvOp OpCode => RecvOp.REQUEST_ITEM_STORAGE;

    public RequestItemStorage() : base() { }

    private enum ItemStorageMode : byte
    {
        Add = 0x00,
        Remove = 0x01,
        Move = 0x02,
        Mesos = 0x03,
        Expand = 0x06,
        Sort = 0x08,
        LoadBank = 0x0C,
        Close = 0x0F
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        ItemStorageMode mode = (ItemStorageMode) packet.ReadByte();

        switch (mode)
        {
            case ItemStorageMode.Add:
                HandleAdd(session, packet);
                break;
            case ItemStorageMode.Remove:
                HandleRemove(session, packet);
                break;
            case ItemStorageMode.Move:
                HandleMove(session, packet);
                break;
            case ItemStorageMode.Mesos:
                HandleMesos(session, packet);
                break;
            case ItemStorageMode.Expand:
                HandleExpand(session);
                break;
            case ItemStorageMode.Sort:
                HandleSort(session);
                break;
            case ItemStorageMode.LoadBank:
                HandleLoadBank(session);
                break;
            case ItemStorageMode.Close:
                HandleClose(session.Player.Account.BankInventory);
                break;
            default:
                IPacketHandler<GameSession>.LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleAdd(GameSession session, PacketReader packet)
    {
        packet.ReadLong();
        long uid = packet.ReadLong();
        short slot = packet.ReadShort();
        int amount = packet.ReadInt();

        if (!session.Player.Inventory.Items.ContainsKey(uid))
        {
            return;
        }

        session.Player.Account.BankInventory.Add(session, uid, amount, slot);
    }

    private static void HandleRemove(GameSession session, PacketReader packet)
    {
        packet.ReadLong();
        long uid = packet.ReadLong();
        short destinationSlot = packet.ReadShort();
        int amount = packet.ReadInt();

        if (!session.Player.Account.BankInventory.Remove(session, uid, amount, out Item item))
        {
            return;
        }
        item.Slot = destinationSlot;

        session.Player.Inventory.AddItem(session, item, false);
    }

    private static void HandleMove(GameSession session, PacketReader packet)
    {
        packet.ReadLong();
        long destinationUid = packet.ReadLong();
        short destinationSlot = packet.ReadShort();

        session.Player.Account.BankInventory.Move(session, destinationUid, destinationSlot);
    }

    private static void HandleMesos(GameSession session, PacketReader packet)
    {
        packet.ReadLong();
        byte mode = packet.ReadByte();
        long amount = packet.ReadLong();
        Wallet wallet = session.Player.Wallet;
        BankInventory bankInventory = session.Player.Account.BankInventory;

        if (mode == 1) // add mesos
        {
            if (wallet.Meso.Modify(-amount))
            {
                bankInventory.Mesos.Modify(amount);
            }
            return;
        }

        if (mode == 0) // remove mesos
        {
            if (bankInventory.Mesos.Modify(-amount))
            {
                wallet.Meso.Modify(amount);
            }
        }
    }

    private static void HandleExpand(GameSession session)
    {
        session.Player.Account.BankInventory.Expand(session);
    }

    private static void HandleSort(GameSession session)
    {
        session.Player.Account.BankInventory.Sort(session);
    }

    private static void HandleLoadBank(GameSession session)
    {
        session.Player.Account.BankInventory.LoadBank(session);
    }

    private static void HandleClose(BankInventory bankInventory)
    {
        DatabaseManager.BankInventories.Update(bankInventory);
    }
}
