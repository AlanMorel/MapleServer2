﻿using System;
using System.Collections.Generic;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class PartyPacket
    {
        private enum PartyPacketMode : byte
        {
            Join = 0x2,
            Leave = 0x3,
            Kick = 0x4,
            Disband = 0x7,
            SetLeader = 0x8,
            Create = 0x9,
            Invite = 0xB,
            UpdatePlayer = 0xD,
            UpdateHitpoints = 0x13,
            MatchParty = 0x1A,
            StartReadyCheck = 0x2F,
            ReadyCheck = 0x30,
            EndReadyCheck = 0x31
        }

        public static Packet Join(Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PARTY)
                .WriteEnum(PartyPacketMode.Join);

            CharacterListPacket.WriteCharacter(player, pWriter);
            pWriter.WriteInt();
            JobPacket.WriteSkills(pWriter, player);
            pWriter.WriteLong();
            return pWriter;
        }

        public static Packet Leave(Player player, byte self)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PARTY)
                .WriteEnum(PartyPacketMode.Leave)
                .WriteLong(player.CharacterId)
                .WriteByte(self); //0 = Other leaving, 1 = Self leaving
            return pWriter;
        }

        public static Packet Kick(Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PARTY)
                .WriteEnum(PartyPacketMode.Kick)
                .WriteLong(player.CharacterId);
            return pWriter;
        }

        //Generates the header code for Create
        public static void CreatePartyHeader(Player player, PacketWriter pWriter, short members)
        {
            pWriter.WriteEnum(PartyPacketMode.Create)
                .WriteByte()
                .WriteInt()
                .WriteLong(player.CharacterId)
                .WriteShort(members); //# of Party member. but it's scuffed atm
        }

        public static Packet Create(Player leader)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PARTY);

            CreatePartyHeader(leader, pWriter, 1);

            CharacterListPacket.WriteCharacter(leader, pWriter);
            pWriter.WriteLong();
            pWriter.WriteInt();
            pWriter.WriteShort();
            pWriter.WriteByte();
            JobPacket.WriteSkills(pWriter, leader);
            pWriter.WriteLong();
            return pWriter;
        }

        public static Packet CreateExisting(Player leader, List<Player> members)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PARTY);

            CreatePartyHeader(leader, pWriter, (short) members.Count);

            foreach (Player member in members)
            {
                CharacterListPacket.WriteCharacter(member, pWriter);
                pWriter.WriteLong();
                pWriter.WriteInt();
                pWriter.WriteShort();
                pWriter.WriteByte();
                JobPacket.WriteSkills(pWriter, member);
            }

            pWriter.WriteLong();
            return pWriter;
        }

        public static Packet Disband()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PARTY)
                .WriteEnum(PartyPacketMode.Disband);
            return pWriter;
        }

        public static Packet SetLeader(Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PARTY)
                .WriteEnum(PartyPacketMode.SetLeader)
                .WriteLong(player.CharacterId);
            return pWriter;
        }

        public static Packet SendInvite(Player sender)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PARTY)
                .WriteEnum(PartyPacketMode.Invite)
                .WriteUnicodeString(sender.Name)
                .WriteShort() //Unk
                .WriteByte() //Unk
                .WriteByte(); //Unk
            return pWriter;
        }

        public static Packet UpdatePlayer(Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PARTY)
                .WriteEnum(PartyPacketMode.UpdatePlayer)
                .WriteLong(player.CharacterId);

            CharacterListPacket.WriteCharacter(player, pWriter);
            pWriter.WriteInt();
            JobPacket.WriteSkills(pWriter, player);
            pWriter.WriteLong();
            return pWriter;
        }

        public static Packet UpdateHitpoints(Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PARTY)
                .WriteEnum(PartyPacketMode.UpdateHitpoints)
                .WriteLong(player.CharacterId)
                .WriteLong(player.AccountId)
                .WriteInt(player.Stats.Hp.Total)
                .WriteInt(player.Stats.CurrentHp.Min)
                .WriteShort();
            return pWriter;
        }

        public static Packet MatchParty(Party party)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PARTY)
                .WriteEnum(PartyPacketMode.MatchParty);
            if (party == null)
            {
                pWriter.WriteByte();
            }
            else
            {
                MatchPartyPacket.WritePartyInformation(pWriter, party);
            }

            return pWriter;
        }

        public static Packet StartReadyCheck(Player leader, List<Player> members, int count)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PARTY)
                .WriteEnum(PartyPacketMode.StartReadyCheck)
                .WriteByte(2) //unk
                .WriteInt(count)
                .WriteLong(DateTimeOffset.Now.ToUnixTimeSeconds() + Environment.TickCount)
                .WriteInt(members.Count);
            foreach (Player partyMember in members)
            {
                pWriter.WriteLong(partyMember.CharacterId);
            }

            pWriter.WriteInt(1) //unk
                .WriteLong(leader.CharacterId)
                .WriteInt(); //unk
            return pWriter;
        }

        public static Packet ReadyCheck(Player player, byte accept)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PARTY)
                .WriteEnum(PartyPacketMode.ReadyCheck)
                .WriteLong(player.CharacterId)
                .WriteByte(accept);
            return pWriter;
        }

        public static Packet EndReadyCheck()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PARTY)
                .WriteEnum(PartyPacketMode.EndReadyCheck);
            return pWriter;
        }
    }
}
