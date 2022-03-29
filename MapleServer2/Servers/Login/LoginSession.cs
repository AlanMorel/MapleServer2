using MapleServer2.Enums;
using MapleServer2.Network;
using MapleServer2.Packets;

namespace MapleServer2.Servers.Login;

public class LoginSession : Session
{
    protected override PatchType Type => PatchType.Delete;

    public long AccountId;
    public long CharacterId;
    public int ServerTick;
    public int ClientTick;

    protected override void EndSession(bool logoutNotice) { }

    public Task HeartbeatLoop()
    {
        return Task.Run(async () =>
        {
            while (this != null)
            {
                Send(HeartbeatPacket.Request());
                await Task.Delay(30000); // every 30 seconds
            }
        });
    }
}
