using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class HomeActionHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.HOME_ACTION;

        public HomeActionHandler(ILogger<HomeActionHandler> logger) : base(logger) { }

        private enum HomeActionMode : byte
        {
            Smite = 0x01,
            Kick = 0x02,
            PortalCube = 0x0D
        }

        public override void Handle(GameSession session, PacketReader packet)
        {
            HomeActionMode mode = (HomeActionMode) packet.ReadByte();

            switch (mode)
            {
                case HomeActionMode.Kick:
                    HandleKick(packet);
                    break;
            }
        }

        private static void HandleKick(PacketReader packet)
        {
            string characterName = packet.ReadUnicodeString();
            Player target = GameServer.Storage.GetPlayerByName(characterName);
            if (target == null)
            {
                return;
            }

            target.Warp(target.ReturnMapId, target.ReturnCoord, target.Rotation, 0);
            target.ReturnMapId = 0;
            target.VisitingHomeId = 0;
        }
    }
}
