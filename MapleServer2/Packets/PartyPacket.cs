using Maple2Storage.Enums;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Enums;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class PartyPacket
{
    private enum Mode : byte
    {
        Notice = 0x0,
        Join = 0x2,
        Leave = 0x3,
        Kick = 0x4,
        LoginNotice = 0x5,
        LogoutNotice = 0x6,
        Disband = 0x7,
        SetLeader = 0x8,
        Create = 0x9,
        Invite = 0xB,
        UpdateMemberLocation = 0xC,
        UpdatePlayer = 0xD,
        UpdateDungeonInfo = 0xE,
        UpdateHitpoints = 0x13,
        PartyHelp = 0x19,
        MatchParty = 0x1A,
        DungeonFindParty = 0x1E,
        DungeonHelperCooldown = 0x28,
        JoinRequest = 0x2C,
        StartReadyCheck = 0x2F,
        ReadyCheck = 0x30,
        EndReadyCheck = 0x31
    }

    public static PacketWriter Notice(Player player, PartyNotice notice)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Party);
        pWriter.Write(Mode.Notice);
        pWriter.Write(notice);
        pWriter.WriteUnicodeString(player.Name);
        return pWriter;
    }

    public static PacketWriter Join(Player player)
    {
        SkillTab skillTab = player.SkillTabs.First(x => x.TabId == player.ActiveSkillTabId);

        PacketWriter pWriter = PacketWriter.Of(SendOp.Party);
        pWriter.Write(Mode.Join);
        pWriter.WriteClass(player);
        pWriter.WriteInt();
        pWriter.WriteSkills(skillTab, SkillType.Active);
        pWriter.WriteSkills(skillTab, SkillType.Passive);
        pWriter.WriteShort(); // more skills?
        pWriter.WriteLong();
        return pWriter;
    }

    public static PacketWriter Leave(Player player, byte self)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Party);
        pWriter.Write(Mode.Leave);
        pWriter.WriteLong(player.CharacterId);
        pWriter.WriteByte(self); //0 = Other leaving, 1 = Self leaving
        return pWriter;
    }

    public static PacketWriter Kick(Player player)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Party);
        pWriter.Write(Mode.Kick);
        pWriter.WriteLong(player.CharacterId);
        return pWriter;
    }

    public static PacketWriter Create(Party party, bool joinNotice)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Party);
        pWriter.Write(Mode.Create);
        pWriter.WriteBool(joinNotice);
        pWriter.WriteInt(party.Id);
        pWriter.WriteLong(party.Leader.CharacterId);
        pWriter.WriteByte((byte) party.Members.Count);

        foreach (Player member in party.Members)
        {
            pWriter.WriteBool(!member.Session?.Connected() ?? false);
            pWriter.WriteClass(member);
            WritePartyDungeonInfo(pWriter);
        }

        pWriter.WriteByte(); // is in dungeon? might be a bool.
        pWriter.WriteInt(); //dungeonid for "enter dungeon"
        pWriter.WriteByte();
        pWriter.WriteByte();
        pWriter.WriteByte();
        return pWriter;
    }

    public static PacketWriter LoginNotice(Player player)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Party);
        pWriter.Write(Mode.LoginNotice);
        pWriter.WriteClass(player);
        pWriter.WriteLong();
        pWriter.WriteInt();
        pWriter.WriteShort();
        pWriter.WriteByte();
        return pWriter;
    }

    public static PacketWriter LogoutNotice(long characterId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Party);
        pWriter.Write(Mode.LogoutNotice);
        pWriter.WriteLong(characterId);
        return pWriter;
    }

    public static PacketWriter Disband()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Party);
        pWriter.Write(Mode.Disband);
        return pWriter;
    }

    public static PacketWriter SetLeader(Player player)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Party);
        pWriter.Write(Mode.SetLeader);
        pWriter.WriteLong(player.CharacterId);
        return pWriter;
    }

    public static PacketWriter SendInvite(Player sender, Party party)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Party);
        pWriter.Write(Mode.Invite);
        pWriter.WriteUnicodeString(sender.Name);
        pWriter.WriteInt(party.Id);
        return pWriter;
    }

    public static PacketWriter UpdateMemberLocation(Player player)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Party);
        pWriter.Write(Mode.UpdateMemberLocation);
        pWriter.WriteLong(player.CharacterId);
        pWriter.WriteClass(player);
        WritePartyDungeonInfo(pWriter);
        return pWriter;
    }

    public static PacketWriter UpdatePlayer(Player player)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Party);
        pWriter.Write(Mode.UpdatePlayer);
        pWriter.WriteLong(player.CharacterId);

        pWriter.WriteClass(player);
        WritePartyDungeonInfo(pWriter);
        return pWriter;
    }

    public static PacketWriter UpdateDungeonInfo(Player player)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Party);
        pWriter.Write(Mode.UpdateDungeonInfo);
        pWriter.WriteLong(player.CharacterId);
        pWriter.WriteInt(); //unknown: but value 100 was frequent
        WritePartyDungeonInfo(pWriter);
        return pWriter;
    }

    public static PacketWriter UpdateHitpoints(Player player)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Party);
        pWriter.Write(Mode.UpdateHitpoints);
        pWriter.WriteLong(player.CharacterId);
        pWriter.WriteLong(player.AccountId);
        pWriter.WriteInt(player.Stats[StatAttribute.Hp].Bonus);
        pWriter.WriteInt(player.Stats[StatAttribute.Hp].Total);
        pWriter.WriteShort();
        return pWriter;
    }

    public static PacketWriter PartyHelp(int dungeonId, byte enabled = 1)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Party);
        pWriter.Write(Mode.PartyHelp);
        pWriter.WriteByte(enabled);
        pWriter.WriteInt(dungeonId);
        return pWriter;
    }

    public static PacketWriter MatchParty(Party party, bool createListing)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Party);
        pWriter.Write(Mode.MatchParty);
        pWriter.WriteBool(createListing);
        if (createListing)
        {
            pWriter.WriteLong(party.PartyFinderId);
            pWriter.WriteInt(party.Id);
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteUnicodeString(party.Name);
            pWriter.WriteBool(party.Approval);
            pWriter.WriteInt(party.Members.Count);
            pWriter.WriteInt(party.RecruitMemberCount);
            pWriter.WriteLong(party.Leader.AccountId);
            pWriter.WriteLong(party.Leader.CharacterId);
            pWriter.WriteUnicodeString(party.Leader.Name);
            pWriter.WriteLong(party.CreationTimestamp);
        }
        else
        {
            pWriter.WriteByte();
        }

        return pWriter;
    }

    public static PacketWriter DungeonFindParty()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Party);
        pWriter.Write(Mode.DungeonFindParty);
        pWriter.WriteInt(); // dungeon queue Id
        return pWriter;
    }

    public static PacketWriter DungeonHelperCooldown(int tickTime)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Party);
        pWriter.Write(Mode.DungeonHelperCooldown);
        pWriter.WriteInt(tickTime);
        return pWriter;
    }

    public static PacketWriter JoinRequest(Player player)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Party);
        pWriter.Write(Mode.JoinRequest);
        pWriter.WriteUnicodeString(player.Name);
        return pWriter;
    }

    public static PacketWriter StartReadyCheck(Player leader, List<Player> members, int count)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Party);
        pWriter.Write(Mode.StartReadyCheck);
        pWriter.WriteByte(2); //unk
        pWriter.WriteInt(count);
        pWriter.WriteLong(TimeInfo.Now() + Environment.TickCount);
        pWriter.WriteInt(members.Count);
        foreach (Player partyMember in members)
        {
            pWriter.WriteLong(partyMember.CharacterId);
        }

        pWriter.WriteInt(1); //unk
        pWriter.WriteLong(leader.CharacterId);
        pWriter.WriteInt(); //unk
        return pWriter;
    }

    public static PacketWriter ReadyCheck(Player player, byte accept)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Party);
        pWriter.Write(Mode.ReadyCheck);
        pWriter.WriteLong(player.CharacterId);
        pWriter.WriteByte(accept);
        return pWriter;
    }

    public static PacketWriter EndReadyCheck()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Party);
        pWriter.Write(Mode.EndReadyCheck);
        return pWriter;
    }

    private static void WritePartyDungeonInfo(PacketWriter pWriter)
    {
        pWriter.WriteInt(1); // dungeon info from player. Dungeon count (loop every dungeon)
        pWriter.WriteInt(); // dungeonID
        pWriter.WriteByte(); // dungeon clear count
    }
}
