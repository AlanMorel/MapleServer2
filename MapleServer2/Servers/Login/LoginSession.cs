using Maple2Storage.Tools;
using MapleServer2.Enums;
using MapleServer2.Network;
using MapleServer2.Packets;

namespace MapleServer2.Servers.Login
{
    public class LoginSession : Session
    {
        protected override SessionType Type => SessionType.Login;

        public long AccountId;
        public int ServerTick;
        public int ClientTick;

        public LoginSession() : base() { }

        public static int GetToken()
        {
            return RandomProvider.Get().Next();
        }

        public override void EndSession()
        {
        }

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
}
