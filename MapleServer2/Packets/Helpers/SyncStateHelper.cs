using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Enums;
using MapleServer2.Types;

namespace MapleServer2.Packets.Helpers;

public static class SyncStateHelper
{
    public static SyncState ReadSyncState(this PacketReader packet)
    {
        SyncState state = new();

        state.BoreAnimation = packet.ReadByte();
        state.SubAnimation = packet.ReadByte();
        state.Flag = (SyncStateFlag) packet.ReadByte();
        if (state.Flag.HasFlag(SyncStateFlag.Flag1))
        {
            state.EmoteId = packet.ReadInt();
            state.EmoteUnk = packet.ReadShort();
        }

        state.Coord = packet.Read<CoordS>();
        state.Rotation = packet.ReadShort(); // CoordS / 10 (Rotation?)
        state.JumpAnimation = packet.ReadByte();
        if (state.JumpAnimation > 127)
        { // if animation < 0 (signed)
            state.UnknownFloat1 = packet.ReadFloat();
            state.UnknownFloat2 = packet.ReadFloat();
        }
        state.Speed = packet.Read<CoordS>(); // XYZ Speed?
        state.Unknown1 = packet.ReadByte();
        state.Unknown2 = packet.ReadShort(); // CoordS / 10
        state.Unknown3 = packet.ReadShort(); // CoordS / 1000

        if (state.Flag.HasFlag(SyncStateFlag.Flag2))
        {
            state.Flag2Unknown1 = packet.Read<CoordF>();
            state.Flag2Unknown2 = packet.ReadUnicodeString();
        }
        if (state.Flag.HasFlag(SyncStateFlag.Flag3))
        {
            state.Flag3Unknown1 = packet.ReadInt();
            state.Flag3Unknown2 = packet.ReadUnicodeString();
        }
        if (state.Flag.HasFlag(SyncStateFlag.InteractableObject))
        {
            state.InteractableObjectCoord = packet.ReadUnicodeString(); // Animation string?
        }
        if (state.Flag.HasFlag(SyncStateFlag.Flag5))
        {
            state.Flag5Unknown1 = packet.ReadInt();
            state.Flag5Unknown2 = packet.ReadUnicodeString();
        }
        if (state.Flag.HasFlag(SyncStateFlag.Flag6))
        {
            state.Flag6Unknown1 = packet.ReadInt();
            state.Flag6Unknown2 = packet.ReadInt();
            state.Flag6Unknown3 = packet.ReadByte();
            state.Flag6Unknown4 = packet.Read<CoordF>();
            state.Flag6Unknown5 = packet.Read<CoordF>();
        }

        state.Unknown4 = packet.ReadInt();

        if (state.BoreAnimation == 60) // RPS
        {
            state.OpponentObjectId = packet.ReadInt();
            state.RPSUnk1 = packet.ReadByte();
            state.RPSUnk2 = packet.ReadByte();
            state.RPSUnk3 = packet.ReadUnicodeString();
        }

        return state;
    }

    public static PacketWriter WriteSyncState(this PacketWriter pWriter, SyncState entry)
    {
        pWriter.WriteByte(entry.BoreAnimation);
        pWriter.WriteByte(entry.SubAnimation);
        pWriter.WriteByte((byte) entry.Flag);
        if (entry.Flag.HasFlag(SyncStateFlag.Flag1))
        {
            pWriter.WriteInt(entry.EmoteId);
            pWriter.WriteShort(entry.EmoteUnk);
        }

        pWriter.Write(entry.Coord);
        pWriter.WriteShort(entry.Rotation);
        pWriter.WriteByte(entry.JumpAnimation);

        if (entry.JumpAnimation > 127)
        {
            pWriter.WriteFloat(entry.UnknownFloat1);
            pWriter.WriteFloat(entry.UnknownFloat2);
        }

        pWriter.Write(entry.Speed);
        pWriter.WriteByte(entry.Unknown1);
        pWriter.WriteShort(entry.Unknown2);
        pWriter.WriteShort(entry.Unknown3);
        if (entry.Flag.HasFlag(SyncStateFlag.Flag2))
        {
            pWriter.Write(entry.Flag2Unknown1);
            pWriter.WriteUnicodeString(entry.Flag2Unknown2 ?? "");
        }
        if (entry.Flag.HasFlag(SyncStateFlag.Flag3))
        {
            pWriter.WriteInt(entry.Flag3Unknown1);
            pWriter.WriteUnicodeString(entry.Flag3Unknown2 ?? "");
        }
        if (entry.Flag.HasFlag(SyncStateFlag.InteractableObject))
        {
            pWriter.WriteUnicodeString(entry.InteractableObjectCoord ?? "");
        }
        if (entry.Flag.HasFlag(SyncStateFlag.Flag5))
        {
            pWriter.WriteInt(entry.Flag5Unknown1);
            pWriter.WriteUnicodeString(entry.Flag5Unknown2 ?? "");
        }
        if (entry.Flag.HasFlag(SyncStateFlag.Flag6))
        {
            pWriter.WriteInt(entry.Flag6Unknown1);
            pWriter.WriteInt(entry.Flag6Unknown2);
            pWriter.WriteByte(entry.Flag6Unknown3);
            pWriter.Write(entry.Flag6Unknown4);
            pWriter.Write(entry.Flag6Unknown5);
        }
        pWriter.WriteInt(entry.Unknown4);

        if (entry.BoreAnimation == 60) // RPS
        {
            pWriter.WriteInt(entry.OpponentObjectId);
            pWriter.WriteByte(entry.RPSUnk1);
            pWriter.WriteByte(entry.RPSUnk2);
            pWriter.WriteUnicodeString(entry.RPSUnk3);
        }

        return pWriter;
    }

    public static PacketWriter WriteSyncStates(this PacketWriter pWriter, params SyncState[] syncStates)
    {
        pWriter.WriteByte((byte) syncStates.Length);
        foreach (SyncState entry in syncStates)
        {
            pWriter.WriteSyncState(entry);
        }

        return pWriter;
    }
}
