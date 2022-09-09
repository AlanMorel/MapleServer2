using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class HomeDoctorHandler : GamePacketHandler<HomeDoctorHandler>
{
    public override RecvOp OpCode => RecvOp.RequestHomeDoctor;

    public override void Handle(GameSession session, PacketReader packet)
    {
        session.Player.HouseDoctorAccessTime = TimeInfo.Now();
    }
}
