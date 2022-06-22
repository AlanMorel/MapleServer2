using Maple2Storage.Enums;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;

namespace MapleServer2.PacketHandlers.Game;

internal class RideConsumeEpHandler : GamePacketHandler<RideConsumeEpHandler>
{
    public override RecvOp OpCode => RecvOp.RideConsumeEp;

    public override void Handle(GameSession session, PacketReader packet)
    {
        session.Player.FieldPlayer.ConsumeStamina(Constant.MountStaminaConsumption);
        session.Send(StatPacket.UpdateStats(session.Player.FieldPlayer, StatAttribute.Stamina));
    }
}
