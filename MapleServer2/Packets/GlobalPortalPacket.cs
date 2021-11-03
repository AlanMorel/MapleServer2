using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class GlobalPortalPacket
    {
        private enum GlobalPortalPacketMode : byte
        {
            Notice = 0x0,
            Clear = 0x1,
        }

        public static PacketWriter Notice(GlobalEvent globalEvent)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GLOBAL_PORTAL);
            pWriter.Write(GlobalPortalPacketMode.Notice);
            pWriter.WriteInt(globalEvent.Id);
            pWriter.WriteInt(144); // unk. seems to either be 144 or 145
            pWriter.WriteUnicodeString("s_massive_event_message");
            pWriter.WriteUnicodeString("System_Quiz_Global_Portal"); // SystemSound key
            foreach (GlobalEventType eventMap in globalEvent.Events)
            {
                pWriter.WriteUnicodeString("s_massive_event_name_" + eventMap.ToString());
            }
            return pWriter;
        }

        public static PacketWriter Clear(GlobalEvent globalEvent)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GLOBAL_PORTAL);
            pWriter.Write(GlobalPortalPacketMode.Clear);
            pWriter.WriteInt(globalEvent.Id);
            return pWriter;
        }
    }
}
