using MaplePacketLib2.Tools;
using MapleServer2.Network;

namespace MapleServer2.PacketHandlers {
    // All implementing classes should be thread safe and stateless.
    // All state should be stored in Session
    public interface IPacketHandler<in T> where T : Session {
        public ushort OpCode { get; }

        public void Handle(T session, PacketReader packet);
    }
}