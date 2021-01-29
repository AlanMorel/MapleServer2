using System;
using MapleServer2.Enums;
using MapleServer2.Network;
using Microsoft.Extensions.Logging;

namespace MapleServer2.Servers.Login
{
    public class LoginSession : Session
    {
        protected override SessionType Type => SessionType.Login;

        public long AccountId;
        private readonly Random Rng;

        public LoginSession(ILogger<LoginSession> logger) : base(logger)
        {
            Rng = new Random();
        }

        public int GetToken()
        {
            return Rng.Next();
        }

        public override void EndSession()
        {
        }
    }
}
