using System;
using Maple2Storage.Enums;
using MapleServer2.Network;
using Microsoft.Extensions.Logging;
using Maple2.Data.Storage;

namespace MapleServer2.Servers.Login {
    public class LoginSession : Session {
        protected override SessionType Type => SessionType.Login;

        public long AccountId;
        private readonly Random rng;


        public LoginSession(ILogger<LoginSession> logger) : base(logger) {
            this.rng = new Random();

        }

        public int GetToken() {
            return rng.Next();
        }
    }
}