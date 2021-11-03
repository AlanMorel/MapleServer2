using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game
{
    public class BreakableHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.BREAKABLE;

        public BreakableHandler() : base() { }

        public override void Handle(GameSession session, PacketReader packet)
        {
            string entityId = packet.ReadString();
            long someId = packet.ReadLong();
            int randId = packet.ReadInt(); //unk
            int unk = packet.ReadInt();

            BreakableActorObject breakable = session.FieldManager.State.BreakableActors.GetValueOrDefault(entityId);
            if (breakable == null)
            {
                return;
            }

            breakable.BreakObject(session.FieldManager);
        }
    }
}
