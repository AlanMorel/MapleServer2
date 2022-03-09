using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Database;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class SkillCompactControlHandler : GamePacketHandler
{
    public override RecvOp OpCode => RecvOp.SKILL_COMPACT_CONTROL;

    private enum SkillCompactControlMode : byte
    {
        OpenSettings = 0x0,
        CloseSettings = 0x1,
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        SkillCompactControlMode mode = (SkillCompactControlMode) packet.ReadByte();

        switch (mode)
        {
            case SkillCompactControlMode.OpenSettings:
                HandleOpenSettings(session);
                break;
            case SkillCompactControlMode.CloseSettings:
                HandleCloseSettings(session, packet);
                break;
            default:
                IPacketHandler<GameSession>.LogUnknownMode(mode);
                break;
        }
    }
    
    private static void HandleOpenSettings(GameSession session)
    {
        session.Send(SkillCompactControlPacket.OpenSettings(session.Player.SkillCompactControls));
    }
    
    private static void HandleCloseSettings(GameSession session, PacketReader packet)
    {
        List<SkillCompactControl> skillCompactControls = session.Player.SkillCompactControls;
        int compactControlCount = packet.ReadInt();
        for (int i = 0; i < compactControlCount; i++)
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

            SkillCompactControl skillCompactControl = skillCompactControls.ElementAtOrDefault(i);
            if (skillCompactControl is null)
            {
                skillCompactControl = new(session.Player.CharacterId, name, shortcutKeyCode, skillIds);
                skillCompactControls.Add(skillCompactControl);
                continue;
            }
            
            skillCompactControl.Name = name;
            skillCompactControl.ShortcutKeyCode = shortcutKeyCode;
            skillCompactControl.SkillIds = skillIds;
            DatabaseManager.SkillCompactControls.Update(skillCompactControl);
        }
    }
}
