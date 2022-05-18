using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class FieldNpcPacket
{
    public static PacketWriter AddNpc(IFieldActor<NpcMetadata> npc)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FieldAddNPC);
        pWriter.WriteInt(npc.ObjectId);
        pWriter.WriteInt(npc.Value.Id);
        pWriter.Write(npc.Coord);
        pWriter.Write(npc.Rotation);
        // If NPC is not valid, the packet seems to stop here

        pWriter.DefaultStatsMob(npc);

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

    public static PacketWriter AddMob(IFieldActor<NpcMetadata> mob)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FieldAddNPC);

        pWriter.WriteInt(mob.ObjectId);
        pWriter.WriteInt(mob.Value.Id);
        pWriter.Write(mob.Coord);
        pWriter.Write(mob.Rotation);
        if (mob.Value.IsBoss())
        {
            pWriter.WriteString(mob.Value.Model); // StrA - kfm model string
        }
        // If NPC is not valid, the packet seems to stop here

        pWriter.DefaultStatsMob(mob);

        pWriter.WriteByte();
        short count = 0;
        pWriter.WriteShort(count);
        for (int i = 0; i < count; i++)
        {
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteInt(); // usually 90000814 (skill id)??
            pWriter.WriteShort();
            pWriter.WriteInt();
            pWriter.WriteByte();
            pWriter.WriteLong();
        }

        pWriter.WriteLong();
        pWriter.WriteByte();
        pWriter.WriteInt(mob.Value.Level);
        if (mob.Value.IsBoss())
        {
            pWriter.WriteInt();
            pWriter.WriteUnicodeString();

            int skillCount = 0;
            pWriter.WriteInt(skillCount);
            for (int i = 0; i < skillCount; i++)
            {
                pWriter.WriteInt(); // skill id
                pWriter.WriteShort(); // skill level
            }
        }

        pWriter.WriteInt();
        pWriter.WriteByte();

        return pWriter;
    }

    public static PacketWriter RemoveNpc(IFieldActor<NpcMetadata> npc)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FieldRemoveNPC);
        pWriter.WriteInt(npc.ObjectId);
        return pWriter;
    }
}
