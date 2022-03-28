using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class PlayerHostHandler : GamePacketHandler
{
    public override RecvOp OpCode => RecvOp.PlayerHost;

    private enum PlayerHostMode : byte
    {
        Claim = 0x1
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        PlayerHostMode mode = (PlayerHostMode) packet.ReadByte();

        switch (mode)
        {
            case PlayerHostMode.Claim:
                HandleClaim(session, packet);
                break;
            default:
                IPacketHandler<GameSession>.LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleClaim(GameSession session, PacketReader packet)
    {
        int hongBaoId = packet.ReadInt();

        HongBao hongBao = GameServer.HongBaoManager.GetHongBaoById(hongBaoId);
        if (hongBao == null)
        {
            return;
        }

        if (hongBao.Active == false)
        {
            session.Send(PlayerHostPacket.HongbaoGiftNotice(session.Player, hongBao, 0));
            return;
        }

        hongBao.AddReceiver(session.Player);
    }
}
