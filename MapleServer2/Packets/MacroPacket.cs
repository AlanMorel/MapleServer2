using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class MacroPacket
{
    private enum Mode : byte
    {
        OpenSettings = 0x00,
        LoadControls = 0x02,
    }

    public static PacketWriter OpenSettings(List<Macro> compactControls)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Macro);
        pWriter.Write(Mode.OpenSettings);
        WriteMacroList(pWriter, compactControls);
        return pWriter;
    }

    public static PacketWriter LoadControls(List<Macro> compactControls)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Macro);
        pWriter.Write(Mode.LoadControls);
        WriteMacroList(pWriter, compactControls);
        return pWriter;
    }

    private static void WriteMacroList(PacketWriter pWriter, List<Macro> compactControls)
    {
        pWriter.WriteInt(compactControls.Count);
        foreach (Macro control in compactControls)
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
