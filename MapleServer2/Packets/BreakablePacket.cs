using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class BreakablePacket
{
    private enum BreakablePacketMode : byte
    {
        LoadBreakables = 0x0,
        Interact = 0x1
    }

    public static PacketWriter LoadBreakables(List<BreakableObject> breakables)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Breakable);
        pWriter.Write(BreakablePacketMode.LoadBreakables);
        pWriter.WriteInt(breakables.Count);
        foreach (BreakableObject breakable in breakables)
        {
            WriteBreakable(pWriter, breakable);
        }
        return pWriter;
    }

    public static PacketWriter Interact(BreakableObject breakable)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Breakable);
        pWriter.Write(BreakablePacketMode.Interact);
        WriteBreakable(pWriter, breakable);
        return pWriter;
    }

    private static void WriteBreakable(PacketWriter pWriter, BreakableObject breakable)
    {
        pWriter.WriteString(breakable.Id);
        pWriter.Write(breakable.State);
        pWriter.WriteBool(breakable.IsEnabled);
        pWriter.WriteInt();
        pWriter.WriteInt(); // looks to be some server tick stamp?
    }
}
