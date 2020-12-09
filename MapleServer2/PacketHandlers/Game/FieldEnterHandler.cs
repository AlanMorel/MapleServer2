using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class FieldEnterHandler : GamePacketHandler
    {
        public override ushort OpCode => RecvOp.RESPONSE_FIELD_ENTER;

        public FieldEnterHandler(ILogger<FieldEnterHandler> logger) : base(logger) { }

        public override void Handle(GameSession session, PacketReader packet)
        {
            packet.ReadInt(); // ?

            // Liftable: 00 00 00 00 00
            // SendBreakable
            // Self
            session.EnterField(session.Player.MapId);
            session.Send(FieldObjectPacket.SetStats(session.FieldPlayer));
            session.Send(EmotePacket.LoadEmotes());

            // Normally skill layout would be loaded from a database
            QuickSlot testSkill = QuickSlot.From(10500153);


            if (session.Player.GameOptions.TryGetHotbar(0, out Hotbar mainHotbar))
            {

                mainHotbar.MoveQuickSlot(7, testSkill);
                session.Send(KeyTablePacket.SendHotbars(session.Player.GameOptions));
            }
        }
    }
}