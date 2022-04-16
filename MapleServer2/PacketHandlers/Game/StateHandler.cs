using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Managers;
using MapleServer2.Servers.Game;

namespace MapleServer2.PacketHandlers.Game;

public class StateHandler : GamePacketHandler<StateHandler>
{
    public override RecvOp OpCode => RecvOp.State;

    private enum StateHandlerMode : byte
    {
        Jump = 0x0,
        Land = 0x1
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        StateHandlerMode mode = (StateHandlerMode) packet.ReadByte();

        switch (mode)
        {
            case StateHandlerMode.Jump:
                HandleJump(session);
                break;
            case StateHandlerMode.Land:
                break;
            default:
                LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleJump(GameSession session)
    {
        TrophyManager.OnJump(session.Player);
    }
}
