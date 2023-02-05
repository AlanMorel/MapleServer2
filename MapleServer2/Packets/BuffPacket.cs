using System.Diagnostics;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class BuffPacket
{
    private enum Mode : byte
    {
        Add = 0,
        Remove = 1,
        Update = 2
    }

    public static PacketWriter AddBuff(Status status)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Buff);
        pWriter.Write(Mode.Add);
        pWriter.WriteBuffOwner(status.Target, status.UniqueId, status.Source);

        pWriter.WriteBuff(status.Start, status.End, status.SkillId, status.Level, status.Stacks);
        pWriter.WriteByte(1); // sniffs always get 1 but doesn't change behaviour
        pWriter.WriteLong();

        return pWriter;
    }

    public static PacketWriter UpdateBuff(Status status)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Buff);
        pWriter.Write(Mode.Update);
        pWriter.WriteBuffOwner(status.Target, status.UniqueId, status.Source);

        pWriter.WriteInt(status.Target);

        pWriter.WriteBuff(status.Start, status.End, status.SkillId, status.Level, status.Stacks);

        pWriter.WriteByte();
        return pWriter;
    }

    public static PacketWriter RemoveBuff(Status status)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Buff);
        pWriter.Write(Mode.Remove);
        pWriter.WriteBuffOwner(status.Target, status.UniqueId, status.Source);

        return pWriter;
    }

    public static PacketWriter AddBuff(AdditionalEffect status, int target)
    {
        Debug.Assert(status.Caster != null, "status.Caster != null");

        PacketWriter pWriter = PacketWriter.Of(SendOp.Buff);
        pWriter.Write(Mode.Add);
        pWriter.WriteBuffOwner(target, status.BuffId, status.Caster?.ObjectId ?? 0);

        pWriter.WriteBuff(status.Start, status.End, status.Id, status.Level, status.Stacks);
        pWriter.WriteBool(status.IsActive); // sniffs always get 1 but doesn't change behaviour
        pWriter.WriteLong(status.ShieldHealth);

        return pWriter;
    }

    public static PacketWriter UpdateBuff(AdditionalEffect status, int target)
    {
        Debug.Assert(status.Caster != null, "status.Caster != null");

        PacketWriter pWriter = PacketWriter.Of(SendOp.Buff);
        pWriter.Write(Mode.Update);
        pWriter.WriteBuffOwner(target, status.BuffId, status.Caster?.ObjectId ?? 0);

        pWriter.WriteInt(1);

        pWriter.WriteBuff(status.Start, status.End, status.Id, status.Level, status.Stacks);

        pWriter.WriteBool(status.IsActive);
        return pWriter;
    }

    public static PacketWriter UpdateShieldBuff(AdditionalEffect status, int target)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Buff);
        pWriter.Write(Mode.Update);
        pWriter.WriteBuffOwner(target, status.BuffId, status.Caster?.ObjectId ?? 0);

        pWriter.WriteInt(2);
        pWriter.WriteLong(status.ShieldHealth);

        return pWriter;
    }

    public static PacketWriter RemoveBuff(AdditionalEffect status, int target)
    {
        Debug.Assert(status.Caster != null, "status.Caster != null");

        PacketWriter pWriter = PacketWriter.Of(SendOp.Buff);
        pWriter.Write(Mode.Remove);
        pWriter.WriteBuffOwner(target, status.BuffId, status.Caster?.ObjectId ?? 0);

        return pWriter;
    }

    private static void WriteBuffOwner(this PacketWriter pWriter, int target, int uniqueId, int source)
    {
        pWriter.WriteInt(target);
        pWriter.WriteInt(uniqueId);
        pWriter.WriteInt(source);
    }

    private static void WriteBuff(this PacketWriter pWriter, int start, int end, int skillId, int level, int stacks)
    {
        pWriter.WriteInt(start);
        pWriter.WriteInt(end);
        pWriter.WriteInt(skillId);
        pWriter.WriteShort((short) level);
        pWriter.WriteInt(stacks);
    }

    public static void WriteFieldEnterBuffs(this PacketWriter pWriter, AdditionalEffects effects)
    {
        Debug.Assert(effects.Parent != null, "effects.Parent != null");

        pWriter.WriteShort((short) effects.Effects.Count);

        foreach (AdditionalEffect effect in effects.Effects)
        {
            WriteFieldEnterBuff(pWriter, effect, effects.Parent?.ObjectId ?? 0);
        }
    }

    private static void WriteFieldEnterBuff(this PacketWriter pWriter, AdditionalEffect status, int target)
    {
        pWriter.WriteBuffOwner(target, status.BuffId, status.Caster?.ObjectId ?? 0);
        pWriter.WriteBuff(status.Start, status.End, status.Id, status.Level, status.Stacks);

        pWriter.WriteBool(status.IsActive);
        pWriter.WriteLong(status.ShieldHealth);
    }
}
