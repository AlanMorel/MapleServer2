using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Database;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;

namespace MapleServer2.PacketHandlers.Game
{
    public class UgcHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.UGC;

        public UgcHandler() : base() { }

        private enum UgcMode : byte
        {
            ProfilePicture = 0x0B
        }

        public override void Handle(GameSession session, PacketReader packet)
        {
            UgcMode function = (UgcMode) packet.ReadByte();
            switch (function)
            {
                case UgcMode.ProfilePicture:
                    HandleProfilePicture(session, packet);
                    break;
                default:
                    IPacketHandler<GameSession>.LogUnknownMode(function);
                    break;
            }
        }

        private static void HandleProfilePicture(GameSession session, PacketReader packet)
        {
            string path = packet.ReadUnicodeString();
            session.Player.ProfileUrl = path;
            DatabaseManager.Characters.UpdateProfileUrl(session.Player.CharacterId, path);

            session.FieldManager.BroadcastPacket(UgcPacket.SetProfilePictureURL(session.FieldPlayer.ObjectId, session.Player.CharacterId, path));
        }
    }
}