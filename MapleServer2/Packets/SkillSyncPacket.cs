using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class SkillSyncPacket
{
    public static PacketWriter Sync(long skillSN, IFieldObject<Player> player, CoordF position, CoordF rotation, bool toggle)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.SKILL_SYNC);
        SkillCast skillCast = SkillUsePacket.SkillCastMap[skillSN];

        pWriter.WriteLong(skillSN);
        pWriter.WriteInt(player.ObjectId);
        pWriter.WriteInt(skillCast.SkillId);
        pWriter.WriteShort(skillCast.SkillLevel);
        pWriter.WriteByte();
        pWriter.Write(position);
        pWriter.Write(CoordF.From(0, 0, 0)); // unk
        pWriter.Write(rotation);
        pWriter.Write(CoordF.From(0, 0, 0)); // filler for many 00
        pWriter.WriteBool(toggle);
        pWriter.WriteInt();
        pWriter.WriteByte();

        return pWriter;
    }
}
