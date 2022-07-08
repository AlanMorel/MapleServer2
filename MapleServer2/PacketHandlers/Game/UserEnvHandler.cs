using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;

namespace MapleServer2.PacketHandlers.Game;

public class UserEnvHandler : GamePacketHandler<UserEnvHandler>
{
    public override RecvOp OpCode => RecvOp.RequestUserEnvironment;

    private enum Mode : byte
    {
        Change = 0x1,
        Trophy = 0x3
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        Mode mode = (Mode) packet.ReadByte();

        switch (mode)
        {
            case Mode.Change:
                HandleTitleChange(session, packet);
                break;
            case Mode.Trophy:
                HandleTrophy(session);
                break;
            default:
                LogUnknownMode(mode);
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
