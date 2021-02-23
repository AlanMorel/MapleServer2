using System.Linq;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    class InteractObjectHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.INTERACT_OBJECT;

        public InteractObjectHandler(ILogger<InteractObjectHandler> logger) : base(logger) { }

        private enum InteractObjectMode : byte
        {
            Use = 0x0C,
        }

        public override void Handle(GameSession session, PacketReader packet)
        {
            InteractObjectMode mode = (InteractObjectMode) packet.ReadByte();

            switch (mode)
            {
                case InteractObjectMode.Use:
                    HandleUse(session, packet);
                    break;
            }
        }

        private static void HandleUse(GameSession session, PacketReader packet)
        {
            string uuid = packet.ReadMapleString();
            MapInteractActor actor = MapEntityStorage.GetInteractActors(session.Player.MapId).First(x => x.Uuid == uuid);
            session.Send(InteractActorPacket.UseObject(actor));
            session.Send(InteractActorPacket.Extra(actor));
        }
    }
}
