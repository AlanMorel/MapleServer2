using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;

namespace MapleServer2.PacketHandlers.Game;

public class RequestUserEnvHandler : GamePacketHandler
{
    public override RecvOp OpCode => RecvOp.REQUEST_USER_ENV;

    private enum UserEnvMode : byte
    {
        Change = 0x1,
        Trophy = 0x3
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        UserEnvMode mode = (UserEnvMode) packet.ReadByte();

        switch (mode)
        {
            case UserEnvMode.Change:
                HandleTitleChange(session, packet);
                break;
            case UserEnvMode.Trophy:
                HandleTrophy(session);
                break;
            default:
                IPacketHandler<GameSession>.LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleTitleChange(GameSession session, PacketReader packet)
    {
        int titleID = packet.ReadInt();

        if (titleID < 0)
        {
            return;
        }

        session.Player.TitleId = titleID;
        session.FieldManager.BroadcastPacket(UserEnvPacket.UpdateTitle(session, titleID));
    }

    private static void HandleTrophy(GameSession session)
    {
        session.Send(UserEnvPacket.UpdateTrophy());
    }
}
