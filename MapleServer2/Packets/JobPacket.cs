using Maple2Storage.Enums;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class JobPacket
{
    private enum Mode : byte
    {
        Update = 0x01,
        Unk = 0x02,
        Close = 0x08,
        Save = 0x09,
    }

    public static PacketWriter UpdateSkillTab(IFieldObject<Player> fieldPlayer, HashSet<int> newSkillIds = null)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Job);
        pWriter.WriteInt(fieldPlayer.ObjectId);
        pWriter.Write(Mode.Update);
        pWriter.WriteJobInfo(fieldPlayer.Value, newSkillIds);

        return pWriter;
    }

    public static PacketWriter SendJob(IFieldObject<Player> fieldPlayer)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Job);
        pWriter.WriteInt(fieldPlayer.ObjectId);
        pWriter.Write(Mode.Unk);
        pWriter.WriteJobInfo(fieldPlayer.Value);

        return pWriter;
    }

    public static PacketWriter Close(IFieldActor<Player> fieldPlayer)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Job);

        pWriter.WriteInt(fieldPlayer.ObjectId);
        pWriter.Write(Mode.Close);
        pWriter.WriteJobInfo(fieldPlayer.Value);

        return pWriter;
    }

    public static PacketWriter Save(IFieldActor<Player> fieldPlayer, HashSet<int> newSkillIds = null)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Job);

        pWriter.WriteInt(fieldPlayer.ObjectId);
        pWriter.Write(Mode.Save);
        pWriter.WriteJobInfo(fieldPlayer.Value, newSkillIds);

        return pWriter;
    }

    public static void WriteJobInfo(this PacketWriter pWriter, Player player, HashSet<int> newSkillIds = null)
    {
        SkillTab skillTab = player.SkillTabs.First(x => x.TabId == player.ActiveSkillTabId);

        pWriter.Write(player.JobCode);
        bool flag = true;
        pWriter.WriteBool(flag);
        if (!flag)
        {
            return;
        }

        pWriter.Write(player.Job);
        pWriter.WriteSkills(skillTab, SkillType.Active, newSkillIds);
        pWriter.WriteSkills(skillTab, SkillType.Passive, newSkillIds);
        pWriter.WriteByte(); // More skills?
        pWriter.WriteByte(); // More skills?
    }

    public static void WriteSkills(this PacketWriter pWriter, SkillTab skillTab, SkillType type, HashSet<int> newSkillsId = null)
    {
        List<(int skillId, short skillLevel)> skills = skillTab.GetSkillsByType(type);
        pWriter.WriteByte((byte) skills.Count);

        foreach ((int skillId, short skillLevel) in skills)
        {
            pWriter.WriteBool(newSkillsId?.Contains(skillId) ?? false);
            pWriter.WriteBool(skillLevel > 0); // Is it learned?
            pWriter.WriteInt(skillId);
            pWriter.WriteInt(Math.Max((int) skillLevel, 1));
            pWriter.WriteByte();
        }
    }

    public static void WritePassiveSkills(this PacketWriter pWriter, IFieldObject<Player> fieldPlayer)
    {
        Player player = fieldPlayer.Value;
        SkillTab skillTab = player.SkillTabs.First(x => x.TabId == player.ActiveSkillTabId);

        List<(int skillId, short skillLevel)> passiveSkillList = skillTab.GetSkillsByType(SkillType.Passive);
        pWriter.WriteShort((short) passiveSkillList.Count);

        foreach ((int skillId, short skillLevel) in passiveSkillList)
        {
            pWriter.WriteInt(fieldPlayer.ObjectId);
            pWriter.WriteInt(); // unk int
            pWriter.WriteInt(fieldPlayer.ObjectId);
            pWriter.WriteInt(); // unk int 2
            pWriter.WriteInt(); // same as the unk int 2
            pWriter.WriteInt(skillId);
            pWriter.WriteShort(skillLevel);
            pWriter.WriteInt(1); // unk int = 1
            pWriter.WriteByte(1); // unk byte = 1
            pWriter.WriteLong();
        }
    }
}
