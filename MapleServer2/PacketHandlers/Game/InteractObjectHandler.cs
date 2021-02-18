using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;
using Microsoft.Extensions.Logging;


namespace MapleServer2.PacketHandlers.Game
{
    class InteractObjectHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.INTERACT_OBJECT;

        public InteractObjectHandler(ILogger<InteractObjectHandler> logger) : base(logger) { }

        public override void Handle(GameSession session, PacketReader packet)
        {
            byte mode = packet.ReadByte();  // Action player did to this object

            switch (mode)
            {
                case 0x0B:  // Started interacting with object
                    // We reply with method 0x04
                    HandleInteractStarted(session, packet);
                    break;
                case 0x0C:  // Finished interacting with object
                    HandleInteractFinished(session, packet);
                    break;
            }
        }
        private static void HandleInteractStarted(GameSession session, PacketReader packet)
        {

        }

        private static void HandleInteractFinished(GameSession session, PacketReader packet)
        {

        }
    }
}
