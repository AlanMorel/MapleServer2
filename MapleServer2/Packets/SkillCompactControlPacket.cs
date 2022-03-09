using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class SkillCompactControlPacket
{
    private enum SkillCompactControlPacketMode : byte
    {
        OpenSettings = 0x00,
        LoadControls = 0x02,
    }
    
    public static PacketWriter OpenSettings(List<SkillCompactControl> compactControls)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.SKILL_COMPACT_CONTROL);
        pWriter.Write(SkillCompactControlPacketMode.OpenSettings);
        WriteCompactControlsList(pWriter, compactControls);
        return pWriter;
    }

    public static PacketWriter LoadControls(List<SkillCompactControl> compactControls)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.SKILL_COMPACT_CONTROL);
        pWriter.Write(SkillCompactControlPacketMode.LoadControls);
        WriteCompactControlsList(pWriter, compactControls);
        return pWriter;
    }

    public static void WriteCompactControlsList(PacketWriter pWriter, List<SkillCompactControl> compactControls)
    {
        pWriter.WriteInt(compactControls.Count);
        foreach (SkillCompactControl control in compactControls)
        {
            pWriter.WriteUnicodeString(control.Name);
            pWriter.WriteLong(control.ShortcutKeyCode);
            pWriter.WriteInt(control.SkillIds.Count);
            foreach (int skillId in control.SkillIds)
            {
                pWriter.WriteInt(skillId);
            }
        }
    }
}
