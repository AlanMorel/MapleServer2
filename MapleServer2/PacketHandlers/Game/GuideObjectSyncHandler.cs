﻿using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class GuideObjectSync : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.GUIDE_OBJECT_SYNC;

        public GuideObjectSync(ILogger<GuideObjectSync> logger) : base(logger) { }

        public override void Handle(GameSession session, PacketReader packet)
        {
            byte objectType = packet.ReadByte(); // 0 = build, 1 = fish
            byte unk = packet.ReadByte();
            int unk2 = packet.ReadInt(); // this should be sent as individual bytes. Not sure what it's used for
            CoordS coord = packet.Read<CoordS>();
            CoordS unkCoord = packet.Read<CoordS>();
            CoordS rotation = packet.Read<CoordS>();
            short unk3 = packet.ReadShort();
            int unk4 = packet.ReadInt(); // always -1 ?
            int playerTick = packet.ReadInt();
            int playerTick2 = packet.ReadInt(); // packet is given twice for some reason

            if (session.Player.Guide == null)
            {
                return;
            }

            session.Player.Guide.Rotation = rotation.ToFloat();
            session.Player.Guide.Coord = coord.ToFloat();
            //     session.FieldManager.BroadcastPacket(GuideObjectPacket.Sync(session.Player.Guide, unk2, unkCoord));
        }
    }
}
