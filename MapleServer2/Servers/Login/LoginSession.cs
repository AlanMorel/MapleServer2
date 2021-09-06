using Maple2Storage.Tools;
using MapleServer2.Enums;
using MapleServer2.Network;

namespace MapleServer2.Servers.Login
{
    public class LoginSession : Session
    {
        protected override SessionType Type => SessionType.Login;

        public long AccountId;

        public LoginSession() : base() { }

        public static int GetToken()
        {
            return RandomProvider.Get().Next();
        }

        public override void EndSession()
        {
        }
    }
}
