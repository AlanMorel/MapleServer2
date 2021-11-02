using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;

namespace MapleServer2.PacketHandlers.Game
{
    public class FallDamageHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.STATE_FALL_DAMAGE;

        public FallDamageHandler() : base() { }

        public override void Handle(GameSession session, PacketReader packet)
        {
            float distance = packet.ReadFloat();

            if (distance > Block.BLOCK_SIZE * 6)
            {
                // TODO: Check if player has Safe ride enabled
                if (session.Player.Mount != null)
                {
                    session.FieldManager.BroadcastPacket(MountPacket.StopRide(session.FieldPlayer, false));
                }

                session.Player.FallDamage();
            }

            if (session.Player.OnAirMount)
            {
                session.Player.OnAirMount = false;
            }
        }
    }
}
