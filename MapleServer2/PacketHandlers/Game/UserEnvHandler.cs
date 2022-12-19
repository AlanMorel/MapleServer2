using System.Diagnostics;
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
        int titleId = packet.ReadInt();

        if (titleId < 0)
        {
            return;
        }
        Debug.Assert(session.Player.FieldPlayer != null, "session.Player.FieldPlayer != null");

        session.Player.TitleId = titleId;
        session.FieldManager.BroadcastPacket(UserEnvPacket.UpdateTitle(session.Player.FieldPlayer.ObjectId, titleId));
    }

    private static void HandleTrophy(GameSession session)
    {
        session.Send(UserEnvPacket.UpdateTrophy());
    }
}
