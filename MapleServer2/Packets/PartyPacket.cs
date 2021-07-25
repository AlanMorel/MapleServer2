using System;
using System.Collections.Generic;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Enums;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class PartyPacket
    {
        private enum PartyPacketMode : byte
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
            UpdatePlayer = 0xD,
            UpdateDungeonInfo = 0xE,
            UpdateHitpoints = 0x13,
            PartyHelp = 0x19,
            MatchParty = 0x1A,
            DungeonFindParty = 0x1E,
            JoinRequest = 0x2C,
            StartReadyCheck = 0x2F,
            ReadyCheck = 0x30,
            EndReadyCheck = 0x31
        }

        public static Packet Notice(Player player, PartyNotice notice)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PARTY);
            pWriter.WriteEnum(PartyPacketMode.Notice);
            pWriter.WriteEnum(notice);
            pWriter.WriteUnicodeString(player.Name);
            return pWriter;
        }

        public static Packet Join(Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PARTY);
            pWriter.WriteEnum(PartyPacketMode.Join);
            CharacterListPacket.WriteCharacter(player, pWriter);
            pWriter.WriteInt();
            JobPacket.WriteSkills(pWriter, player);
            pWriter.WriteLong();
            return pWriter;
        }

        public static Packet Leave(Player player, byte self)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PARTY);
            pWriter.WriteEnum(PartyPacketMode.Leave);
            pWriter.WriteLong(player.CharacterId);
            pWriter.WriteByte(self); //0 = Other leaving, 1 = Self leaving
            return pWriter;
        }

        public static Packet Kick(Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PARTY);
            pWriter.WriteEnum(PartyPacketMode.Kick);
            pWriter.WriteLong(player.CharacterId);
            return pWriter;
        }

        public static Packet Create(Party party, bool joinNotice)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PARTY);
            pWriter.WriteEnum(PartyPacketMode.Create);
            pWriter.WriteBool(joinNotice);
            pWriter.WriteInt(party.Id);
            pWriter.WriteLong(party.Leader.CharacterId);
            pWriter.WriteByte((byte) party.Members.Count);

            foreach (Player member in party.Members)
            {
                pWriter.WriteBool(!member.Session.Connected());
                CharacterListPacket.WriteCharacter(member, pWriter);
                WritePartyDungeonInfo(pWriter);
            }

            pWriter.WriteByte(); // is in dungeon? might be a bool.
            pWriter.WriteInt(); //dungeonid for "enter dungeon"
            pWriter.WriteByte();
            pWriter.WriteByte();
            pWriter.WriteByte();
            return pWriter;
        }

        public static Packet LoginNotice(Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PARTY);
            pWriter.WriteEnum(PartyPacketMode.LoginNotice);
            CharacterListPacket.WriteCharacter(player, pWriter);
            pWriter.WriteLong();
            pWriter.WriteInt();
            pWriter.WriteShort();
            pWriter.WriteByte();
            return pWriter;
        }

        public static Packet LogoutNotice(long characterId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PARTY);
            pWriter.WriteEnum(PartyPacketMode.LogoutNotice);
            pWriter.WriteLong(characterId);
            return pWriter;
        }

        public static Packet Disband()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PARTY);
            pWriter.WriteEnum(PartyPacketMode.Disband);
            return pWriter;
        }

        public static Packet SetLeader(Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PARTY);
            pWriter.WriteEnum(PartyPacketMode.SetLeader);
            pWriter.WriteLong(player.CharacterId);
            return pWriter;
        }

        public static Packet SendInvite(Player sender, Party party)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PARTY);
            pWriter.WriteEnum(PartyPacketMode.Invite);
            pWriter.WriteUnicodeString(sender.Name);
            pWriter.WriteInt(party.Id);
            return pWriter;
        }

        public static Packet UpdatePlayer(Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PARTY);
            pWriter.WriteEnum(PartyPacketMode.UpdatePlayer);
            pWriter.WriteLong(player.CharacterId);

            CharacterListPacket.WriteCharacter(player, pWriter);
            WritePartyDungeonInfo(pWriter);
            return pWriter;
        }

        public static Packet UpdateDungeonInfo(Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PARTY);
            pWriter.WriteEnum(PartyPacketMode.UpdateDungeonInfo);
            pWriter.WriteLong(player.CharacterId);
            pWriter.WriteInt(); //unknown: but value 100 was frequent
            WritePartyDungeonInfo(pWriter);
            return pWriter;
        }

        public static Packet UpdateHitpoints(Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PARTY);
            pWriter.WriteEnum(PartyPacketMode.UpdateHitpoints);
            pWriter.WriteLong(player.CharacterId);
            pWriter.WriteLong(player.AccountId);
            pWriter.WriteInt(player.Stats[PlayerStatId.Hp].Max);
            pWriter.WriteInt(player.Stats[PlayerStatId.Hp].Current);
            pWriter.WriteShort();
            return pWriter;
        }

        public static Packet PartyHelp(int dungeonId, byte enabled = 1)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PARTY);
            pWriter.WriteEnum(PartyPacketMode.PartyHelp);
            pWriter.WriteByte(enabled);
            pWriter.WriteInt(dungeonId);
            return pWriter;
        }

        public static Packet MatchParty(Party party, bool createListing)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PARTY);
            pWriter.WriteEnum(PartyPacketMode.MatchParty);
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
                pWriter.WriteByte(0);
            }

            return pWriter;
        }

        public static Packet DungeonFindParty()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PARTY);
            pWriter.WriteEnum(PartyPacketMode.DungeonFindParty);
            pWriter.WriteInt(); // dungeon queue Id
            return pWriter;
        }

        public static Packet JoinRequest(Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PARTY);
            pWriter.WriteEnum(PartyPacketMode.JoinRequest);
            pWriter.WriteUnicodeString(player.Name);
            return pWriter;
        }

        public static Packet StartReadyCheck(Player leader, List<Player> members, int count)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PARTY);
            pWriter.WriteEnum(PartyPacketMode.StartReadyCheck);
            pWriter.WriteByte(2); //unk
            pWriter.WriteInt(count);
            pWriter.WriteLong(DateTimeOffset.Now.ToUnixTimeSeconds() + Environment.TickCount);
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

        public static Packet ReadyCheck(Player player, byte accept)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PARTY);
            pWriter.WriteEnum(PartyPacketMode.ReadyCheck);
            pWriter.WriteLong(player.CharacterId);
            pWriter.WriteByte(accept);
            return pWriter;
        }

        public static Packet EndReadyCheck()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PARTY);
            pWriter.WriteEnum(PartyPacketMode.EndReadyCheck);
            return pWriter;
        }

        private static void WritePartyDungeonInfo(PacketWriter pWriter)
        {
            pWriter.WriteInt(1); // dungeon info from player. Dungeon count (loop every dungeon)
            pWriter.WriteInt(); // dungeonID
            pWriter.WriteByte(); // dungeon clear count
        }
    }
}
