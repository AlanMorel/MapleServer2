using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class BreakablePacket
    {
        private enum BreakablePacketMode : byte
        {
            LoadBreakables = 0x0,
            Interact = 0x1,
        }

        public static Packet LoadBreakables(List<Breakable> breakables)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.BREAKABLE);
            pWriter.WriteEnum(BreakablePacketMode.LoadBreakables);
            pWriter.WriteInt(breakables.Count);
            foreach (Breakable breakable in breakables)
            {
                WriteBreakable(pWriter, breakable);
            }
            return pWriter;
        }

        public static Packet Interact(Breakable breakable)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.BREAKABLE);
            pWriter.WriteEnum(BreakablePacketMode.Interact);
            WriteBreakable(pWriter, breakable);
            return pWriter;
        }

        private static void WriteBreakable(PacketWriter pWriter, Breakable breakable)
        {
            pWriter.WriteMapleString(breakable.Id);
            pWriter.WriteEnum(breakable.State);
            pWriter.WriteBool(breakable.IsEnabled);
            pWriter.WriteInt();
            pWriter.WriteInt(); // looks to be some server tick stamp?
        }
    }
}
