using Maple2Storage.Tools;
using MapleServer2.Enums;
using MapleServer2.Network;
using Microsoft.Extensions.Logging;

namespace MapleServer2.Servers.Login
{
    public class LoginSession : Session
    {
        protected override SessionType Type => SessionType.Login;

        public long AccountId;
        public long CharacterId;

        public LoginSession(ILogger<LoginSession> logger) : base(logger)
        {
        }

        public static int GetToken() => RandomProvider.Get().Next();

        public override void EndSession() { }
    }
}
