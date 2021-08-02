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
            LoadTriggers = 0x2,
            UpdateTrigger = 0x3,
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

        public static Packet LoadTriggers(List<TriggerObject> triggerObjects)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.TRIGGER);
            pWriter.WriteEnum(TriggerPacketMode.LoadTriggers);
            pWriter.WriteInt(triggerObjects.Count);

            foreach (TriggerObject triggerObject in triggerObjects)
            {
                WriteTrigger(pWriter, triggerObject);
            }
            return pWriter;
        }

        public static Packet UpdateTrigger(TriggerObject trigger)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.TRIGGER);
            pWriter.WriteEnum(TriggerPacketMode.UpdateTrigger);
            WriteTrigger(pWriter, trigger);
            return pWriter;
        }

        public static void WriteTrigger(PacketWriter pWriter, TriggerObject trigger)
        {
            switch (trigger)
            {
                case TriggerMesh triggerMesh:
                    pWriter.WriteInt(triggerMesh.Id);
                    pWriter.WriteBool(triggerMesh.IsVisible);
                    pWriter.WriteByte(0x00);
                    pWriter.WriteInt(triggerMesh.Value);
                    pWriter.WriteUnicodeString("");
                    pWriter.WriteFloat(1); //constant
                    break;

                case TriggerEffect triggerEffect:
                    pWriter.WriteInt(triggerEffect.Id);
                    pWriter.WriteBool(triggerEffect.IsVisible);
                    pWriter.WriteByte(0x00);
                    pWriter.WriteInt(3); //not sure where this value is coming from.
                    break;

                case TriggerCamera triggerCamera:
                    pWriter.WriteInt(triggerCamera.Id);
                    pWriter.WriteBool(triggerCamera.IsEnabled);
                    break;

                case TriggerActor triggerActor: // needs verification if this is the packet structure
                    pWriter.WriteInt(triggerActor.Id);
                    pWriter.WriteBool(triggerActor.IsVisible);
                    pWriter.WriteUnicodeString(triggerActor.StateName);
                    break;

                    //case TriggerLadder triggerLadder:
                    //    pWriter.WriteInt(triggerLadder.Id);
                    //    pWriter.WriteBoolarg2);
                    //    pWriter.WriteBool(arg3);
                    //    pWriter.WriteInt(3); //not sure where this value is coming from.
                    //    break;
            }
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
    }
}
