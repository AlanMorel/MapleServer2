using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;

namespace MapleServer2.PacketHandlers.Game;

public class RequestItemLockHandler : GamePacketHandler
{
    public override RecvOp OpCode => RecvOp.REQUEST_ITEM_LOCK;

    public RequestItemLockHandler() : base() { }

    private enum ItemLockMode : byte
    {
        Open = 0x00,
        Add = 0x01,
        Remove = 0x02,
        Update = 0x03
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        ItemLockMode mode = (ItemLockMode) packet.ReadByte();
        switch (mode)
        {
            case ItemLockMode.Open:
                session.Player.LockInventory = new();
                break;
            case ItemLockMode.Add:
                HandleAdd(session, packet);
                break;
            case ItemLockMode.Remove:
                HandleRemove(session, packet);
                break;
            case ItemLockMode.Update:
                HandleUpdateItem(session, packet);
                break;
            default:
                IPacketHandler<GameSession>.LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleAdd(GameSession session, PacketReader packet)
    {
        byte mode = packet.ReadByte();
        long uid = packet.ReadLong();

        session.Player.LockInventory.Add(session, mode, uid);
    }

    private static void HandleRemove(GameSession session, PacketReader packet)
    {
        long uid = packet.ReadLong();

        session.Player.LockInventory.Remove(session, uid);
    }

    private static void HandleUpdateItem(GameSession session, PacketReader packet)
    {
        byte mode = packet.ReadByte();

        session.Player.LockInventory.Update(session, mode);
    }
}
