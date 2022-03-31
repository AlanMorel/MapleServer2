using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Network;
using Serilog;

namespace MapleServer2.PacketHandlers;

// All implementing classes should be thread safe and stateless.
// All state should be stored in Session
public interface IPacketHandler<in T> where T : Session
{
    public RecvOp OpCode { get; }

    public void Handle(T session, PacketReader packet);

    public static void LogUnknownMode(Enum mode)
    {
        Log.Logger.ForContext<T>().Warning("New Unknown {0}: 0x{1}", mode.GetType().Name, mode.ToString("X"));
    }
}
