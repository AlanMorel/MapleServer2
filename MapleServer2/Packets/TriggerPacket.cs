using System;
using System.Collections.Generic;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets
{
    public static class TriggerPacket
    {
        private enum TriggerPacketMode : byte
        {
            SendTriggers = 0x2,
            SingleTrigger = 0x3,
            Banner = 0x8,
            Timer = 0xE,
        }

        public static Packet SendTriggers(List<TriggerObject> triggerObjects)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.TRIGGER);
            pWriter.WriteEnum(TriggerPacketMode.SendTriggers);
            pWriter.WriteInt(triggerObjects.Count);

            foreach (TriggerObject triggerObject in triggerObjects)
            {
                if (triggerObject is TriggerMesh)
                {
                    Console.WriteLine("triggerObject is TriggerMesh");
                }
            }

            return pWriter;
        }

        public static Packet SetMeshTrigger(int meshId, bool isVisible, float arg5)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.TRIGGER);
            pWriter.WriteEnum(TriggerPacketMode.SingleTrigger);
            pWriter.WriteInt(meshId);
            pWriter.WriteBool(isVisible);
            pWriter.WriteByte(0x00);
            pWriter.WriteFloat(arg5);
            pWriter.WriteInt(0);
            pWriter.WriteShort(16256); //constant: 80 3F
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

        public static Packet MovieTrigger(string path, int movieId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.TRIGGER);
            pWriter.WriteEnum(TriggerPacketMode.Banner);
            pWriter.WriteByte(0x04);
            pWriter.WriteUnicodeString(path);
            pWriter.WriteInt(movieId);
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
