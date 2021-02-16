using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Enums;
using MapleServer2.Types;

namespace MapleServer2.Packets.Helpers
{
    public static class SyncStateHelper
    {
        public static SyncState ReadSyncState(this PacketReader packet)
        {
            SyncState state = new SyncState();

            state.Animation1 = packet.ReadByte();
            state.Animation2 = packet.ReadByte();
            state.Flag = (SyncStateFlag) packet.ReadByte();
            if (state.Flag.HasFlag(SyncStateFlag.Flag1))
            {
                state.Flag1Unknown1 = packet.ReadInt();
                state.Flag1Unknown2 = packet.ReadShort();
            }

            state.Coord = packet.Read<CoordS>();
            state.Rotation = packet.ReadShort(); // CoordS / 10 (Rotation?)
            state.Animation3 = packet.ReadByte();
            if (state.Animation3 > 127)
            { // if animation < 0 (signed)
                state.UnknownFloat1 = packet.Read<float>();
                state.UnknownFloat2 = packet.Read<float>();
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
            if (state.Flag.HasFlag(SyncStateFlag.Flag4))
            {
                state.Flag4Unknown = packet.ReadUnicodeString(); // Animation string?
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

            return state;
        }

        public static PacketWriter WriteSyncState(this PacketWriter pWriter, SyncState entry)
        {
            pWriter.WriteByte(entry.Animation1);
            pWriter.WriteByte(entry.Animation2);
            pWriter.WriteByte((byte) entry.Flag);
            if (entry.Flag.HasFlag(SyncStateFlag.Flag1))
            {
                pWriter.WriteInt(entry.Flag1Unknown1);
                pWriter.WriteShort(entry.Flag1Unknown2);
            }

            pWriter.Write(entry.Coord);
            pWriter.WriteShort(entry.Rotation);
            pWriter.WriteByte(entry.Animation3);

            if (entry.Animation3 > 127)
            {
                pWriter.Write(entry.UnknownFloat1);
                pWriter.Write(entry.UnknownFloat2);
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
            if (entry.Flag.HasFlag(SyncStateFlag.Flag4))
            {
                pWriter.WriteUnicodeString(entry.Flag4Unknown ?? "");
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
            return pWriter;
        }

        public static Packet WriteSyncStates(this PacketWriter pWriter, params SyncState[] syncStates)
        {
            pWriter.WriteByte((byte) syncStates.Length);
            foreach (SyncState entry in syncStates)
            {
                pWriter.WriteSyncState(entry);
            }

            return pWriter;
        }
    }
}
