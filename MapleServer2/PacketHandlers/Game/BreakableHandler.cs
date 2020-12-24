using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

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

            //After 3 seconds, send despawn - TODO: Check if some objects shouldn't despawn?
            Despawn(session.FieldManager, BreakablePacket.Break(entityId, 4)); //4 = Despawn debris

            //After 3 minutes, send respawn - TODO: Get respawn time from metadata if available?
            Respawn(session.FieldManager, BreakablePacket.Break(entityId, 2)); //2 = Respawn box
        }

        public async Task Despawn(FieldManager manager, Packet packet)
        {
            await Task.Factory.StartNew(async () =>
            {
                await Task.Delay(3000);
                manager.BroadcastPacket(packet);
            });
        }

        public async Task Respawn(FieldManager manager, Packet packet)
        {
            await Task.Factory.StartNew(async () =>
            {
                await Task.Delay(180000);
                manager.BroadcastPacket(packet);
            });
        }
    }
}
