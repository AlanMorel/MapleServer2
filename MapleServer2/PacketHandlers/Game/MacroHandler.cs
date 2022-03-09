using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Database;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class MacroHandler : GamePacketHandler
{
    public override RecvOp OpCode => RecvOp.MACRO;

    private enum MacroMode : byte
    {
        OpenSettings = 0x0,
        CloseSettings = 0x1,
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        MacroMode mode = (MacroMode) packet.ReadByte();

        switch (mode)
        {
            case MacroMode.OpenSettings:
                HandleOpenSettings(session);
                break;
            case MacroMode.CloseSettings:
                HandleCloseSettings(session, packet);
                break;
            default:
                IPacketHandler<GameSession>.LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleOpenSettings(GameSession session)
    {
        session.Send(MacroPacket.OpenSettings(session.Player.Macros));
    }

    private static void HandleCloseSettings(GameSession session, PacketReader packet)
    {
        List<Macro> macros = session.Player.Macros;
        int macroCount = packet.ReadInt();
        for (int i = 0; i < macroCount; i++)
        {
            string name = packet.ReadUnicodeString();
            long shortcutKeyCode = packet.ReadLong();
            int skillIdCount = packet.ReadInt();
            List<int> skillIds = new();
            for (int j = 0; j < skillIdCount; j++)
            {
                int skillId = packet.ReadInt();
                skillIds.Add(skillId);
            }

            Macro macro = macros.ElementAtOrDefault(i);
            if (macro is null)
            {
                macro = new(session.Player.CharacterId, name, shortcutKeyCode, skillIds);
                macros.Add(macro);
                continue;
            }

            macro.Name = name;
            macro.ShortcutKeyCode = shortcutKeyCode;
            macro.SkillIds = skillIds;
            DatabaseManager.Macros.Update(macro);
        }
    }
}
