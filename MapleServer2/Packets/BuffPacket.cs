using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class BuffPacket
{
    private enum StatusMode : byte
    {
        Add = 0,
        Remove = 1,
        Update = 2
    }

    public static PacketWriter SendBuff(byte mode, Status status)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Buff);
        pWriter.WriteByte(mode);
        pWriter.WriteInt(status.Target);
        pWriter.WriteInt(status.UniqueId);
        pWriter.WriteInt(status.Source);
        switch (mode)
        {
            case (byte) StatusMode.Add:
                pWriter.WriteInt(status.Start);
                pWriter.WriteInt(status.End);
                pWriter.WriteInt(status.SkillId);
                pWriter.WriteShort(status.Level);
                pWriter.WriteInt(status.Stacks);
                pWriter.WriteByte(1); // sniffs always get 1 but doesn't change behaviour
                pWriter.WriteLong();
                break;
            case (byte) StatusMode.Update:
                pWriter.WriteInt(status.Target);
                pWriter.WriteInt(status.Start);
                pWriter.WriteInt(status.End);
                pWriter.WriteInt(status.SkillId);
                pWriter.WriteShort(status.Level);
                pWriter.WriteInt(status.Stacks);
                pWriter.WriteByte();
                break;
            case (byte) StatusMode.Remove:
                break;
        }

        return pWriter;
    }

    public static PacketWriter BuffPacketHeader(byte mode, AdditionalEffect effect, int target)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Buff);
        pWriter.WriteByte(mode);
        pWriter.WriteInt(target);
        pWriter.WriteInt(effect.BuffId);
        pWriter.WriteInt(effect.SourceId);

        return pWriter;
    }

    public static PacketWriter AddBuff(AdditionalEffect effect, int target)
    {
        PacketWriter pWriter = BuffPacketHeader(0, effect, target);

        pWriter.WriteInt(effect.Start);
        pWriter.WriteInt(effect.End);
        pWriter.WriteInt(effect.Id);
        pWriter.WriteShort((short) effect.Level);
        pWriter.WriteInt(effect.Stacks);
        pWriter.WriteByte(1); // sniffs always get 1 but doesn't change behaviour
        pWriter.WriteLong();

        return pWriter;
    }

    public static PacketWriter RemoveBuff(AdditionalEffect effect, int target)
    {
        PacketWriter pWriter = BuffPacketHeader(1, effect, target);

        return pWriter;
    }

    public static PacketWriter UpdateBuff(AdditionalEffect effect, int target)
    {
        PacketWriter pWriter = BuffPacketHeader(2, effect, target);

        pWriter.WriteInt(target);
        pWriter.WriteInt(effect.Start);
        pWriter.WriteInt(effect.End);
        pWriter.WriteInt(effect.Id);
        pWriter.WriteShort((short) effect.Level);
        pWriter.WriteInt(effect.Stacks);
        pWriter.WriteByte();

        return pWriter;
    }
}
