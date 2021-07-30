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
            Cutscene = 0x4,
            Camera = 0x5,
            UI = 0x8,
            Timer = 0xE,
        }

        public enum TriggerUIMode : byte
        {
            Guide = 0x1,
            EnableBanner = 0x2,
            DisableBanner = 0x3,
            StartCutscene = 0x4,
            StopCutscene = 0x5,
        }

        public static Packet SendTriggerObjects(List<TriggerObject> triggerObjects)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.TRIGGER);
            pWriter.WriteEnum(TriggerPacketMode.SendTriggers);
            pWriter.WriteInt(triggerObjects.Count);

            foreach (TriggerObject triggerObject in triggerObjects)
            {
                switch (triggerObject)
                {
                    case TriggerMesh:
                        TriggerMesh triggerMesh = (TriggerMesh) triggerObject;
                        //pWriter.Write(SetMeshTrigger(triggerMesh.Id, triggerMesh.IsVisible, 0));
                        pWriter.WriteInt(triggerMesh.Id);
                        pWriter.WriteBool(triggerMesh.IsVisible);
                        pWriter.WriteByte(0x00);
                        pWriter.WriteInt(2); //get this from the correct place, it probably is not always 2
                        pWriter.WriteInt(0);
                        pWriter.WriteShort(16256); //constant: 80 3F ends a Mesh trigger.
                        break;

                    case TriggerEffect:
                        TriggerEffect triggerEffect = (TriggerEffect) triggerObject;
                        pWriter.WriteInt(triggerEffect.Id);
                        pWriter.WriteBool(triggerEffect.IsVisible);
                        pWriter.WriteByte(0x00);
                        pWriter.WriteInt(3); //not sure where this value is coming from.
                        break;

                    case TriggerCamera:
                        TriggerCamera triggerCamera = (TriggerCamera) triggerObject;
                        pWriter.WriteInt(triggerCamera.Id);
                        pWriter.WriteBool(triggerCamera.IsEnabled);
                        break;
                }
            }

            return pWriter;
        }

        public static Packet SetLadderTrigger(int ladderId, bool arg2, bool arg3)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.TRIGGER);
            pWriter.WriteEnum(TriggerPacketMode.SingleTrigger);
            pWriter.WriteInt(ladderId);
            pWriter.WriteBool(arg2);
            pWriter.WriteBool(arg3);
            pWriter.WriteInt(3); //unsure where this 3 is coming from, triggermesh also has it
            return pWriter;
        }

        public static Packet SetEffectTrigger(int effectId, bool isVisible)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.TRIGGER);
            pWriter.WriteEnum(TriggerPacketMode.SingleTrigger);
            pWriter.WriteInt(effectId);
            pWriter.WriteBool(isVisible);
            pWriter.WriteByte();
            pWriter.WriteInt(3); //unsure where this 3 is coming from, triggermesh also has it
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

        public static Packet Guide(int eventId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.TRIGGER);
            pWriter.WriteEnum(TriggerPacketMode.UI);
            pWriter.WriteEnum(TriggerUIMode.Guide);
            pWriter.WriteInt(eventId);
            return pWriter;
        }

        public static Packet Banner(byte state, int entityId, int stringGuideId = 0, int time = 0)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.TRIGGER);
            pWriter.WriteEnum(TriggerPacketMode.UI);
            pWriter.WriteByte(state); // 02 = on, 03 = off
            pWriter.WriteInt(entityId);
            pWriter.WriteInt(stringGuideId);
            pWriter.WriteInt(time); //display duration in ms
            return pWriter;
        }

        public static Packet StartCutscene(string fileName, int movieId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.TRIGGER);
            pWriter.WriteEnum(TriggerPacketMode.UI);
            pWriter.WriteEnum(TriggerUIMode.StartCutscene);
            pWriter.WriteMapleString(fileName);
            pWriter.WriteInt(movieId);
            return pWriter;
        }

        public static Packet StopCutscene(int movieId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.TRIGGER);
            pWriter.WriteEnum(TriggerPacketMode.UI);
            pWriter.WriteEnum(TriggerUIMode.StopCutscene);
            pWriter.WriteInt(movieId);
            return pWriter;
        }

        public static Packet Camera(int[] pathIds, bool returnView)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.TRIGGER);
            pWriter.WriteEnum(TriggerPacketMode.Camera);
            pWriter.WriteByte((byte) pathIds.Length);
            foreach (int pathId in pathIds)
            {
                pWriter.WriteInt(pathId);
            }
            pWriter.WriteBool(returnView);
            return pWriter;
        }

        public static Packet Timer(int msTime, bool clearAtZero = false, bool display = false)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.TRIGGER);
            pWriter.WriteEnum(TriggerPacketMode.Timer);
            pWriter.WriteBool(display);
            pWriter.WriteInt(Environment.TickCount);
            pWriter.WriteInt(msTime);
            pWriter.WriteBool(clearAtZero);
            pWriter.WriteInt();
            pWriter.WriteUnicodeString("");
            return pWriter;
        }

        public static Packet Timer2()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.TRIGGER);
            pWriter.WriteEnum(TriggerPacketMode.Timer);
            pWriter.WriteBool(false);
            pWriter.WriteInt(Environment.TickCount);
            pWriter.WriteInt(18000);
            pWriter.WriteBool(false);
            pWriter.WriteInt();
            pWriter.WriteUnicodeString("");
            return pWriter;
        }
    }
}
