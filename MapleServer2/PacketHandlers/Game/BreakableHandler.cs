using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class BreakableHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.BREAKABLE;

        public BreakableHandler(ILogger<BreakableHandler> logger) : base(logger) { }

        public override void Handle(GameSession session, PacketReader packet)
        {
            string entityId = packet.ReadMapleString(); //Object hash
            long someId = packet.ReadLong();
            int randId = packet.ReadInt(); //unk
            int unk = packet.ReadInt();

            //TODO: Keep track of broken objects in each field. Whenever a player joins the map send them the state of all of them. Might have to load from game metadata.
            session.FieldManager.BroadcastPacket(BreakablePacket.Break(entityId, 3)); //3 = Break

            //After 3 seconds, send despawn - TODO: Check if some objects shouldn't despawn? (hideTimer in flat mixin SInt32)
            _ = session.FieldManager.DelayBroadcastPacket(BreakablePacket.Break(entityId, 4), 3000);  //4 = Despawn debris

            //After 3 minutes, send respawn - TODO: Get respawn time from metadata if available? (resetTimer in flat mixin SInt32)
            _ = session.FieldManager.DelayBroadcastPacket(BreakablePacket.Break(entityId, 2), 180000); //2 = Respawn
        }
    }
}
