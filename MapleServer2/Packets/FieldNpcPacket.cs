using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class FieldNpcPacket
{
    public static PacketWriter AddNpc(IFieldObject<NpcMetadata> npc)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FIELD_ADD_NPC);
        pWriter.WriteInt(npc.ObjectId);
        pWriter.WriteInt(npc.Value.Id);
        pWriter.Write(npc.Coord);
        pWriter.Write(npc.Rotation);
        // If NPC is not valid, the packet seems to stop here

        pWriter.DefaultStatsNpc();

        pWriter.WriteByte();
        short count = 0;
        pWriter.WriteShort(count); // branch
        for (int i = 0; i < count; i++)
        {
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteShort();
            pWriter.WriteInt();
            pWriter.WriteByte();
            pWriter.WriteLong();
        }

        pWriter.WriteLong(); // uid
        pWriter.WriteByte();
        pWriter.WriteInt(1); // NPC level
        pWriter.WriteInt();
        pWriter.WriteByte();

        return pWriter;
    }

    public static PacketWriter AddBoss(IFieldActor<NpcMetadata> mob)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FIELD_ADD_NPC);

        pWriter.WriteInt(mob.ObjectId);
        pWriter.WriteInt(mob.Value.Id);
        pWriter.Write(mob.Coord);
        pWriter.Write(mob.Rotation);
        pWriter.WriteString(mob.Value.Model); // StrA - kfm model string
        // If NPC is not valid, the packet seems to stop here

        StatPacket.DefaultStatsMob(pWriter, mob);

        pWriter.WriteByte();
        pWriter.WriteLong();
        pWriter.WriteLong();
        pWriter.WriteInt();
        pWriter.WriteByte();
        int count = 0;
        pWriter.WriteInt(count); // branch
        for (int i = 0; i < count; i++)
        {
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteShort();
            pWriter.WriteInt();
            pWriter.WriteByte();
            pWriter.WriteLong();
        }

        pWriter.WriteLong();
        pWriter.WriteByte();
        pWriter.WriteInt(1);
        pWriter.WriteInt();
        pWriter.WriteByte();

        return pWriter;
    }

    public static PacketWriter AddMob(IFieldActor<NpcMetadata> mob)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FIELD_ADD_NPC);

        pWriter.WriteInt(mob.ObjectId);
        pWriter.WriteInt(mob.Value.Id);
        pWriter.Write(mob.Coord);
        pWriter.Write(mob.Rotation);
        // If NPC is not valid, the packet seems to stop here

        pWriter.DefaultStatsMob(mob);

        pWriter.WriteLong();
        pWriter.WriteInt();
        pWriter.WriteInt(0x0E); // NPC level
        pWriter.WriteInt();
        pWriter.WriteByte();

        return pWriter;
    }

    public static PacketWriter RemoveNpc(IFieldActor<NpcMetadata> npc)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FIELD_REMOVE_NPC);
        pWriter.WriteInt(npc.ObjectId);
        return pWriter;
    }
}
