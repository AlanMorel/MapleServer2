using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;

namespace MapleServer2.PacketHandlers.Game
{
    public class UgcHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.UGC;

        public UgcHandler() : base() { }

        public override void Handle(GameSession session, PacketReader packet)
        {
            byte function = packet.ReadByte();
            if (function == 0x12)
            {
                //session.Send(PacketWriter.Of(SendOp.UGC).WriteByte(0x12).WriteZero(12));
            }
        }
    }
}
