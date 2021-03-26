﻿using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class StateHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.STATE;

        public StateHandler(ILogger<StateHandler> logger) : base(logger) { }

        private enum StateHandlerMode : byte
        {
            Jump = 0x0,
            Land = 0x1
        };

        public override void Handle(GameSession session, PacketReader packet)
        {
            StateHandlerMode mode = (StateHandlerMode) packet.ReadByte();

            switch (mode)
            {
                case StateHandlerMode.Jump:
                    HandleJump(session);
                    break;
                case StateHandlerMode.Land:
                    break;
            }
        }

        private static void HandleJump(GameSession session)
        {
            session.Player.TrophyUpdate(22100012, 1, 5);
        }
    }
}
