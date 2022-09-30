using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Enums;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class FieldObjectPacket
{
    private enum Mode : byte
    {
        LoadPlayer = 0x03,
        RemovePlayer = 0x04,
        UpdateEntity = 0x05,
        LoadNpc = 0x06,
        RemoveNpc = 0x07,
        MoveNpc = 0x08,
        Unk2 = 0x0B
    }

    public static PacketWriter LoadPlayer(IFieldObject<Player> fieldPlayer)
    {
        Player player = fieldPlayer.Value;
        PacketWriter pWriter = PacketWriter.Of(SendOp.FieldObject);
        pWriter.Write(Mode.LoadPlayer);
        pWriter.WriteInt(fieldPlayer.ObjectId);
        pWriter.WriteLong(player.CharacterId);
        pWriter.WriteLong(player.AccountId);
        pWriter.WriteUnicodeString(player.Name);
        pWriter.WriteUnicodeString(player.ProfileUrl);
        pWriter.WriteUnicodeString(player.Motto);
        pWriter.WriteByte();
        pWriter.Write(fieldPlayer.Coord);
        pWriter.WriteShort(player.Levels.Level);
        pWriter.WriteShort((short) player.JobCode);
        pWriter.WriteShort((short) player.SubJobCode);
        pWriter.WriteShort();
        pWriter.WriteInt(player.MapId);
        pWriter.WriteLong(1); // unk
        pWriter.WriteUnicodeString(player.Account.Home?.Name ?? "");
        pWriter.WriteInt();
        pWriter.WriteShort();
        foreach (int trophyCount in player.TrophyCount)
        {
            pWriter.WriteInt(trophyCount);
        }

        return pWriter;
    }

    public static PacketWriter RemovePlayer(IFieldObject<Player> fieldPlayer)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FieldObject);
        pWriter.Write(Mode.RemovePlayer);
        pWriter.WriteInt(fieldPlayer.ObjectId);

        return pWriter;
    }

    public static PacketWriter UpdatePlayer(IFieldActor<Player> player)
    {
        FieldObjectUpdate flag = FieldObjectUpdate.Move | FieldObjectUpdate.Animate;
        PacketWriter pWriter = PacketWriter.Of(SendOp.FieldObject);
        pWriter.Write(Mode.UpdateEntity);
        pWriter.WriteInt(player.ObjectId);
        pWriter.WriteByte((byte) flag);

        if (flag.HasFlag(FieldObjectUpdate.Type1))
        {
            pWriter.WriteByte();
        }

        if (flag.HasFlag(FieldObjectUpdate.Move))
        {
            pWriter.Write(player.Coord);
        }

        if (flag.HasFlag(FieldObjectUpdate.Level))
        {
            pWriter.WriteShort(player.Value.Levels.Level);
        }

        if (flag.HasFlag(FieldObjectUpdate.Type4))
        {
            pWriter.WriteShort();
            pWriter.WriteInt();
        }

        if (flag.HasFlag(FieldObjectUpdate.Type5))
        {
            pWriter.WriteUnicodeString("Unknown");
        }

        if (flag.HasFlag(FieldObjectUpdate.Type6))
        {
            pWriter.WriteInt();
        }

        if (flag.HasFlag(FieldObjectUpdate.Animate))
        {
            pWriter.WriteShort(player.Animation);
        }

        return pWriter;
    }

    // Temporary packet, we should be using the packet above
    public static PacketWriter UpdateCharacterLevel(Player player)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FieldObject);

        pWriter.Write(Mode.UpdateEntity);
        pWriter.WriteInt(player.FieldPlayer.ObjectId);
        pWriter.Write(FieldObjectUpdate.Level);
        pWriter.WriteShort(player.Levels.Level);

        return pWriter;
    }

    public static PacketWriter LoadNpc(IFieldObject<NpcMetadata> npc)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FieldObject);
        pWriter.Write(Mode.LoadNpc);
        pWriter.WriteInt(npc.ObjectId);
        pWriter.WriteInt(npc.Value.Id);
        pWriter.WriteByte();
        pWriter.WriteInt(200);
        pWriter.Write(npc.Coord);
        return pWriter;
    }

    public static PacketWriter ControlNpc(IFieldActor<NpcMetadata> mob)
    {
        PacketWriter npcBuffer = new();
        npcBuffer.WriteInt(mob.ObjectId);
        npcBuffer.WriteByte();
        npcBuffer.Write(mob.Coord.ToShort());
        npcBuffer.WriteShort(mob.LookDirection);
        npcBuffer.Write(mob.Velocity.ToShort()); // Target Position's Displacement
        npcBuffer.WriteShort(100); // Unknown
        if (mob.Value.IsBoss())
        {
            npcBuffer.WriteInt(); // Player target id, only used for bosses. Agro?
        }

        npcBuffer.WriteByte(1); // Flag ?
        npcBuffer.WriteShort(mob.Animation);
        npcBuffer.WriteShort(1); // counter (increments every packet)
        // There can be more to this packet, probably dependent on Flag.

        PacketWriter pWriter = PacketWriter.Of(SendOp.NpcControl);
        pWriter.WriteShort(1); // Segments
        pWriter.WriteShort((short) npcBuffer.Length);
        pWriter.WriteBytes(npcBuffer.ToArray());
        return pWriter;
    }

    public static PacketWriter UpdateEntity(int objectId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FieldObject);
        pWriter.Write(Mode.UpdateEntity);
        pWriter.WriteInt(objectId);
        pWriter.WriteByte();
        pWriter.WriteShort();
        return pWriter;
    }

    public static PacketWriter RemoveNpc(IFieldObject<NpcMetadata> npc)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FieldObject);
        pWriter.Write(Mode.RemoveNpc);
        pWriter.WriteInt(npc.ObjectId);
        return pWriter;
    }

    public static PacketWriter MoveNpc(IFieldObject<NpcMetadata> npc)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FieldObject);
        pWriter.Write(Mode.MoveNpc);
        pWriter.WriteInt(npc.ObjectId);
        pWriter.WriteByte();
        pWriter.Write(npc.Coord);
        return pWriter;
    }
}
