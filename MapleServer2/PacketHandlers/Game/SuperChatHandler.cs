using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class SuperChatHandler : GamePacketHandler<SuperChatHandler>
{
    public override RecvOp OpCode => RecvOp.SuperWorldChat;

    private enum SuperChatMode : byte
    {
        Select = 0x0,
        Deselect = 0x1
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        SuperChatMode mode = (SuperChatMode) packet.ReadByte();

        switch (mode)
        {
            case SuperChatMode.Select:
                HandleSelect(session, packet);
                break;
            case SuperChatMode.Deselect:
                HandleDeselect(session);
                break;
            default:
                LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleSelect(GameSession session, PacketReader packet)
    {
        int itemId = packet.ReadInt();

        Item superChatItem = session.Player.Inventory.GetById(itemId);
        if (superChatItem == null)
        {
            return;
        }

        session.Player.SuperChatId = superChatItem.Function.Id;
        session.Send(SuperChatPacket.Select(session.Player.FieldPlayer, superChatItem.Id));
    }

    private static void HandleDeselect(GameSession session)
    {
        session.Send(SuperChatPacket.Deselect(session.Player.FieldPlayer));
    }
}
