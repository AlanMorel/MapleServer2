using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets
{
    public static class TriggerPacket
    {
        private enum TriggerPacketMode : byte
        {
            Trigger = 0x3,
            Banner = 0x8,
            Timer = 0xE,
        }

        public static Packet Trigger(int arg1, byte arg2)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.TRIGGER);
            pWriter.WriteEnum(TriggerPacketMode.Trigger);
            pWriter.WriteInt(arg1);
            pWriter.WriteByte(arg2);
            pWriter.WriteByte();
            pWriter.WriteInt();
            return pWriter;
        }

        public static Packet Banner(byte state, int stringGuideId, int time)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.TRIGGER);
            pWriter.WriteEnum(TriggerPacketMode.Banner);
            pWriter.WriteByte(state); // 02 = on, 03 = off
            pWriter.WriteInt(stringGuideId);
            pWriter.WriteInt(stringGuideId);
            pWriter.WriteInt(time); //display duration in ms
            return pWriter;
        }

        public static Packet Timer(int time, bool startCountdown)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.TRIGGER);
            pWriter.WriteEnum(TriggerPacketMode.Timer);
            pWriter.WriteByte();
            pWriter.WriteInt();
            pWriter.WriteInt(time); // in ms
            pWriter.WriteBool(startCountdown); // maybe?
            pWriter.WriteInt();
            pWriter.WriteShort();
            return pWriter;
        }
    }
}
