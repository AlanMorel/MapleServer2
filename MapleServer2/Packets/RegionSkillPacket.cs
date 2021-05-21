using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class RegionSkillPacket
    {
        private enum RegionSkillMode : byte
        {
            Add = 0x0,
            Remove = 0x1
        }

        public static Packet Send(int sourceObjectId, CoordS effectCoord, SkillCast skill)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.REGION_SKILL);
            byte tileCount = 1; // TODO: add amount of tiles affected to SkillCast?
            pWriter.WriteEnum(RegionSkillMode.Add);
            pWriter.WriteInt(sourceObjectId);
            pWriter.WriteInt(sourceObjectId);
            pWriter.WriteInt();
            if (tileCount != 0)
            {
                pWriter.WriteByte(tileCount);
                for (int i = 0; i < tileCount; i++)
                {
                    pWriter.Write(effectCoord.ToFloat());
                }
                pWriter.WriteInt(skill.SkillId);
                pWriter.WriteShort(skill.SkillLevel);
                pWriter.WriteLong();
            }

            return pWriter;
        }

        public static Packet RemoveRegionSkill()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.REGION_SKILL);
            pWriter.WriteEnum(RegionSkillMode.Remove);
            pWriter.WriteInt(); // Uid regionEffect
            return pWriter;
        }
    }
}
