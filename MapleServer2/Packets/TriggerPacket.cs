using System;
using System.Collections.Generic;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

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

        public static Packet SendTriggerObjects(List<TriggerObject> triggerObjects)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.TRIGGER);
            pWriter.WriteEnum(TriggerPacketMode.SendTriggers);
            pWriter.WriteInt(triggerObjects.Count);

            foreach (TriggerObject triggerObject in triggerObjects)
            {
                if (triggerObject is TriggerMesh)
                {
                    TriggerMesh triggerMesh = (TriggerMesh) triggerObject;
                    //pWriter.Write(SetMeshTrigger(triggerMesh.Id, triggerMesh.IsVisible, 0));
                    pWriter.WriteInt(triggerMesh.Id);
                    pWriter.WriteBool(triggerMesh.IsVisible);
                    pWriter.WriteByte(0x00);
                    pWriter.WriteInt(2);
                    pWriter.WriteInt(0);
                    pWriter.WriteShort(16256); //constant: 80 3F
                }

                if (triggerObject is TriggerEffect)
                {
                    TriggerEffect triggerEffect = (TriggerEffect) triggerObject;
                    pWriter.WriteInt(triggerEffect.Id);
                    pWriter.WriteBool(triggerEffect.IsVisible);
                    pWriter.WriteByte(0x00);
                    pWriter.WriteInt(3); //not sure where this value is coming from.
                }

                if (triggerObject is TriggerCamera)
                {
                    TriggerCamera triggerCamera = (TriggerCamera) triggerObject;
                    pWriter.WriteInt(triggerCamera.Id);
                    pWriter.WriteBool(triggerCamera.IsEnabled);
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
            pWriter.WriteInt((int) arg5);
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
