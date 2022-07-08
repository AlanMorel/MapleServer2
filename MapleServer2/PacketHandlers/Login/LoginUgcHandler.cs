using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Database;
using MapleServer2.Packets;
using MapleServer2.Servers.Login;

namespace MapleServer2.PacketHandlers.Login;

public class LoginUgcHandler : LoginPacketHandler<LoginUgcHandler>
{
    public override RecvOp OpCode => RecvOp.UGC;

    private enum Mode : byte
    {
        ProfilePicture = 0x0B
    }

    public override void Handle(LoginSession session, PacketReader packet)
    {
        Mode function = (Mode) packet.ReadByte();
        switch (function)
        {
            case Mode.ProfilePicture:
                HandleProfilePicture(session, packet);
                break;
            default:
                LogUnknownMode(function);
                break;
        }
    }

    private static void HandleProfilePicture(LoginSession session, PacketReader packet)
    {
        string path = packet.ReadUnicodeString();
        DatabaseManager.Characters.UpdateProfileUrl(session.CharacterId, path);

        session.Send(UGCPacket.SetProfilePictureUrl(0, session.CharacterId, path));
    }
}
