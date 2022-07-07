using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;

namespace MapleServer2.PacketHandlers.Game;

public class ItemLockHandler : GamePacketHandler<ItemLockHandler>
{
    public override RecvOp OpCode => RecvOp.RequestItemLock;

    private enum Mode : byte
    {
        Open = 0x00,
        Add = 0x01,
        Remove = 0x02,
        Update = 0x03
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        Mode mode = (Mode) packet.ReadByte();
        switch (mode)
        {
            case Mode.Open:
                session.Player.LockInventory = new();
                break;
            case Mode.Add:
                HandleAdd(session, packet);
                break;
            case Mode.Remove:
                HandleRemove(session, packet);
                break;
            case Mode.Update:
                HandleUpdateItem(session, packet);
                break;
            default:
                LogUnknownMode(mode);
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
