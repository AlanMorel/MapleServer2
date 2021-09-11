using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;

namespace MapleServer2.PacketHandlers.Game
{
    public class ConsumeRideStaminaHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.RIDE_CONSUME_EP;

        public ConsumeRideStaminaHandler() : base() { }

        public override void Handle(GameSession session, PacketReader packet)
        {
            if (session.Player.Mount == null)
            {
                return;
            }

            session.Player.ConsumeStamina(15);
        }
    }
}
