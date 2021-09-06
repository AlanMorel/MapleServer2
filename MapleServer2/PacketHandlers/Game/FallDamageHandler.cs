using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;

namespace MapleServer2.PacketHandlers.Game
{
    public class FallDamageHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.STATE_FALL_DAMAGE;

        public FallDamageHandler() : base() { }

        public override void Handle(GameSession session, PacketReader packet)
        {
            if (session.Player.OnAirMount)
            {
                session.Player.OnAirMount = false;
            }
        }
    }
}
