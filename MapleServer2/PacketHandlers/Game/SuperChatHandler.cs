using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game
{
    public class SuperChatHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.SUPER_WORLDCHAT;

        public SuperChatHandler() : base() { }

        private enum SuperChatMode : byte
        {
            Select = 0x0,
            Deselect = 0x1,
        }

        public override void Handle(GameSession session, PacketReader packet)
        {
            SuperChatMode mode = (SuperChatMode) packet.ReadByte();

            switch (mode)
            {
                case SuperChatMode.Select:
                    HandleSelect(session, packet);
                    break;
                case SuperChatMode.Deselect:
                    HandleDeselect(session);
                    break;
                default:
                    IPacketHandler<GameSession>.LogUnknownMode(mode);
                    break;
            }
        }

        private static void HandleSelect(GameSession session, PacketReader packet)
        {
            int item = packet.ReadInt();

            Item superChatItem = session.Player.Inventory.Items.Values.FirstOrDefault(x => x.Id == item);
            if (superChatItem == null)
            {
                return;
            }

            session.Player.SuperChat = superChatItem.Function.Id;
            session.Send(SuperChatPacket.Select(session.FieldPlayer, superChatItem.Id));
        }

        private static void HandleDeselect(GameSession session)
        {
            session.Send(SuperChatPacket.Deselect(session.FieldPlayer));
        }
    }
}
