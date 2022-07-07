using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Database;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class JobHandler : GamePacketHandler<JobHandler>
{
    public override RecvOp OpCode => RecvOp.Job;

    private enum Mode : byte
    {
        Close = 0x08,
        Save = 0x09,
        Reset = 0x0A,
        Preset = 0x0B
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        Mode mode = (Mode) packet.ReadByte();
        switch (mode)
        {
            case Mode.Close:
                HandleCloseSkillTree(session);
                break;
            case Mode.Save:
                HandleSaveSkillTree(session, packet);
                break;
            case Mode.Reset:
                HandleResetSkillTree(session, packet);
                break;
            case Mode.Preset:
                HandlePresetSkillTree(session, packet);
                break;
            default:
                LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleCloseSkillTree(GameSession session)
    {
        session.Send(JobPacket.Close(session.Player.FieldPlayer));
    }

    private static void HandleSaveSkillTree(GameSession session, PacketReader packet)
    {
        Player player = session.Player;
        SkillTab skillTab = player.SkillTabs.FirstOrDefault(x => x.TabId == player.ActiveSkillTabId);
        if (skillTab is null)
        {
            return;
        }

        ReadSkills(packet, skillTab, out HashSet<int> newSkillIds);

        player.RemoveSkillsFromHotbar();
        player.AddNewSkillsToHotbar(newSkillIds);

        session.Send(JobPacket.Save(player.FieldPlayer, newSkillIds));
        DatabaseManager.SkillTabs.Update(skillTab);

        session.Send(KeyTablePacket.SendHotbars(player.GameOptions));
        DatabaseManager.GameOptions.Update(player.GameOptions);
    }

    private static void HandleResetSkillTree(GameSession session, PacketReader packet)
    {
        int unknown = packet.ReadInt();

        Player player = session.Player;
        SkillTab skillTab = player.SkillTabs.FirstOrDefault(x => x.TabId == player.ActiveSkillTabId);
        if (skillTab is null)
        {
            return;
        }

        skillTab.ResetSkillTree(player.Job, player.JobCode);
        player.RemoveSkillsFromHotbar();

        session.Send(JobPacket.Save(player.FieldPlayer));
        DatabaseManager.SkillTabs.Update(skillTab);

        session.Send(KeyTablePacket.SendHotbars(player.GameOptions));
        DatabaseManager.GameOptions.Update(player.GameOptions);
    }

    private static void HandlePresetSkillTree(GameSession session, PacketReader packet)
    {
        Player player = session.Player;
        SkillTab skillTab = player.SkillTabs.FirstOrDefault(x => x.TabId == player.ActiveSkillTabId);
        if (skillTab is null)
        {
            return;
        }

        skillTab.ResetSkillTree(player.Job, player.JobCode);

        ReadSkills(packet, skillTab, out HashSet<int> newSkillIds);

        player.RemoveSkillsFromHotbar();
        player.AddNewSkillsToHotbar(newSkillIds);

        session.Send(JobPacket.Save(player.FieldPlayer, newSkillIds));
        DatabaseManager.SkillTabs.Update(skillTab);

        session.Send(KeyTablePacket.SendHotbars(player.GameOptions));
        DatabaseManager.GameOptions.Update(player.GameOptions);
    }

    private static void ReadSkills(PacketReader packet, SkillTab skillTab, out HashSet<int> newSkillIds)
    {
        newSkillIds = new();
        int skillCount = packet.ReadInt();
        for (int i = 0; i < skillCount; i++)
        {
            int skillId = packet.ReadInt();
            short skillLevel = packet.ReadShort();
            bool learned = packet.ReadBool();

            if (skillTab.SkillLevels[skillId] == 0 && learned)
            {
                newSkillIds.Add(skillId);
            }

            skillTab.AddOrUpdate(skillId, skillLevel, learned);
        }
    }
}
