using System;
using System.Linq;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class InsigniaHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.INSIGNIA;

        public InsigniaHandler(ILogger<InsigniaHandler> logger) : base(logger) { }

        public override void Handle(GameSession session, PacketReader packet)
        {

            short insigniaId = packet.ReadShort();

            if (insigniaId < 0 && !InsigniaMetadataStorage.IsValid(insigniaId))
            {
                return;
            }

            session.Player.InsigniaId = insigniaId;
            session.FieldManager.BroadcastPacket(InsigniaPacket.UpdateInsignia(session, insigniaId, CanEquipInsignia(session, insigniaId)));
        }

        private static bool CanEquipInsignia(GameSession session, short insigniaId)
        {
            string type = InsigniaMetadataStorage.GetConditionType(insigniaId);

            switch (type) // TODO: handling survivallevel
            {
                case "vip":
                    return session.Player.IsVip();
                case "level":
                    return session.Player.Levels.Level >= 50;
                case "enchant":
                    return session.Player.Equips.FirstOrDefault(x => x.Value.Enchants >= 12).Value != null;
                case "trophy_point":
                    return session.Player.Trophy[0] + session.Player.Trophy[1] + session.Player.Trophy[2] > 1000;
                case "title":
                    return session.Player.Titles.Contains(InsigniaMetadataStorage.GetTitleId(insigniaId));
                case "adventure_level":
                    return session.Player.Levels.PrestigeLevel >= 100;
                default:
                    Console.WriteLine("Unhandled condition type for insigniaid: " + insigniaId + ", type: " + type);
                    break;
            }
            return false;
        }
    }
}
