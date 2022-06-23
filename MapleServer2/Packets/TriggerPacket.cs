using Maple2.Trigger.Enum;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

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
        SidePopup = 0x12
    }

    public enum TriggerUIMode : byte
    {
        Guide = 0x1,
        EnableBanner = 0x2,
        DisableBanner = 0x3,
        StartCutscene = 0x4,
        StopCutscene = 0x5,
        SetAnimationSequence = 0x7,
        SetAnimationLoop = 0x8,
        FaceEmotion = 0x9
    }

    public static PacketWriter LoadTriggers(List<TriggerObject> triggerObjects)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Trigger);
        pWriter.Write(TriggerPacketMode.LoadTriggers);
        pWriter.WriteInt(triggerObjects.Count);

        foreach (TriggerObject triggerObject in triggerObjects)
        {
            WriteTrigger(pWriter, triggerObject);
        }
        return pWriter;
    }

    public static PacketWriter UpdateTrigger(TriggerObject trigger)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Trigger);
        pWriter.Write(TriggerPacketMode.UpdateTrigger);
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
                pWriter.WriteByte();
                pWriter.WriteInt(triggerMesh.Animation);
                pWriter.WriteUnicodeString();
                pWriter.WriteFloat(1); //constant
                break;
            case TriggerEffect triggerEffect:
                pWriter.WriteInt(triggerEffect.Id);
                pWriter.WriteBool(triggerEffect.IsVisible);
                pWriter.WriteByte();
                pWriter.WriteInt(3); //not sure where this value is coming from.
                break;

            case TriggerCamera triggerCamera:
                pWriter.WriteInt(triggerCamera.Id);
                pWriter.WriteBool(triggerCamera.IsEnabled);
                break;

            case TriggerActor triggerActor:
                pWriter.WriteInt(triggerActor.Id);
                pWriter.WriteBool(triggerActor.IsVisible);
                pWriter.WriteUnicodeString(triggerActor.StateName);
                break;
            case TriggerLadder triggerLadder:
                pWriter.WriteInt(triggerLadder.Id);
                pWriter.WriteBool(triggerLadder.IsVisible);
                pWriter.WriteBool(triggerLadder.AnimationEffect);
                pWriter.WriteInt(triggerLadder.AnimationDelay);
                break;
            case TriggerRope triggerRope:
                pWriter.WriteInt(triggerRope.Id);
                pWriter.WriteBool(triggerRope.IsVisible);
                pWriter.WriteBool(triggerRope.AnimationEffect);
                pWriter.WriteInt(triggerRope.AnimationDelay);
                break;
            case TriggerSound triggerSound:
                pWriter.WriteInt(triggerSound.Id);
                pWriter.WriteBool(triggerSound.IsEnabled);
                break;
        }
    }

    public static PacketWriter Guide(int eventId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Trigger);
        pWriter.Write(TriggerPacketMode.UI);
        pWriter.Write(TriggerUIMode.Guide);
        pWriter.WriteInt(eventId);
        return pWriter;
    }

    public static PacketWriter Banner(byte state, int entityId, int stringGuideId = 0, int time = 0)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Trigger);
        pWriter.Write(TriggerPacketMode.UI);
        pWriter.WriteByte(state); // 02 = on, 03 = off
        pWriter.WriteInt(entityId);
        pWriter.WriteInt(stringGuideId);
        pWriter.WriteInt(time); //display duration in ms
        return pWriter;
    }

    public static PacketWriter StartCutscene(string fileName, int movieId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Trigger);
        pWriter.Write(TriggerPacketMode.UI);
        pWriter.Write(TriggerUIMode.StartCutscene);
        pWriter.WriteString(fileName);
        pWriter.WriteInt(movieId);
        return pWriter;
    }

    public static PacketWriter StopCutscene(int movieId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Trigger);
        pWriter.Write(TriggerPacketMode.UI);
        pWriter.Write(TriggerUIMode.StopCutscene);
        pWriter.WriteInt(movieId);
        return pWriter;
    }

    public static PacketWriter SetAnimationSequence(string animationState)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Trigger);
        pWriter.Write(TriggerPacketMode.UI);
        pWriter.Write(TriggerUIMode.SetAnimationSequence);
        pWriter.WriteInt(1);
        pWriter.WriteUnicodeString(animationState);
        return pWriter;
    }

    public static PacketWriter SetAnimationLoop(string animationState, int duration, bool loop)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Trigger);
        pWriter.Write(TriggerPacketMode.UI);
        pWriter.Write(TriggerUIMode.SetAnimationLoop);
        pWriter.WriteBool(loop);
        pWriter.WriteInt(duration);
        pWriter.WriteUnicodeString(animationState);
        return pWriter;
    }

    public static PacketWriter SetFaceEmotion(int objectId, string animationString)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Trigger);
        pWriter.Write(TriggerPacketMode.UI);
        pWriter.Write(TriggerUIMode.FaceEmotion);
        pWriter.WriteInt(objectId);
        pWriter.WriteString(animationString);
        return pWriter;
    }

    public static PacketWriter Camera(int[] pathIds, bool returnView)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Trigger);
        pWriter.Write(TriggerPacketMode.Camera);
        pWriter.WriteByte((byte) pathIds.Length);
        foreach (int pathId in pathIds)
        {
            pWriter.WriteInt(pathId);
        }
        pWriter.WriteBool(returnView);
        return pWriter;
    }

    public static PacketWriter Timer(int msTime, bool clearAtZero = false, bool display = false)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Trigger);
        pWriter.Write(TriggerPacketMode.Timer);
        pWriter.WriteBool(display);
        pWriter.WriteInt(Environment.TickCount);
        pWriter.WriteInt(msTime);
        pWriter.WriteBool(clearAtZero);
        pWriter.WriteInt();
        pWriter.WriteUnicodeString();
        return pWriter;
    }

    public static PacketWriter SidePopUp(SideNpcTalkType type, int duration, string illustration, string sound, string script)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Trigger);
        pWriter.Write(TriggerPacketMode.SidePopup);
        pWriter.Write((byte) type);
        pWriter.WriteInt(duration);
        pWriter.WriteString();
        pWriter.WriteString(illustration);
        pWriter.WriteString(sound);
        pWriter.WriteString();
        pWriter.WriteUnicodeString(script);
        return pWriter;
    }
}
