using System;
using System.Text;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Extensions;
using MapleServer2.Network;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Common
{
    // Note: socket_exception debug offset includes +6 bytes from encrypted header
    public class LogSendHandler : CommonPacketHandler
    {
        public override RecvOp OpCode => RecvOp.LOG_SEND;

        public LogSendHandler(ILogger<LogSendHandler> logger) : base(logger) { }

        protected override void HandleCommon(Session session, PacketReader packet)
        {
            packet.ReadByte();
            byte function = packet.ReadByte();
            if (function == 1)
            {
                // Some random data that isn't text...
                // Example: 56 00 00 01 03 03 00 66 70 73 9B D2 6A 42 29 73 07 44 A3 45 00 00 00 00 00 00 00 00 70 42 03 00 6D 65 6D BC 2E 01 45 B4 FA B3 43 A3 45 00 00 00 A0 FE 44 00 80 01 45 03 00 6C 61 74 00 00 00 00 00 00 00 00 A3 45 00 00 00 00 00 00 00 00 00 00
                return;
            }
            try
            {
                StringBuilder builder = new StringBuilder();
                while (packet.Available > 2)
                {
                    string message = packet.ReadUnicodeString();
                    if (message.Contains("exception"))
                    {
                        // Read remaining string
                        string debug = packet.ReadUnicodeString(packet.Available / 2);
                        logger.Error($"[{message}] {debug}");

                        session.OnError?.Invoke(session, debug);
                        return;
                    }

                    builder.Append(message);
                }
                logger.Warning($"Client Log: {builder}");
            }
            catch (Exception ex)
            {
                logger.Error($"Error parsing DEBUG_MSG packet:{packet} f({function})", ex);
            }
        }
    }
}
